using CaseFileLocalSuspect.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CaseFileLocalSuspect.UI
{
    public class InterrogationPanelUI : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private TMP_Text selectedSuspectNameText;
        [SerializeField] private TMP_Text selectedSuspectRoleText;
        [SerializeField] private TMP_Text questionsRemainingText;
        [SerializeField] private TMP_Text conversationHistoryText;
        [SerializeField] private TMP_Text hintText;
        [SerializeField] private Image selectedSuspectPortraitImage;
        [SerializeField] private ScrollRect conversationScrollRect;
        [SerializeField] private RectTransform conversationContentRect;
        [SerializeField] private TMP_InputField questionInputField;
        [SerializeField] private Button submitQuestionButton;
        [SerializeField] private Button accuseButton;
        [SerializeField] private SuspectChoiceButtonUI[] suspectButtons;

        public void ShowState(CaseFile caseFile, PortraitLibrary portraitLibrary, int selectedSuspectIndex, int questionsRemaining, string conversationHistory, bool canAskQuestions)
        {
            if (caseFile == null || caseFile.suspects == null || caseFile.suspects.Length == 0)
            {
                return;
            }

            Suspect selectedSuspect = caseFile.suspects[selectedSuspectIndex];
            SetText(selectedSuspectNameText, selectedSuspect.name);
            SetText(selectedSuspectRoleText, selectedSuspect.role);
            SetText(questionsRemainingText, $"Questions Remaining: {questionsRemaining}");
            SetText(conversationHistoryText, string.IsNullOrWhiteSpace(conversationHistory)
                ? "Select a suspect and start asking questions."
                : conversationHistory);
            SetText(hintText, canAskQuestions
                ? "Ask about alibis, motives, the study, the victim, or the missing ledger."
                : "No questions remain. Make your accusation.");

            if (selectedSuspectPortraitImage != null)
            {
                selectedSuspectPortraitImage.sprite = portraitLibrary != null ? portraitLibrary.GetPortrait(selectedSuspect.portraitId) : null;
                selectedSuspectPortraitImage.enabled = selectedSuspectPortraitImage.sprite != null;
            }

            if (submitQuestionButton != null)
            {
                submitQuestionButton.interactable = canAskQuestions;
            }

            if (accuseButton != null)
            {
                accuseButton.interactable = true;
            }

            ScrollConversationToBottom();

            for (int i = 0; i < suspectButtons.Length; i++)
            {
                Suspect suspect = i < caseFile.suspects.Length ? caseFile.suspects[i] : null;
                Sprite portrait = portraitLibrary != null && suspect != null ? portraitLibrary.GetPortrait(suspect.portraitId) : null;
                suspectButtons[i].SetSuspect(suspect, portrait, i == selectedSuspectIndex);
            }
        }

        public void ClearQuestionInput()
        {
            if (questionInputField != null)
            {
                questionInputField.text = string.Empty;
                questionInputField.ActivateInputField();
            }
        }

        public void ShowHint(string message)
        {
            SetText(hintText, message);
        }

        private void ScrollConversationToBottom()
        {
            if (conversationHistoryText != null)
            {
                conversationHistoryText.ForceMeshUpdate();

                RectTransform textRect = conversationHistoryText.rectTransform;
                float preferredHeight = Mathf.Max(conversationHistoryText.preferredHeight + 20f, 200f);
                textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredHeight);

                if (conversationContentRect != null)
                {
                    conversationContentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredHeight);
                }
            }

            if (conversationContentRect != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(conversationContentRect);
            }

            Canvas.ForceUpdateCanvases();

            if (conversationScrollRect != null)
            {
                conversationScrollRect.verticalNormalizedPosition = 0f;
            }
        }

        public void SubmitQuestionPressed()
        {
            if (gameManager != null && questionInputField != null)
            {
                gameManager.SubmitQuestion(questionInputField.text);
            }
        }

        public void OpenAccusationPressed()
        {
            if (gameManager != null)
            {
                gameManager.OpenAccusation();
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
