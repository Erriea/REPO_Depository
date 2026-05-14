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
                SetText(suspectMotiveText, "Motive: Unknown");
                SetText(suspectAlibiText, "Alibi: Unknown");
                return;
            }

            SetPortrait(portraitSprite);
            SetText(suspectNameText, suspect.name);
            SetText(suspectRoleText, $"Role: {suspect.role}");
            SetText(suspectMotiveText, $"Motive: {Shorten(suspect.motive, 90)}");
            SetText(suspectAlibiText, $"Alibi: {Shorten(suspect.alibi, 95)}");
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
