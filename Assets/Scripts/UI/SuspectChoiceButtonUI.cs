using CaseFileLocalSuspect.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CaseFileLocalSuspect.UI
{
    public class SuspectChoiceButtonUI : MonoBehaviour
    {
        public enum ChoiceMode
        {
            Interrogation,
            Accusation
        }

        [SerializeField] private GameManager gameManager;
        [SerializeField] private ChoiceMode choiceMode;
        [SerializeField] private int suspectIndex;
        [SerializeField] private Image portraitImage;
        [SerializeField] private Image selectionFrame;
        [SerializeField] private TMP_Text labelText;
        [SerializeField] private Button button;
        [SerializeField] private Color selectedColor = new Color(0.82f, 0.64f, 0.28f, 1f);
        [SerializeField] private Color normalColor = new Color(0.28f, 0.28f, 0.28f, 1f);

        public void SetSuspect(Suspect suspect, Sprite portraitSprite, bool isSelected)
        {
            if (portraitImage != null)
            {
                portraitImage.sprite = portraitSprite;
                portraitImage.enabled = portraitSprite != null;
            }

            if (labelText != null)
            {
                labelText.text = suspect != null ? suspect.name : "Unknown";
            }

            if (selectionFrame != null)
            {
                selectionFrame.color = isSelected ? selectedColor : normalColor;
            }

            if (button != null)
            {
                button.interactable = suspect != null;
            }
        }

        public void Press()
        {
            if (gameManager == null)
            {
                return;
            }

            if (choiceMode == ChoiceMode.Interrogation)
            {
                gameManager.SelectSuspect(suspectIndex);
            }
            else
            {
                gameManager.SelectAccusation(suspectIndex);
            }
        }
    }
}
