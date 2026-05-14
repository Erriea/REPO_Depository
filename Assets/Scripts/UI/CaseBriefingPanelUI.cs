using CaseFileLocalSuspect.Game;
using TMPro;
using UnityEngine;

namespace CaseFileLocalSuspect.UI
{
    public class CaseBriefingPanelUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text caseTitleText;
        [SerializeField] private TMP_Text crimeText;
        [SerializeField] private TMP_Text victimText;
        [SerializeField] private TMP_Text locationText;
        [SerializeField] private SuspectCardUI[] suspectCards;

        public void ShowCase(CaseFile caseFile)
        {
            if (caseFile == null)
            {
                SetText(caseTitleText, "No Case Loaded");
                SetText(crimeText, "Crime: No case has been prepared yet.");
                SetText(victimText, "Victim: Unknown");
                SetText(locationText, "Location: Unknown");
                return;
            }

            SetText(caseTitleText, caseFile.caseTitle);
            SetText(crimeText, $"Crime: {caseFile.crime}");
            SetText(victimText, $"Victim: {caseFile.victim}");
            SetText(locationText, $"Location: {caseFile.location}");

            for (int i = 0; i < suspectCards.Length; i++)
            {
                Suspect suspect = caseFile.suspects != null && i < caseFile.suspects.Length
                    ? caseFile.suspects[i]
                    : null;
                suspectCards[i].SetSuspect(suspect);
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
