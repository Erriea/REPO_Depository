using System;
using System.Text;
using CaseFileLocalSuspect.AI;
using CaseFileLocalSuspect.UI;
using UnityEngine;

namespace CaseFileLocalSuspect.Game
{
    public class GameManager : MonoBehaviour
    {
        private const int MaxVisibleConversationCharacters = 1800;
        private const int QuestionsPerSuspect = 2;

        [SerializeField] private UIManager uiManager;
        [SerializeField] private OllamaClient ollamaClient;
        [SerializeField] private bool useOllamaForCaseGeneration = true;

        private CaseFile currentCaseFile;
        private StringBuilder[] suspectConversationLogs;
        private bool[,] askedQuestionMatrix;
        private bool[] introducedSuspects;
        private int selectedSuspectIndex;
        private int selectedAccusationIndex;
        private int questionsRemaining;
        private bool isGeneratingCase;
        private int generatedCaseCount;

        public GameScreen CurrentScreen { get; private set; } = GameScreen.MainMenu;
        public const int StartingQuestionCount = 6;

        [Serializable]
        private class CaseFileJson
        {
            public string case_title;
            public string crime;
            public string victim;
            public string location;
            public SuspectJson[] suspects;
            public string guilty_suspect;
            public string key_clue;
            public string explanation;
        }

        [Serializable]
        private class SuspectJson
        {
            public string name;
            public string role;
            public string connection_to_case;
            public string motive;
            public string alibi;
            public string personality;
            public string opening_statement;
            public FollowUpQuestion[] questions;
        }

        private void Awake()
        {
            EnsureOllamaClient();
        }

        private void Start()
        {
            ShowScreen(GameScreen.MainMenu);
        }

        public void StartNewCase()
        {
            if (isGeneratingCase)
            {
                return;
            }

            ResetCaseState();
            EnsureOllamaClient();

            if (!useOllamaForCaseGeneration || ollamaClient == null)
            {
                LoadFallbackCase("Ollama case generation is unavailable. Loaded the fallback case instead.");
                return;
            }

            isGeneratingCase = true;
            generatedCaseCount++;
            int varietySeed = UnityEngine.Random.Range(1000, 999999);
            ShowScreen(GameScreen.CaseBriefing);
            ollamaClient.GenerateStructuredJson(
                PromptBuilder.BuildCaseGenerationPrompt(generatedCaseCount, varietySeed),
                PromptBuilder.BuildCaseGenerationSchema(),
                HandleCaseGenerationSuccess,
                HandleCaseGenerationError);
        }

        public void BeginInterrogation()
        {
            if (!HasCase())
            {
                return;
            }

            EnsureSuspectIntroduction(selectedSuspectIndex);
            RefreshInterrogationUI();
            ShowScreen(GameScreen.Interrogation);
        }

        public void SelectSuspect(int suspectIndex)
        {
            if (!HasCase() || !IsValidSuspectIndex(suspectIndex))
            {
                return;
            }

            selectedSuspectIndex = suspectIndex;
            EnsureSuspectIntroduction(selectedSuspectIndex);
            RefreshInterrogationUI();
        }

        public void SubmitQuestion(string question)
        {
            if (!HasCase())
            {
                return;
            }

            if (questionsRemaining <= 0)
            {
                uiManager.ShowInterrogationMessage("All six follow-up questions have been used. Make your accusation.");
                return;
            }

            if (!TryResolveQuestionIndex(question, out int questionIndex))
            {
                uiManager.ShowInterrogationMessage(BuildQuestionPrompt(currentCaseFile.suspects[selectedSuspectIndex], selectedSuspectIndex));
                return;
            }

            AskQuestion(questionIndex);
        }

        public void OpenAccusation()
        {
            if (!HasCase())
            {
                return;
            }

            uiManager.ShowAccusation(currentCaseFile, selectedAccusationIndex);
            ShowScreen(GameScreen.Accusation);
        }

