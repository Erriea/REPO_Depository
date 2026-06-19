using CaseFileLocalSuspect.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CaseFileLocalSuspect.UI
{
    public class ResultPanelUI : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private TMP_Text resultHeaderText;
        [SerializeField] private TMP_Text accusedSuspectText;
        [SerializeField] private TMP_Text guiltySuspectText;
        [SerializeField] private TMP_Text keyClueText;
        [SerializeField] private TMP_Text explanationText;
        [SerializeField] private Image guiltySuspectPortraitImage;

        public void ShowResult(CaseFile caseFile, PortraitLibrary portraitLibrary, int accusedIndex, bool isCorrect)
        {
            if (caseFile == null || caseFile.suspects == null || caseFile.suspects.Length == 0)
            {
                return;
            }

            Suspect accusedSuspect = caseFile.suspects[accusedIndex];
            Suspect guiltySuspect = FindGuiltySuspect(caseFile);

            SetText(resultHeaderText, isCorrect ? "Correct Accusation" : "Incorrect Accusation");
            SetText(accusedSuspectText, $"You accused: {accusedSuspect.name}");
            SetText(guiltySuspectText, isCorrect
                ? $"Verdict: {caseFile.guiltySuspect} was the correct arrest."
                : $"True criminal: {caseFile.guiltySuspect}");
            SetText(keyClueText, BuildClueSummary(caseFile.guiltClues));
            SetText(explanationText, caseFile.explanation);

            if (guiltySuspectPortraitImage != null)
            {
                guiltySuspectPortraitImage.sprite = portraitLibrary != null && guiltySuspect != null
                    ? portraitLibrary.GetPortrait(guiltySuspect.portraitId)
                    : null;
                guiltySuspectPortraitImage.enabled = guiltySuspectPortraitImage.sprite != null;
            }
        }

        public void StartNewCasePressed()
        {
            if (gameManager != null)
            {
                gameManager.ReturnToMainMenu();
            }
        }

        private static string BuildClueSummary(string[] guiltClues)
        {
            if (guiltClues == null || guiltClues.Length == 0)
            {
                return "Clues that pointed to the culprit: None recorded.";
            }

            return "Clues that pointed to the culprit:\n- " + string.Join("\n- ", guiltClues);
        }

        private static Suspect FindGuiltySuspect(CaseFile caseFile)
        {
            for (int i = 0; i < caseFile.suspects.Length; i++)
            {
                if (caseFile.suspects[i].name == caseFile.guiltySuspect)
                {
                    return caseFile.suspects[i];
                }
            }

            return null;
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
