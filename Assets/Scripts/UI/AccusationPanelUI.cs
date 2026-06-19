using CaseFileLocalSuspect.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CaseFileLocalSuspect.UI
{
    public class AccusationPanelUI : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private TMP_Text promptText;
        [SerializeField] private SuspectChoiceButtonUI[] suspectButtons;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Image panelBackground;
        [SerializeField] private Color normalBackgroundColor = new Color(0.08f, 0.07f, 0.07f, 0.82f);
        [SerializeField] private Color timedOutBackgroundColor = new Color(0.20f, 0.20f, 0.20f, 0.90f);

        public void ShowChoices(CaseFile caseFile, PortraitLibrary portraitLibrary, int selectedIndex, bool forcedByTimeout)
        {
            if (caseFile == null || caseFile.suspects == null)
            {
                SetText(promptText, "Choose the suspect you believe is guilty.");
                return;
            }

            if (panelBackground != null)
            {
                panelBackground.color = forcedByTimeout ? timedOutBackgroundColor : normalBackgroundColor;
            }

            string selectedLabel = selectedIndex >= 0 && selectedIndex < caseFile.suspects.Length
                ? caseFile.suspects[selectedIndex].name
                : "No suspect selected";
            string timeoutMessage = forcedByTimeout
                ? "Time is up. You must choose who to arrest with what you learned.\n"
                : string.Empty;
            SetText(promptText, $"{timeoutMessage}Make the arrest for \"{caseFile.caseTitle}\".\nSelected suspect: {selectedLabel}");
            if (confirmButton != null)
            {
                confirmButton.interactable = selectedIndex >= 0 && selectedIndex < caseFile.suspects.Length;
            }

            for (int i = 0; i < suspectButtons.Length; i++)
            {
                Suspect suspect = i < caseFile.suspects.Length ? caseFile.suspects[i] : null;
                Sprite portrait = portraitLibrary != null && suspect != null ? portraitLibrary.GetPortrait(suspect.portraitId) : null;
                suspectButtons[i].SetSuspect(suspect, portrait, i == selectedIndex);
            }
        }

        public void ConfirmAccusationPressed()
        {
            if (gameManager != null)
            {
                gameManager.ConfirmAccusation();
            }
        }

        public void BackPressed()
        {
            if (gameManager != null)
            {
                gameManager.ReturnToCrimeBoard();
            }
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