        public void SelectAccusation(int suspectIndex)
        {
            if (!HasCase() || !IsValidSuspectIndex(suspectIndex))
            {
                return;
            }

            selectedAccusationIndex = suspectIndex;
            uiManager.ShowAccusation(currentCaseFile, selectedAccusationIndex);
        }

        public void ReturnToInterrogation()
        {
            RefreshInterrogationUI();
            ShowScreen(GameScreen.Interrogation);
        }

        public void ConfirmAccusation()
        {
            if (!HasCase())
            {
                return;
            }

            bool isCorrect = string.Equals(
                currentCaseFile.suspects[selectedAccusationIndex].name,
                currentCaseFile.guiltySuspect,
                StringComparison.OrdinalIgnoreCase);

            uiManager.ShowResult(currentCaseFile, selectedAccusationIndex, isCorrect);
            ShowScreen(GameScreen.Result);
        }

        public void ReturnToMainMenu()
        {
            ShowScreen(GameScreen.MainMenu);
        }

        public void QuitGame()
        {
            Debug.Log("Quit requested from Main Menu.");
            Application.Quit();
        }

        public void ShowScreen(GameScreen screen)
        {
            CurrentScreen = screen;

            if (uiManager == null)
            {
                Debug.LogWarning("GameManager is missing a UIManager reference.");
                return;
            }

            uiManager.ShowScreen(screen);
        }

        private void AskQuestion(int questionIndex)
        {
            if (!HasCase() || questionIndex < 0 || questionIndex >= QuestionsPerSuspect)
            {
                return;
            }

            if (askedQuestionMatrix[selectedSuspectIndex, questionIndex])
            {
                uiManager.ShowInterrogationMessage("That follow-up question has already been used. Pick the remaining one.");
                return;
            }

            Suspect suspect = currentCaseFile.suspects[selectedSuspectIndex];
            if (suspect.followUpQuestions == null || questionIndex >= suspect.followUpQuestions.Length)
            {
                uiManager.ShowInterrogationMessage("This suspect has no more valid follow-up questions.");
                return;
            }

            FollowUpQuestion selectedQuestion = suspect.followUpQuestions[questionIndex];
            askedQuestionMatrix[selectedSuspectIndex, questionIndex] = true;
            questionsRemaining = Mathf.Max(questionsRemaining - 1, 0);

            AppendConversationLine(selectedSuspectIndex, "Detective", selectedQuestion.question);
            AppendConversationLine(selectedSuspectIndex, suspect.name, selectedQuestion.answer);

            uiManager.ClearQuestionInput();
            RefreshInterrogationUI();
        }

        private void RefreshInterrogationUI()
        {
            if (!HasCase())
            {
                return;
            }

            Suspect selectedSuspect = currentCaseFile.suspects[selectedSuspectIndex];
            bool currentSuspectHasQuestions = HasUnaskedQuestions(selectedSuspectIndex);

            uiManager.ShowInterrogation(
                currentCaseFile,
                selectedSuspectIndex,
                questionsRemaining,
                GetVisibleConversationHistory(selectedSuspectIndex),
                BuildQuestionPrompt(selectedSuspect, selectedSuspectIndex),
                questionsRemaining > 0 && currentSuspectHasQuestions);
        }

        private void HandleCaseGenerationSuccess(string rawResponse)
        {
            isGeneratingCase = false;

            if (TryParseGeneratedCase(rawResponse, out CaseFile generatedCase, out string parseError))
            {
                Debug.Log($"Ollama case generation succeeded. Loaded case: {generatedCase.caseTitle}");
                LoadCase(generatedCase, "Case generated with Ollama. Review the briefing, then begin the interrogation.");
                return;
            }

            Debug.LogWarning($"Generated case could not be parsed. {parseError}\nRaw response:\n{rawResponse}");
            LoadFallbackCase($"Ollama returned an unusable case, so the fallback case was loaded instead. {parseError}");
        }

