using System.Text;
using CaseFileLocalSuspect.UI;
using UnityEngine;

namespace CaseFileLocalSuspect.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private UIManager uiManager;

        private CaseFile currentCaseFile;
        private readonly StringBuilder conversationHistory = new StringBuilder();
        private int selectedSuspectIndex;
        private int selectedAccusationIndex;
        private int questionsRemaining;

        public GameScreen CurrentScreen { get; private set; } = GameScreen.MainMenu;
        public const int StartingQuestionCount = 6;

        private void Start()
        {
            ShowScreen(GameScreen.MainMenu);
        }

        public void StartNewCase()
        {
            currentCaseFile = FallbackCaseProvider.CreateCase();
            selectedSuspectIndex = 0;
            selectedAccusationIndex = 0;
            questionsRemaining = StartingQuestionCount;
            conversationHistory.Clear();
            AppendSystemLine("Case generated. Review the briefing, then begin the interrogation.");
            uiManager.ShowCaseBriefing(currentCaseFile);
            ShowScreen(GameScreen.CaseBriefing);
        }

        public void BeginInterrogation()
        {
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
            RefreshInterrogationUI();
        }

        public void SubmitQuestion(string question)
        {
            if (!HasCase() || questionsRemaining <= 0)
            {
                return;
            }

            string trimmedQuestion = string.IsNullOrWhiteSpace(question) ? string.Empty : question.Trim();
            if (trimmedQuestion.Length == 0)
            {
                uiManager.ShowInterrogationMessage("Type a question before submitting.");
                return;
            }

            Suspect suspect = currentCaseFile.suspects[selectedSuspectIndex];
            int questionsAskedSoFar = StartingQuestionCount - questionsRemaining;

            AppendConversationLine("Detective", trimmedQuestion);
            string response = FallbackDialogueGenerator.GenerateResponse(currentCaseFile, suspect, trimmedQuestion, questionsAskedSoFar);
            AppendConversationLine(suspect.name, response);

            questionsRemaining--;
            uiManager.ClearQuestionInput();

            if (questionsRemaining == 0)
            {
                AppendSystemLine("You have used all available questions. Make your accusation.");
            }

            RefreshInterrogationUI();
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
                System.StringComparison.OrdinalIgnoreCase);

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

        private void RefreshInterrogationUI()
        {
            if (!HasCase())
            {
                return;
            }

            uiManager.ShowInterrogation(
                currentCaseFile,
                selectedSuspectIndex,
                questionsRemaining,
                conversationHistory.ToString(),
                questionsRemaining > 0);
        }

        private void AppendConversationLine(string speaker, string text)
        {
            if (conversationHistory.Length > 0)
            {
                conversationHistory.AppendLine();
                conversationHistory.AppendLine();
            }

            conversationHistory.Append(speaker);
            conversationHistory.Append(": ");
            conversationHistory.Append(text);
        }

        private void AppendSystemLine(string text)
        {
            if (conversationHistory.Length > 0)
            {
                conversationHistory.AppendLine();
                conversationHistory.AppendLine();
            }

            conversationHistory.Append("System: ");
            conversationHistory.Append(text);
        }

        private bool HasCase()
        {
            return currentCaseFile != null && currentCaseFile.suspects != null && currentCaseFile.suspects.Length == 3;
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
