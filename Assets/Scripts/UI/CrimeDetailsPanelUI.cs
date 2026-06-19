using System.Text;
using CaseFileLocalSuspect.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CaseFileLocalSuspect.UI
{
    public class CrimeDetailsPanelUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text caseTitleText;
        [SerializeField] private TMP_Text locationText;
        [SerializeField] private TMP_Text victimText;
        [SerializeField] private TMP_Text crimeText;
        [SerializeField] private TMP_Text cluesText;
        [SerializeField] private TMP_Text suspectsText;
        [SerializeField] private Image victimPortraitImage;

        public void ShowCase(CaseFile caseFile, PortraitLibrary portraitLibrary)
        {
            if (caseFile == null)
            {
                SetText(caseTitleText, "No Case Loaded");
                SetText(locationText, "Location: Unknown");
                SetText(victimText, "Victim: Unknown");
                SetText(crimeText, "What Happened: No case has been prepared yet.");
                SetText(cluesText, "On-site Clues:\n- None");
                SetText(suspectsText, "Suspects:\n- None");
                SetPortrait(victimPortraitImage, null);
                return;
            }

            SetText(caseTitleText, caseFile.caseTitle);
            SetText(locationText, BuildNarrativeField("Location", caseFile.location));
            SetText(victimText, BuildNarrativeField("Victim", $"{caseFile.victim}\n{caseFile.victimDescription}"));
            SetText(crimeText, BuildNarrativeField("What Happened", caseFile.crime));
            SetText(cluesText, BuildBulletList("On-site Clues", caseFile.onSiteClues));
            SetText(suspectsText, BuildSuspectList(caseFile.suspects));
            SetPortrait(victimPortraitImage, portraitLibrary != null ? portraitLibrary.GetPortrait(caseFile.victimPortraitId) : null);
        }

        private static string BuildNarrativeField(string label, string body)
        {
            return $"{label}:\n{body}";
        }

        private static string BuildBulletList(string heading, string[] items)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"{heading}:");

            if (items == null || items.Length == 0)
            {
                builder.Append("- None");
                return builder.ToString();
            }

            for (int i = 0; i < items.Length; i++)
            {
                builder.Append("- ");
                builder.AppendLine(items[i]);
            }

            return builder.ToString().TrimEnd();
        }

        private static string BuildSuspectList(Suspect[] suspects)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Suspects:");

            if (suspects == null || suspects.Length == 0)
            {
                builder.Append("- None");
                return builder.ToString();
            }

            for (int i = 0; i < suspects.Length; i++)
            {
                builder.Append("- ");
                builder.Append(suspects[i].name);
                builder.Append(" - ");
                builder.AppendLine(suspects[i].role);
            }

            return builder.ToString().TrimEnd();
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