        private void HandleCaseGenerationError(string error)
        {
            isGeneratingCase = false;
            Debug.LogWarning($"Ollama case generation failed: {error}");
            LoadFallbackCase($"Ollama case generation failed, so the fallback case was loaded instead. {error}");
        }

        private void LoadFallbackCase(string systemMessage)
        {
            LoadCase(FallbackCaseProvider.CreateCase(), systemMessage);
        }

        private void LoadCase(CaseFile caseFile, string systemMessage)
        {
            currentCaseFile = caseFile;
            selectedSuspectIndex = 0;
            selectedAccusationIndex = 0;
            questionsRemaining = StartingQuestionCount;
            InitializeInterrogationState(caseFile);
            AppendConversationLine(selectedSuspectIndex, "System", systemMessage);
            uiManager.ShowCaseBriefing(currentCaseFile);
            ShowScreen(GameScreen.CaseBriefing);
        }

        private void InitializeInterrogationState(CaseFile caseFile)
        {
            int suspectCount = caseFile != null && caseFile.suspects != null ? caseFile.suspects.Length : 0;
            suspectConversationLogs = new StringBuilder[suspectCount];
            introducedSuspects = new bool[suspectCount];
            askedQuestionMatrix = new bool[suspectCount, QuestionsPerSuspect];

            for (int i = 0; i < suspectCount; i++)
            {
                suspectConversationLogs[i] = new StringBuilder();
            }
        }

        private void ResetCaseState()
        {
            currentCaseFile = null;
            suspectConversationLogs = null;
            introducedSuspects = null;
            askedQuestionMatrix = null;
            selectedSuspectIndex = 0;
            selectedAccusationIndex = 0;
            questionsRemaining = StartingQuestionCount;
        }

        private void EnsureOllamaClient()
        {
            if (ollamaClient != null)
            {
                return;
            }

            ollamaClient = FindFirstObjectByType<OllamaClient>();
            if (ollamaClient != null)
            {
                return;
            }

            GameObject clientObject = new GameObject("OllamaClient");
            ollamaClient = clientObject.AddComponent<OllamaClient>();
        }

        private void EnsureSuspectIntroduction(int suspectIndex)
        {
            if (!IsValidSuspectIndex(suspectIndex) || introducedSuspects[suspectIndex])
            {
                return;
            }

            Suspect suspect = currentCaseFile.suspects[suspectIndex];
            AppendConversationLine(suspectIndex, suspect.name, suspect.openingStatement);
            introducedSuspects[suspectIndex] = true;
        }

