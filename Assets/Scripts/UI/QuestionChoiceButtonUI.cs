using CaseFileLocalSuspect.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CaseFileLocalSuspect.UI
{
    public class QuestionChoiceButtonUI : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private int questionIndex;
        [SerializeField] private TMP_Text labelText;
        [SerializeField] private Button button;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Color availableColor = new Color(0.18f, 0.18f, 0.18f, 0.96f);
        [SerializeField] private Color usedColor = new Color(0.11f, 0.11f, 0.11f, 0.85f);

        public void SetQuestion(string questionText, bool isUsed, int displayIndex)
        {
            if (labelText != null)
            {
                labelText.text = isUsed
                    ? $"{displayIndex}. {questionText}\n[USED]"
                    : $"{displayIndex}. {questionText}";
            }

            if (button != null)
            {
                button.interactable = !isUsed;
            }

            if (backgroundImage != null)
            {
                backgroundImage.color = isUsed ? usedColor : availableColor;
            }
        }

        public void Press()
        {
            if (gameManager != null)
            {
                gameManager.AskQuestion(questionIndex);
            }
        }
    }
}
