using CaseFileLocalSuspect.Game;
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

        [Header("Panel Controllers")]
        [SerializeField] private PortraitLibrary portraitLibrary;
        [SerializeField] private CaseBriefingPanelUI caseBriefingPanelUI;
        [SerializeField] private InterrogationPanelUI interrogationPanelUI;
        [SerializeField] private AccusationPanelUI accusationPanelUI;
        [SerializeField] private ResultPanelUI resultPanelUI;

        public void ShowScreen(GameScreen screen)
        {
            SetPanelActive(mainMenuPanel, screen == GameScreen.MainMenu);
            SetPanelActive(caseBriefingPanel, screen == GameScreen.CaseBriefing);
            SetPanelActive(interrogationPanel, screen == GameScreen.Interrogation);
            SetPanelActive(accusationPanel, screen == GameScreen.Accusation);
            SetPanelActive(resultPanel, screen == GameScreen.Result);
        }

        public void ShowCaseBriefing(CaseFile caseFile)
        {
            if (caseBriefingPanelUI != null)
            {
                caseBriefingPanelUI.ShowCase(caseFile, portraitLibrary);
            }
        }

        private static void SetPanelActive(GameObject panel, bool isActive)
        {
            if (panel != null)
            {
                panel.SetActive(isActive);
            }
        }

        public void ShowInterrogation(CaseFile caseFile, int selectedSuspectIndex, int questionsRemaining, string conversationHistory, string hintMessage, bool canAskQuestions)
        {
            if (interrogationPanelUI != null)
            {
                interrogationPanelUI.ShowState(caseFile, portraitLibrary, selectedSuspectIndex, questionsRemaining, conversationHistory, hintMessage, canAskQuestions);
            }
        }

        public void ClearQuestionInput()
        {
            if (interrogationPanelUI != null)
            {
                interrogationPanelUI.ClearQuestionInput();
            }
        }

        public void ShowInterrogationMessage(string message)
        {
            if (interrogationPanelUI != null)
            {
                interrogationPanelUI.ShowHint(message);
            }
        }

        public void ShowAccusation(CaseFile caseFile, int selectedAccusationIndex)
        {
            if (accusationPanelUI != null)
            {
                accusationPanelUI.ShowChoices(caseFile, portraitLibrary, selectedAccusationIndex);
            }
        }

        public void ShowResult(CaseFile caseFile, int accusedIndex, bool isCorrect)
        {
            if (resultPanelUI != null)
            {
                resultPanelUI.ShowResult(caseFile, portraitLibrary, accusedIndex, isCorrect);
            }
        }
    }
}
