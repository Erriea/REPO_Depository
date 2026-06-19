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
        private const int QuestionCount = 4;
        private const float TimedCaseDurationSeconds = 60f;

        [SerializeField] private UIManager uiManager;
        [SerializeField] private OllamaClient ollamaClient;
        [SerializeField] private bool useOllamaForCaseGeneration = true;

        private CaseFile currentCaseFile;
        private StringBuilder[] suspectConversationLogs;
        private bool[] askedQuestionFlags;
        private bool[] introducedSuspects;
        private bool viewedCrimeScreen;
        private bool viewedSuspectsScreen;
        private bool completedInterrogationStep;
        private int selectedSuspectIndex;
        private int selectedAccusationIndex;
        private bool isGeneratingCase;
        private int generatedCaseCount;
        private string previousCaseTitle;
        private int previousGuiltySuspectIndex = -1;
        private int requiredGuiltySuspectIndex = -1;
        private string currentSystemMessage;
        private bool requestedTimedMode;
        private bool isTimedMode;
        private bool timedCaseExpired;
        private float timedCaseRemainingSeconds;

        public GameScreen CurrentScreen { get; private set; } = GameScreen.MainMenu;

        [Serializable]
        private class CaseFileJson
        {
            public string case_title;
            public string board_summary;
            public string victim;
            public string victim_description;
            public string location;
            public string crime;
            public string[] on_site_clues;
            public string[] interrogation_questions;
            public SuspectJson[] suspects;
            public string guilty_suspect;
            public string[] guilt_clues;
            public string explanation;
        }

        [Serializable]
        private class SuspectJson
        {
            public string name;
            public string role;
            public string description;
            public string appearance;
            public string connection_to_case;
            public string last_seen_victim;
            public string motive;
            public string alibi;
            public string personality;
            public string opening_statement;
            public string[] interrogation_answers;
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
            BeginCase(false);
        }

        public void StartTimedCase()
        {
            BeginCase(true);
        }

        private void Update()
        {
            TickTimedCaseClock();
        }

        private void BeginCase(bool timedMode)
        {
            if (isGeneratingCase)
            {
                return;
            }

            ResetCaseState();
            requestedTimedMode = timedMode;
            EnsureOllamaClient();

            if (!useOllamaForCaseGeneration || ollamaClient == null)
            {
                LoadFallbackCase("Ollama case generation is unavailable. Loaded the fallback case instead.");
                return;
            }

            isGeneratingCase = true;
            generatedCaseCount++;
            requiredGuiltySuspectIndex = generatedCaseCount % 3;
            int varietySeed = UnityEngine.Random.Range(1000, 999999);
            currentSystemMessage = "Generating a new case through Ollama. The first request may take a while if the model is still warming up.";
            uiManager.ShowCrimeBoard(CreateLoadingCase(), false, false, false, false, QuestionCount, currentSystemMessage);
            ShowScreen(GameScreen.CrimeBoard);
            ollamaClient.GenerateStructuredJson(
                PromptBuilder.BuildCaseGenerationPrompt(generatedCaseCount, varietySeed, previousCaseTitle, previousGuiltySuspectIndex, requiredGuiltySuspectIndex),
                PromptBuilder.BuildCaseGenerationSchema(),
                HandleCaseGenerationSuccess,
                HandleCaseGenerationError);
        }

        public void OpenCrimeScreen()
        {
            if (!HasCase())
            {
                return;
            }

            if (timedCaseExpired)
            {
                ForceTimedCaseArrest();
                return;
            }

            viewedCrimeScreen = true;
            uiManager.ShowCrimeDetails(currentCaseFile);
            ShowScreen(GameScreen.Crime);
        }

        public void OpenSuspectsScreen()
        {
            if (!HasCase())
            {
                return;
            }

            if (timedCaseExpired)
            {
                ForceTimedCaseArrest();
                return;
            }

            viewedSuspectsScreen = true;
            uiManager.ShowSuspects(currentCaseFile);
            ShowScreen(GameScreen.Suspects);
        }

        public void OpenInterrogationScreen()
        {
            if (!HasCase())
            {
                return;
            }

            if (timedCaseExpired)
            {
                ForceTimedCaseArrest();
                return;
            }

            EnsureSuspectIntroduction(selectedSuspectIndex);
            RefreshInterrogationUI();
            ShowScreen(GameScreen.Interrogation);
        }

        public void ReturnToCrimeBoard()
        {
            if (!HasCase())
            {
                ShowScreen(GameScreen.MainMenu);
                return;
            }

            if (timedCaseExpired)
            {
                ForceTimedCaseArrest();
                return;
            }

            RefreshCrimeBoardUI();
            ShowScreen(GameScreen.CrimeBoard);
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

        public void AskQuestion(int questionIndex)
        {
            if (!HasCase() || questionIndex < 0 || questionIndex >= QuestionCount)
            {
                return;
            }

            if (timedCaseExpired)
            {
                ForceTimedCaseArrest();
                return;
            }

            if (askedQuestionFlags[questionIndex])
            {
                uiManager.ShowInterrogationMessage("That question has already been used. Choose one of the remaining questions.");
                return;
            }

            Suspect suspect = currentCaseFile.suspects[selectedSuspectIndex];
            if (suspect.interrogationAnswers == null || questionIndex >= suspect.interrogationAnswers.Length)
            {
                uiManager.ShowInterrogationMessage("This suspect does not have a valid answer for that question.");
                return;
            }

            string question = currentCaseFile.interrogationQuestions[questionIndex];
            string answer = suspect.interrogationAnswers[questionIndex];

            askedQuestionFlags[questionIndex] = true;
            completedInterrogationStep = true;

            AppendConversationLine(selectedSuspectIndex, "Detective", question);
            AppendConversationLine(selectedSuspectIndex, suspect.name, answer);

            RefreshInterrogationUI();
        }

        public void OpenAccusation()
        {
            if (!HasCase() || !IsArrestUnlocked())
            {
                return;
            }

            if (!IsValidSuspectIndex(selectedAccusationIndex))
            {
                selectedAccusationIndex = -1;
            }

            uiManager.ShowAccusation(currentCaseFile, selectedAccusationIndex, timedCaseExpired);
            ShowScreen(GameScreen.Arrest);
        }

        public void SelectAccusation(int suspectIndex)
        {
            if (!HasCase() || !IsValidSuspectIndex(suspectIndex))
            {
                return;
            }

            selectedAccusationIndex = suspectIndex;
            uiManager.ShowAccusation(currentCaseFile, selectedAccusationIndex, timedCaseExpired);
        }

        public void ReturnToInterrogation()
        {
            RefreshInterrogationUI();
            ShowScreen(GameScreen.Interrogation);
        }

        public void ConfirmAccusation()
        {
            if (!HasCase() || !IsValidSuspectIndex(selectedAccusationIndex))
            {
                if (HasCase())
                {
                    uiManager.ShowAccusation(currentCaseFile, selectedAccusationIndex, timedCaseExpired);
                }
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
            ResetCaseState();
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
            RefreshTimedCaseUI();
        }

        private void RefreshCrimeBoardUI()
        {
            uiManager.ShowCrimeBoard(
                currentCaseFile,
                viewedCrimeScreen,
                viewedSuspectsScreen,
                completedInterrogationStep,
                IsArrestUnlocked(),
                GetRemainingQuestionCount(),
                BuildSystemMessage());
        }

        private void RefreshInterrogationUI()
        {
            if (!HasCase())
            {
                return;
            }

            Suspect selectedSuspect = currentCaseFile.suspects[selectedSuspectIndex];
            uiManager.ShowInterrogation(
                currentCaseFile,
                selectedSuspectIndex,
                GetRemainingQuestionCount(),
                GetVisibleConversationHistory(selectedSuspectIndex),
                completedInterrogationStep,
                askedQuestionFlags);
            uiManager.ShowInterrogationMessage(
                selectedSuspect != null
                    ? $"Choose one remaining question to ask {selectedSuspect.name}. Used questions are locked for the rest of the case."
                    : "Choose a suspect, then choose a question.");
        }

        private void HandleCaseGenerationSuccess(string rawResponse)
        {
            isGeneratingCase = false;

            if (TryParseGeneratedCase(rawResponse, out CaseFile generatedCase, out string parseError))
            {
                Debug.Log($"Ollama case generation succeeded. Loaded case: {generatedCase.caseTitle}");
                LoadCase(generatedCase, "Ollama case generation succeeded.");
                return;
            }

            Debug.LogWarning($"Generated case could not be parsed. {parseError}\nRaw response:\n{rawResponse}");
            LoadFallbackCase($"Ollama returned unusable case data, so the fallback case was loaded instead. {parseError}");
        }

        private void HandleCaseGenerationError(string error)
        {
            isGeneratingCase = false;
            Debug.LogWarning($"Ollama case generation failed: {error}");
            LoadFallbackCase($"Ollama case generation failed, so the fallback case was loaded instead. {error}");
        }

        private void LoadFallbackCase(string systemMessage)
        {
            CaseFile fallbackCase = FallbackCaseProvider.CreateCase(generatedCaseCount);
            LoadCase(fallbackCase, systemMessage);
        }

        private void LoadCase(CaseFile caseFile, string systemMessage = null)
        {
            currentCaseFile = caseFile;
            previousCaseTitle = caseFile != null ? caseFile.caseTitle : previousCaseTitle;
            currentSystemMessage = string.IsNullOrWhiteSpace(systemMessage) ? string.Empty : systemMessage;
            previousGuiltySuspectIndex = FindSuspectIndex(caseFile, caseFile != null ? caseFile.guiltySuspect : null);
            isTimedMode = requestedTimedMode;
            timedCaseExpired = false;
            timedCaseRemainingSeconds = isTimedMode ? TimedCaseDurationSeconds : 0f;
            selectedSuspectIndex = 0;
            selectedAccusationIndex = -1;
            InitializeInvestigationState(caseFile);
            EnsureSuspectIntroduction(selectedSuspectIndex);
            RefreshCrimeBoardUI();
            ShowScreen(GameScreen.CrimeBoard);
        }

        private void InitializeInvestigationState(CaseFile caseFile)
        {
            int suspectCount = caseFile != null && caseFile.suspects != null ? caseFile.suspects.Length : 0;
            suspectConversationLogs = new StringBuilder[suspectCount];
            introducedSuspects = new bool[suspectCount];
            askedQuestionFlags = new bool[QuestionCount];
            viewedCrimeScreen = false;
            viewedSuspectsScreen = false;
            completedInterrogationStep = false;

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
            askedQuestionFlags = null;
            viewedCrimeScreen = false;
            viewedSuspectsScreen = false;
            completedInterrogationStep = false;
            currentSystemMessage = string.Empty;
            selectedSuspectIndex = 0;
            selectedAccusationIndex = -1;
            requiredGuiltySuspectIndex = -1;
            requestedTimedMode = false;
            isTimedMode = false;
            timedCaseExpired = false;
            timedCaseRemainingSeconds = 0f;
            RefreshTimedCaseUI();
        }

        private static CaseFile CreateLoadingCase()
        {
            return new CaseFile
            {
                caseTitle = "Generating New Case...",
                boardSummary = "Ollama is building a fresh mystery. If this takes a little longer, the local model is still warming up.",
                victim = "Pending victim",
                victimDescription = "The victim profile is still being assembled from witness statements, scene reports, and whatever the local model can piece together.",
                victimPortraitId = string.Empty,
                location = "The setting is still being reconstructed from the first arriving reports.",
                crime = "Detective notes are still being assembled. Cause of death, last movements, and the circumstances around the body are still pending.",
                onSiteClues = new[]
                {
                    "Scanning the crime scene...",
                    "Collecting witness reports...",
                    "Comparing possible suspect links..."
                },
                interrogationQuestions = new[]
                {
                    "Question 1 pending...",
                    "Question 2 pending...",
                    "Question 3 pending...",
                    "Question 4 pending..."
                },
                suspects = new[]
                {
                    CreateLoadingSuspect("FemCharacter1"),
                    CreateLoadingSuspect("FemCharacter2"),
                    CreateLoadingSuspect("MascCharacter2")
                },
                guiltySuspect = string.Empty,
                guiltClues = new[] { "Pending", "Pending" },
                explanation = "Pending"
            };
        }

        private static Suspect CreateLoadingSuspect(string portraitId)
        {
            return new Suspect
            {
                name = "Generating...",
                portraitId = portraitId,
                role = "Profile incoming",
                description = "The detective is still waiting for the suspect's background and first-impression notes.",
                appearance = "Appearance pending.",
                connectionToCase = "Waiting for Ollama response.",
                lastSeenVictim = "Pending",
                motive = "Pending",
                alibi = "Pending",
                personality = "Pending",
                openingStatement = "Pending.",
                interrogationAnswers = new[] { "Pending.", "Pending.", "Pending.", "Pending." }
            };
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

        private bool TryParseGeneratedCase(string rawResponse, out CaseFile caseFile, out string error)
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

            if (!HasExactLength(parsedJson.on_site_clues, 3))
            {
                error = "The generated case did not contain exactly three on-site clues.";
                return false;
            }

            if (!HasExactLength(parsedJson.interrogation_questions, QuestionCount))
            {
                error = "The generated case did not contain exactly four interrogation questions.";
                return false;
            }

            if (!HasExactLength(parsedJson.guilt_clues, 2))
            {
                error = "The generated case did not contain exactly two guilt clues.";
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

                if (!HasExactLength(suspectJson.interrogation_answers, QuestionCount))
                {
                    error = $"Suspect {suspectName} did not contain exactly four interrogation answers.";
                    return false;
                }

                string[] interrogationAnswers = new string[QuestionCount];
                for (int answerIndex = 0; answerIndex < QuestionCount; answerIndex++)
                {
                    string answerText = CleanValue(suspectJson.interrogation_answers[answerIndex]);
                    if (string.IsNullOrWhiteSpace(answerText))
                    {
                        error = $"Suspect {suspectName} has an incomplete interrogation answer set.";
                        return false;
                    }

                    interrogationAnswers[answerIndex] = answerText;
                }

                suspects[i] = new Suspect
                {
                    name = suspectName,
                    portraitId = suspectPortraitIds[i],
                    role = CleanValue(suspectJson.role, "Unknown role"),
                    description = CleanValue(suspectJson.description, "No suspect description was provided."),
                    appearance = CleanValue(suspectJson.appearance, "No appearance details were provided."),
                    connectionToCase = CleanValue(suspectJson.connection_to_case, "Connection unknown."),
                    lastSeenVictim = CleanValue(suspectJson.last_seen_victim, "The last confirmed sighting of the victim is unknown."),
                    motive = CleanValue(suspectJson.motive, "No motive provided."),
                    alibi = CleanValue(suspectJson.alibi, "No alibi provided."),
                    personality = CleanValue(suspectJson.personality, "Reserved"),
                    openingStatement = CleanValue(suspectJson.opening_statement, "I do not know what else I can add right now, detective."),
                    interrogationAnswers = interrogationAnswers
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

            if (HasConflictingCulpritMentions(parsedJson.guilt_clues, parsedJson.explanation, suspects, guiltySuspectName))
            {
                error = "The final clue summary pointed to multiple suspects instead of one clear culprit.";
                return false;
            }

            caseFile = new CaseFile
            {
                caseTitle = CleanValue(parsedJson.case_title, "Untitled Case"),
                boardSummary = CleanValue(parsedJson.board_summary, "A fresh case has arrived. Investigate the evidence and question the suspects."),
                victim = CleanValue(parsedJson.victim, "Unknown victim"),
                victimDescription = CleanValue(parsedJson.victim_description, "No victim profile was provided."),
                victimPortraitId = "MascCharacter1",
                location = CleanValue(parsedJson.location, "Unknown location"),
                crime = CleanValue(parsedJson.crime, "No crime summary was provided."),
                onSiteClues = CleanArray(parsedJson.on_site_clues, "No on-site clue was provided."),
                suspects = suspects,
                interrogationQuestions = CleanArray(parsedJson.interrogation_questions, "No interrogation question was provided."),
                guiltySuspect = guiltySuspectName,
                guiltClues = CleanArray(parsedJson.guilt_clues, "No guilt clue was provided."),
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

        private static string[] CleanArray(string[] values, string fallback)
        {
            if (values == null || values.Length == 0)
            {
                return new[] { fallback };
            }

            string[] cleaned = new string[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                cleaned[i] = CleanValue(values[i], fallback);
            }

            return cleaned;
        }

        private static bool HasExactLength<T>(T[] values, int length)
        {
            return values != null && values.Length == length;
        }

        private static bool HasConflictingCulpritMentions(string[] guiltClues, string explanation, Suspect[] suspects, string guiltySuspectName)
        {
            if (suspects == null)
            {
                return false;
            }

            for (int i = 0; i < suspects.Length; i++)
            {
                string suspectName = suspects[i].name;
                if (string.IsNullOrWhiteSpace(suspectName) || string.Equals(suspectName, guiltySuspectName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (ContainsName(guiltClues, suspectName) || ContainsName(explanation, suspectName))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsName(string[] lines, string suspectName)
        {
            if (lines == null)
            {
                return false;
            }

            for (int i = 0; i < lines.Length; i++)
            {
                if (ContainsName(lines[i], suspectName))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsName(string text, string suspectName)
        {
            return !string.IsNullOrWhiteSpace(text)
                && !string.IsNullOrWhiteSpace(suspectName)
                && text.IndexOf(suspectName, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private int GetRemainingQuestionCount()
        {
            if (askedQuestionFlags == null)
            {
                return QuestionCount;
            }

            int remaining = 0;
            for (int i = 0; i < askedQuestionFlags.Length; i++)
            {
                if (!askedQuestionFlags[i])
                {
                    remaining++;
                }
            }

            return remaining;
        }

        private bool IsArrestUnlocked()
        {
            return timedCaseExpired || (viewedCrimeScreen && viewedSuspectsScreen && completedInterrogationStep);
        }

        private void TickTimedCaseClock()
        {
            if (!isTimedMode || timedCaseExpired || isGeneratingCase || !HasCase())
            {
                return;
            }

            timedCaseRemainingSeconds = Mathf.Max(0f, timedCaseRemainingSeconds - Time.deltaTime);
            RefreshTimedCaseUI();

            if (timedCaseRemainingSeconds <= 0f)
            {
                timedCaseExpired = true;
                currentSystemMessage = AppendMessage(currentSystemMessage, "Time is up. You must make your arrest based on the evidence you managed to review.");
                ForceTimedCaseArrest();
            }
        }

        private void ForceTimedCaseArrest()
        {
            if (!HasCase())
            {
                return;
            }

            if (!IsValidSuspectIndex(selectedAccusationIndex))
            {
                selectedAccusationIndex = -1;
            }

            uiManager.ShowAccusation(currentCaseFile, selectedAccusationIndex, timedCaseExpired);
            ShowScreen(GameScreen.Arrest);
        }

        private void RefreshTimedCaseUI()
        {
            if (uiManager == null)
            {
                return;
            }

            bool visible = isTimedMode && CurrentScreen != GameScreen.MainMenu && CurrentScreen != GameScreen.Result;
            if (!visible)
            {
                uiManager.ShowTimer(false, string.Empty, false);
                return;
            }

            string timerText = timedCaseExpired
                ? "Time's Up"
                : $"{FormatTimer(timedCaseRemainingSeconds)} Left";
            bool urgent = timedCaseExpired || timedCaseRemainingSeconds <= 30f;
            uiManager.ShowTimer(true, timerText, urgent);
        }

        private static string FormatTimer(float secondsRemaining)
        {
            int totalSeconds = Mathf.CeilToInt(Mathf.Max(0f, secondsRemaining));
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            return $"{minutes:00}:{seconds:00}";
        }

        private string BuildSystemMessage()
        {
            return currentSystemMessage;
        }

        private static string AppendMessage(string existingMessage, string nextMessage)
        {
            if (string.IsNullOrWhiteSpace(existingMessage))
            {
                return nextMessage ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(nextMessage))
            {
                return existingMessage;
            }

            return $"{existingMessage}\n\n{nextMessage}";
        }

        private static int FindSuspectIndex(CaseFile caseFile, string suspectName)
        {
            if (caseFile == null || caseFile.suspects == null || string.IsNullOrWhiteSpace(suspectName))
            {
                return -1;
            }

            for (int i = 0; i < caseFile.suspects.Length; i++)
            {
                if (string.Equals(caseFile.suspects[i].name, suspectName, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
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
            return currentCaseFile != null
                && currentCaseFile.suspects != null
                && currentCaseFile.suspects.Length == 3
                && currentCaseFile.interrogationQuestions != null
                && currentCaseFile.interrogationQuestions.Length == QuestionCount;
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
