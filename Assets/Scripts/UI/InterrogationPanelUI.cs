using CaseFileLocalSuspect.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CaseFileLocalSuspect.UI
{
    public class InterrogationPanelUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text selectedSuspectNameText;
        [SerializeField] private TMP_Text selectedSuspectRoleText;
        [SerializeField] private TMP_Text questionsRemainingText;
        [SerializeField] private TMP_Text conversationHistoryText;
        [SerializeField] private TMP_Text hintText;
        [SerializeField] private TMP_Text interrogationStatusText;
        [SerializeField] private TMP_Text answerGuideText;
        [SerializeField] private Image selectedSuspectPortraitImage;
        [SerializeField] private SuspectChoiceButtonUI[] suspectButtons;
        [SerializeField] private QuestionChoiceButtonUI[] questionButtons;

        public void ShowState(CaseFile caseFile, PortraitLibrary portraitLibrary, int selectedSuspectIndex, int questionsRemaining, string conversationHistory, bool completedInterrogationStep, bool[] askedQuestionFlags)
        {
            if (caseFile == null || caseFile.suspects == null || caseFile.suspects.Length == 0)
            {
                return;
            }

            Suspect selectedSuspect = caseFile.suspects[selectedSuspectIndex];
            SetText(selectedSuspectNameText, selectedSuspect.name);
            SetText(selectedSuspectRoleText, $"Who They Are: {selectedSuspect.role}\nConnection: {selectedSuspect.connectionToCase}");
            SetText(questionsRemainingText, $"Questions Remaining: {questionsRemaining}");
            SetText(interrogationStatusText, completedInterrogationStep
                ? "At least one question has been used. You can return to the crime board and unlock the arrest option once the other evidence screens are reviewed."
                : "Use at least one question here before the arrest option will unlock on the crime board.");
            SetText(answerGuideText, "The full exchange appears below after you click a question.");
            SetText(conversationHistoryText, string.IsNullOrWhiteSpace(conversationHistory)
                ? $"{selectedSuspect.name}:\n{selectedSuspect.openingStatement}\n\nSelect one of the remaining questions on the right to record this suspect's answer here."
                : conversationHistory);

            if (selectedSuspectPortraitImage != null)
            {
                selectedSuspectPortraitImage.sprite = portraitLibrary != null ? portraitLibrary.GetPortrait(selectedSuspect.portraitId) : null;
                selectedSuspectPortraitImage.enabled = selectedSuspectPortraitImage.sprite != null;
            }

            for (int i = 0; i < suspectButtons.Length; i++)
            {
                Suspect suspect = i < caseFile.suspects.Length ? caseFile.suspects[i] : null;
                Sprite portrait = portraitLibrary != null && suspect != null ? portraitLibrary.GetPortrait(suspect.portraitId) : null;
                suspectButtons[i].SetSuspect(suspect, portrait, i == selectedSuspectIndex);
            }

            for (int i = 0; i < questionButtons.Length; i++)
            {
                bool isUsed = askedQuestionFlags != null && i < askedQuestionFlags.Length && askedQuestionFlags[i];
                string question = caseFile.interrogationQuestions != null && i < caseFile.interrogationQuestions.Length
                    ? caseFile.interrogationQuestions[i]
                    : "Question unavailable";
                questionButtons[i].SetQuestion(question, isUsed, i + 1);
            }
        }

        public void ShowHint(string message)
        {
            SetText(hintText, message);
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
