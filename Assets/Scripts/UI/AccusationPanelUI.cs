using CaseFileLocalSuspect.Game;
using TMPro;
using UnityEngine;

namespace CaseFileLocalSuspect.UI
{
    public class AccusationPanelUI : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private TMP_Text promptText;
        [SerializeField] private SuspectChoiceButtonUI[] suspectButtons;

        public void ShowChoices(CaseFile caseFile, PortraitLibrary portraitLibrary, int selectedIndex)
        {
            SetText(promptText, "Choose the suspect you believe stole the ledger.");

            if (caseFile == null || caseFile.suspects == null)
            {
                return;
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
                gameManager.ReturnToInterrogation();
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
