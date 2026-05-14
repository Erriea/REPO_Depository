using CaseFileLocalSuspect.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CaseFileLocalSuspect.UI
{
    public class CaseBriefingPanelUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text caseTitleText;
        [SerializeField] private TMP_Text crimeText;
        [SerializeField] private TMP_Text victimText;
        [SerializeField] private TMP_Text locationText;
        [SerializeField] private Image victimPortraitImage;
        [SerializeField] private SuspectCardUI[] suspectCards;

        public void ShowCase(CaseFile caseFile, PortraitLibrary portraitLibrary)
        {
            if (caseFile == null)
            {
                SetText(caseTitleText, "No Case Loaded");
                SetText(crimeText, "Crime: No case has been prepared yet.");
                SetText(victimText, "Victim: Unknown");
                SetText(locationText, "Location: Unknown");
                SetPortrait(victimPortraitImage, null);
                return;
            }

            SetText(caseTitleText, caseFile.caseTitle);
            SetText(crimeText, $"Crime: {caseFile.crime}");
            SetText(victimText, $"Victim: {caseFile.victim}");
            SetText(locationText, $"Location: {caseFile.location}");
            SetPortrait(victimPortraitImage, portraitLibrary != null ? portraitLibrary.GetPortrait(caseFile.victimPortraitId) : null);

            for (int i = 0; i < suspectCards.Length; i++)
            {
                Suspect suspect = caseFile.suspects != null && i < caseFile.suspects.Length
                    ? caseFile.suspects[i]
                    : null;
                Sprite portrait = portraitLibrary != null && suspect != null
                    ? portraitLibrary.GetPortrait(suspect.portraitId)
                    : null;
                suspectCards[i].SetSuspect(suspect, portrait);
            }
        }

        private static void SetText(TMP_Text textField, string value)
        {
            if (textField != null)
            {
                textField.text = value;
            }
        }

        private static void SetPortrait(Image portraitImage, Sprite portraitSprite)
        {
            if (portraitImage != null)
            {
                portraitImage.sprite = portraitSprite;
                portraitImage.enabled = portraitSprite != null;
            }
        }
    }
}