        private static bool TryParseGeneratedCase(string rawResponse, out CaseFile caseFile, out string error)
        {
            caseFile = null;
            string cleanedJson = ExtractJsonObject(rawResponse);

            if (string.IsNullOrWhiteSpace(cleanedJson))
            {
                error = "The response did not contain a valid JSON object.";
                return false;
            }

            CaseFileJson parsedJson;
            try
            {
                parsedJson = JsonUtility.FromJson<CaseFileJson>(cleanedJson);
            }
            catch (Exception exception)
            {
                error = $"Unity could not parse the generated JSON. {exception.Message}";
                return false;
            }

            if (parsedJson == null || parsedJson.suspects == null || parsedJson.suspects.Length != 3)
            {
                error = "The generated case did not contain exactly three suspects.";
                return false;
            }

            string guiltySuspectName = CleanValue(parsedJson.guilty_suspect);
            if (string.IsNullOrWhiteSpace(guiltySuspectName))
            {
                error = "The generated case was missing a guilty suspect name.";
                return false;
            }

            string[] suspectPortraitIds = { "FemCharacter1", "FemCharacter2", "MascCharacter2" };
            Suspect[] suspects = new Suspect[parsedJson.suspects.Length];

            for (int i = 0; i < parsedJson.suspects.Length; i++)
            {
                SuspectJson suspectJson = parsedJson.suspects[i];
                if (suspectJson == null)
                {
                    error = $"Suspect {i + 1} was empty.";
                    return false;
                }

                string suspectName = CleanValue(suspectJson.name);
                if (string.IsNullOrWhiteSpace(suspectName))
                {
                    error = $"Suspect {i + 1} is missing a name.";
                    return false;
                }

                if (suspectJson.questions == null || suspectJson.questions.Length != QuestionsPerSuspect)
                {
                    error = $"Suspect {suspectName} did not contain exactly two follow-up questions.";
                    return false;
                }

                FollowUpQuestion[] followUpQuestions = new FollowUpQuestion[QuestionsPerSuspect];
                for (int questionIndex = 0; questionIndex < QuestionsPerSuspect; questionIndex++)
                {
                    FollowUpQuestion rawQuestion = suspectJson.questions[questionIndex];
                    string questionText = CleanValue(rawQuestion != null ? rawQuestion.question : string.Empty);
                    string answerText = CleanValue(rawQuestion != null ? rawQuestion.answer : string.Empty);

                    if (string.IsNullOrWhiteSpace(questionText) || string.IsNullOrWhiteSpace(answerText))
                    {
                        error = $"Suspect {suspectName} has an incomplete follow-up question set.";
                        return false;
                    }

                    followUpQuestions[questionIndex] = new FollowUpQuestion
                    {
                        question = questionText,
                        answer = answerText
                    };
                }

                suspects[i] = new Suspect
                {
                    name = suspectName,
                    portraitId = suspectPortraitIds[i],
                    role = CleanValue(suspectJson.role, "Unknown role"),
                    connectionToCase = CleanValue(suspectJson.connection_to_case, "Connection unknown."),
                    motive = CleanValue(suspectJson.motive, "No motive provided."),
                    alibi = CleanValue(suspectJson.alibi, "No alibi provided."),
                    personality = CleanValue(suspectJson.personality, "Reserved"),
                    openingStatement = CleanValue(suspectJson.opening_statement, "I do not know what else I can add right now, detective."),
                    followUpQuestions = followUpQuestions
                };
            }

            bool guiltySuspectMatches = false;
            for (int i = 0; i < suspects.Length; i++)
            {
                if (string.Equals(suspects[i].name, guiltySuspectName, StringComparison.OrdinalIgnoreCase))
                {
                    guiltySuspectName = suspects[i].name;
                    guiltySuspectMatches = true;
                    break;
                }
            }

            if (!guiltySuspectMatches)
            {
                error = "The guilty suspect name did not match any generated suspect.";
                return false;
            }

            caseFile = new CaseFile
            {
                caseTitle = CleanValue(parsedJson.case_title, "Untitled Case"),
                crime = CleanValue(parsedJson.crime, "No crime summary was provided."),
                victim = CleanValue(parsedJson.victim, "Unknown victim"),
                victimPortraitId = "MascCharacter1",
                location = CleanValue(parsedJson.location, "Unknown location"),
                suspects = suspects,
                guiltySuspect = guiltySuspectName,
                keyClue = CleanValue(parsedJson.key_clue, "No key clue was provided."),
                explanation = CleanValue(parsedJson.explanation, "No explanation was provided.")
            };

            error = null;
            return true;
        }

        private static string ExtractJsonObject(string rawResponse)
        {
            if (string.IsNullOrWhiteSpace(rawResponse))
            {
                return string.Empty;
            }

            string cleaned = rawResponse.Trim();
            cleaned = cleaned.Replace("```json", string.Empty).Replace("```", string.Empty).Trim();

            int firstBrace = cleaned.IndexOf('{');
            int lastBrace = cleaned.LastIndexOf('}');

            if (firstBrace < 0 || lastBrace <= firstBrace)
            {
                return string.Empty;
            }

            return cleaned.Substring(firstBrace, lastBrace - firstBrace + 1).Trim();
        }

        private static string CleanValue(string value, string fallback = "")
        {
            return string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
        }

