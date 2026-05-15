using CaseFileLocalSuspect.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CaseFileLocalSuspect.UI
{
    public class SuspectCardUI : MonoBehaviour
    {
        [SerializeField] private Image portraitImage;
        [SerializeField] private TMP_Text suspectNameText;
        [SerializeField] private TMP_Text suspectRoleText;
        [SerializeField] private TMP_Text suspectMotiveText;
        [SerializeField] private TMP_Text suspectAlibiText;

        public void SetSuspect(Suspect suspect, Sprite portraitSprite)
        {
            if (suspect == null)
            {
                SetPortrait(null);
                SetText(suspectNameText, "Unknown Suspect");
                SetText(suspectRoleText, "Role: Unknown");
                SetText(suspectMotiveText, "Connection: Unknown");
                SetText(suspectAlibiText, "Story: Unknown");
                return;
            }

            SetPortrait(portraitSprite);
            SetText(suspectNameText, suspect.name);
            SetText(suspectRoleText, $"Role: {suspect.role}");
            SetText(suspectMotiveText, $"Connection: {Shorten(suspect.connectionToCase, 100)}");
            SetText(suspectAlibiText, $"Story: {Shorten(suspect.openingStatement, 125)}");
        }

        private static void SetText(TMP_Text textField, string value)
        {
            if (textField != null)
            {
                textField.text = value;
            }
        }

        private void SetPortrait(Sprite portraitSprite)
        {
            if (portraitImage != null)
            {
                portraitImage.sprite = portraitSprite;
                portraitImage.enabled = portraitSprite != null;
            }
        }

        private static string Shorten(string text, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(text) || text.Length <= maxLength)
            {
                return text;
            }

            return text.Substring(0, maxLength - 3).TrimEnd() + "...";
        }
    }
}
