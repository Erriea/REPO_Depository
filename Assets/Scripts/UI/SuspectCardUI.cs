using CaseFileLocalSuspect.Game;
using TMPro;
using UnityEngine;

namespace CaseFileLocalSuspect.UI
{
    public class SuspectCardUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text suspectNameText;
        [SerializeField] private TMP_Text suspectRoleText;
        [SerializeField] private TMP_Text suspectMotiveText;
        [SerializeField] private TMP_Text suspectAlibiText;

        public void SetSuspect(Suspect suspect)
        {
            if (suspect == null)
            {
                SetText(suspectNameText, "Unknown Suspect");
                SetText(suspectRoleText, "Role: Unknown");
                SetText(suspectMotiveText, "Motive: Unknown");
                SetText(suspectAlibiText, "Alibi: Unknown");
                return;
            }

            SetText(suspectNameText, suspect.name);
            SetText(suspectRoleText, $"Role: {suspect.role}");
            SetText(suspectMotiveText, $"Motive: {suspect.motive}");
            SetText(suspectAlibiText, $"Alibi: {suspect.alibi}");
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
