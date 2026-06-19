using CaseFileLocalSuspect.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CaseFileLocalSuspect.UI
{
    public class CrimeBoardPanelUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text caseTitleText;
        [SerializeField] private TMP_Text summaryText;
        [SerializeField] private TMP_Text progressText;
        [SerializeField] private TMP_Text arrestStatusText;
        [SerializeField] private Button arrestButton;
        [SerializeField] private GameObject loadingOverlay;
        [SerializeField] private TMP_Text loadingOverlayText;

        public void ShowState(CaseFile caseFile, bool viewedCrime, bool viewedSuspects, bool completedInterrogation, bool arrestUnlocked, int questionsRemaining, string systemMessage, bool isLoadingCase)
        {
            SetText(caseTitleText, caseFile != null ? caseFile.caseTitle : "No Case Loaded");
            SetText(summaryText, caseFile != null ? caseFile.boardSummary : "Start a new case to begin your investigation.");
            SetText(
                progressText,
                (string.IsNullOrWhiteSpace(systemMessage) ? string.Empty : $"{systemMessage}\n\n") +
                $"Crime file: {FormatStatus(viewedCrime)}\n" +
                $"Suspects reviewed: {FormatStatus(viewedSuspects)}\n" +
                $"Interrogation used: {FormatStatus(completedInterrogation)}\n" +
                $"Questions remaining: {questionsRemaining}");
            SetText(
                arrestStatusText,
                arrestUnlocked
                    ? "Arrest is unlocked. You have reviewed enough evidence to make your choice."
                    : "Arrest remains locked until you inspect the crime, review the suspects, and use at least one interrogation question.");

            if (arrestButton != null)
            {
                arrestButton.interactable = arrestUnlocked;
            }

            if (loadingOverlay != null)
            {
                loadingOverlay.SetActive(isLoadingCase);
            }

            if (loadingOverlayText != null)
            {
                loadingOverlayText.text = isLoadingCase
                    ? "Case Files Loading...\nConsulting Ollama, reviewing witness notes, and pinning fresh evidence to the board."
                    : string.Empty;
            }
        }

        private static string FormatStatus(bool complete)
        {
            return complete ? "Done" : "Pending";
        }

        private static void SetText(TMP_Text textField, string value)
        {
            if (textField != null)
            {
                textField.text = value;
            }
        }
    }
}