        private string BuildQuestionPrompt(Suspect suspect, int suspectIndex)
        {
            if (suspect == null || suspect.followUpQuestions == null)
            {
                return "This suspect has no generated follow-up questions.";
            }

            StringBuilder builder = new StringBuilder();

            if (questionsRemaining <= 0)
            {
                builder.Append("All six follow-up questions have been used. Make your accusation.");
                return builder.ToString();
            }

            builder.AppendLine("Type 1 or 2 in the question box, or paste the exact question text.");

            for (int i = 0; i < suspect.followUpQuestions.Length; i++)
            {
                string status = askedQuestionMatrix[suspectIndex, i] ? " [asked]" : string.Empty;
                builder.Append(i + 1);
                builder.Append(". ");
                builder.Append(suspect.followUpQuestions[i].question);
                builder.Append(status);

                if (i < suspect.followUpQuestions.Length - 1)
                {
                    builder.AppendLine();
                }
            }

            if (!HasUnaskedQuestions(suspectIndex))
            {
                builder.AppendLine();
                builder.Append("This suspect is done. Switch to another suspect or open the accusation screen.");
            }

            return builder.ToString();
        }

        private bool TryResolveQuestionIndex(string questionInput, out int questionIndex)
        {
            questionIndex = -1;

            if (!HasCase())
            {
                return false;
            }

            Suspect suspect = currentCaseFile.suspects[selectedSuspectIndex];
            if (suspect == null || suspect.followUpQuestions == null)
            {
                return false;
            }

            string trimmedInput = string.IsNullOrWhiteSpace(questionInput) ? string.Empty : questionInput.Trim();
            if (trimmedInput.Length == 0)
            {
                return false;
            }

            if (int.TryParse(trimmedInput, out int numericChoice))
            {
                int resolvedIndex = numericChoice - 1;
                if (resolvedIndex >= 0 && resolvedIndex < suspect.followUpQuestions.Length)
                {
                    questionIndex = resolvedIndex;
                    return true;
                }
            }

            for (int i = 0; i < suspect.followUpQuestions.Length; i++)
            {
                if (string.Equals(trimmedInput, suspect.followUpQuestions[i].question, StringComparison.OrdinalIgnoreCase))
                {
                    questionIndex = i;
                    return true;
                }
            }

            return false;
        }

        private string GetVisibleConversationHistory(int suspectIndex)
        {
            if (suspectConversationLogs == null || suspectIndex < 0 || suspectIndex >= suspectConversationLogs.Length || suspectConversationLogs[suspectIndex] == null)
            {
                return string.Empty;
            }

            string fullHistory = suspectConversationLogs[suspectIndex].ToString();
            if (fullHistory.Length <= MaxVisibleConversationCharacters)
            {
                return fullHistory;
            }

            return "...\n\n" + fullHistory.Substring(fullHistory.Length - MaxVisibleConversationCharacters);
        }

        private void AppendConversationLine(int suspectIndex, string speaker, string text)
        {
            if (suspectConversationLogs == null || suspectIndex < 0 || suspectIndex >= suspectConversationLogs.Length)
            {
                return;
            }

            StringBuilder conversationLog = suspectConversationLogs[suspectIndex];
            if (conversationLog.Length > 0)
            {
                conversationLog.AppendLine();
                conversationLog.AppendLine();
            }

            conversationLog.Append(speaker);
            conversationLog.Append(": ");
            conversationLog.Append(text);
        }

        private bool HasCase()
        {
            return currentCaseFile != null && currentCaseFile.suspects != null && currentCaseFile.suspects.Length == 3;
        }

        private bool HasUnaskedQuestions(int suspectIndex)
        {
            if (!IsValidSuspectIndex(suspectIndex))
            {
                return false;
            }

            for (int i = 0; i < QuestionsPerSuspect; i++)
            {
                if (!askedQuestionMatrix[suspectIndex, i])
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsValidSuspectIndex(int suspectIndex)
        {
            return currentCaseFile != null
                && currentCaseFile.suspects != null
                && suspectIndex >= 0
                && suspectIndex < currentCaseFile.suspects.Length;
        }
    }
}
