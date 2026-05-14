using CaseFileLocalSuspect.Game;
using TMPro;
using UnityEngine;

namespace CaseFileLocalSuspect.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject caseBriefingPanel;
        [SerializeField] private GameObject interrogationPanel;
        [SerializeField] private GameObject accusationPanel;
        [SerializeField] private GameObject resultPanel;

        [Header("Shared Text")]
        [SerializeField] private TMP_Text screenTitleText;
        [SerializeField] private TMP_Text placeholderMessageText;

        [Header("Panel Controllers")]
        [SerializeField] private CaseBriefingPanelUI caseBriefingPanelUI;

        public void ShowScreen(GameScreen screen)
        {
            SetPanelActive(mainMenuPanel, screen == GameScreen.MainMenu);
            SetPanelActive(caseBriefingPanel, screen == GameScreen.CaseBriefing);
            SetPanelActive(interrogationPanel, screen == GameScreen.Interrogation);
            SetPanelActive(accusationPanel, screen == GameScreen.Accusation);
            SetPanelActive(resultPanel, screen == GameScreen.Result);

            UpdatePlaceholderText(screen);
        }

        public void ShowCaseBriefing(CaseFile caseFile)
        {
            if (caseBriefingPanelUI != null)
            {
                caseBriefingPanelUI.ShowCase(caseFile);
            }
        }

        private void UpdatePlaceholderText(GameScreen screen)
        {
            if (screenTitleText != null)
            {
                screenTitleText.text = GetScreenTitle(screen);
            }

            if (placeholderMessageText != null)
            {
                placeholderMessageText.text = GetPlaceholderMessage(screen);
            }
        }

        private static void SetPanelActive(GameObject panel, bool isActive)
        {
            if (panel != null)
            {
                panel.SetActive(isActive);
            }
        }

        private static string GetScreenTitle(GameScreen screen)
        {
            switch (screen)
            {
                case GameScreen.MainMenu:
                    return "CaseFile: Local Suspect";
                case GameScreen.CaseBriefing:
                    return "Case Briefing";
                case GameScreen.Interrogation:
                    return "Interrogation";
                case GameScreen.Accusation:
                    return "Accusation";
                case GameScreen.Result:
                    return "Result";
                default:
                    return "CaseFile: Local Suspect";
            }
        }

        private static string GetPlaceholderMessage(GameScreen screen)
        {
            switch (screen)
            {
                case GameScreen.MainMenu:
                    return "Start a new case to begin the investigation.";
                case GameScreen.CaseBriefing:
                    return "Milestone 3 will populate this screen with the crime summary and suspect cards.";
                case GameScreen.Interrogation:
                    return "Milestone 4 will add suspect questions and conversation history here.";
                case GameScreen.Accusation:
                    return "Milestone 5 will let the player accuse a suspect here.";
                case GameScreen.Result:
                    return "Milestone 5 will show the accusation result and explanation here.";
                default:
                    return string.Empty;
            }
        }
    }
}
