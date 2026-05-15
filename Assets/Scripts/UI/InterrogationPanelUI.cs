using CaseFileLocalSuspect.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CaseFileLocalSuspect.UI
{
    public class InterrogationPanelUI : MonoBehaviour
    {
        private const float MinimumConversationHeight = 360f;
        private const float ConversationBottomPadding = 24f;

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

        private void Awake()
        {
            ConfigureConversationLayout();
        }

        public void ShowState(CaseFile caseFile, PortraitLibrary portraitLibrary, int selectedSuspectIndex, int questionsRemaining, string conversationHistory, string hintMessage, bool canAskQuestions)
        {
            if (caseFile == null || caseFile.suspects == null || caseFile.suspects.Length == 0)
            {
                return;
            }

            Suspect selectedSuspect = caseFile.suspects[selectedSuspectIndex];
            SetText(selectedSuspectNameText, selectedSuspect.name);
            SetText(selectedSuspectRoleText, $"Role: {selectedSuspect.role}\nConnection: {selectedSuspect.connectionToCase}");
            SetText(questionsRemainingText, $"Questions Remaining: {questionsRemaining}");
            SetText(conversationHistoryText, string.IsNullOrWhiteSpace(conversationHistory)
                ? "Select a suspect and start asking questions."
                : conversationHistory);
            SetText(hintText, hintMessage);

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

            RefreshConversationView();
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

        private void RefreshConversationView()
        {
            if (conversationHistoryText == null)
            {
                return;
            }

            ConfigureConversationLayout();
            conversationHistoryText.ForceMeshUpdate();

            if (conversationContentRect != null)
            {
                float preferredHeight = Mathf.Max(conversationHistoryText.preferredHeight + ConversationBottomPadding, MinimumConversationHeight);
                conversationContentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredHeight);
            }

            Canvas.ForceUpdateCanvases();

            if (conversationScrollRect != null)
            {
                conversationScrollRect.verticalNormalizedPosition = 0f;
            }
        }

        private void ConfigureConversationLayout()
        {
            if (conversationHistoryText == null)
            {
                return;
            }

            RectTransform textRect = conversationHistoryText.rectTransform;
            textRect.anchorMin = new Vector2(0f, 1f);
            textRect.anchorMax = new Vector2(1f, 1f);
            textRect.pivot = new Vector2(0.5f, 1f);
            textRect.anchoredPosition = Vector2.zero;
            textRect.offsetMin = new Vector2(0f, 0f);
            textRect.offsetMax = new Vector2(0f, 0f);

            conversationHistoryText.enableAutoSizing = false;
            conversationHistoryText.enableWordWrapping = true;
            conversationHistoryText.overflowMode = TextOverflowModes.Overflow;

            if (conversationContentRect != null)
            {
                conversationContentRect.anchorMin = new Vector2(0f, 1f);
                conversationContentRect.anchorMax = new Vector2(1f, 1f);
                conversationContentRect.pivot = new Vector2(0.5f, 1f);
                conversationContentRect.anchoredPosition = Vector2.zero;
                conversationContentRect.sizeDelta = new Vector2(0f, Mathf.Max(conversationContentRect.sizeDelta.y, MinimumConversationHeight));
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
