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
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text appearanceText;
        [SerializeField] private TMP_Text caseText;
        [SerializeField] private TMP_Text motiveText;

        public void SetSuspect(Suspect suspect, Sprite portraitSprite)
        {
            if (suspect == null)
            {
                SetPortrait(null);
                SetText(suspectNameText, "Unknown Suspect");
                SetText(descriptionText, "Description: Unknown");
                SetText(appearanceText, "Appearance: Unknown");
                SetText(caseText, "Relationship: Unknown");
                SetText(motiveText, "Alibi: Unknown");
                return;
            }

            SetPortrait(portraitSprite);
            SetText(suspectNameText, suspect.name);
            SetText(
                descriptionText,
                $"Description: {suspect.description}");
            SetText(
                appearanceText,
                $"Appearance: {suspect.appearance}\n\n" +
                $"Personality: {suspect.personality}");
            SetText(
                caseText,
                $"Relationship: {suspect.connectionToCase}\n\n" +
                $"Last saw victim: {suspect.lastSeenVictim}");
            SetText(
                motiveText,
                $"Alibi: {suspect.alibi}\n\n" +
                $"Motive: {suspect.motive}");
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
    }
}
