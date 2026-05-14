using CaseFileLocalSuspect.UI;
using UnityEngine;

namespace CaseFileLocalSuspect.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private UIManager uiManager;

        private CaseFile currentCaseFile;

        public GameScreen CurrentScreen { get; private set; } = GameScreen.MainMenu;

        private void Start()
        {
            ShowScreen(GameScreen.MainMenu);
        }

        public void StartNewCase()
        {
            // Milestone 3 uses a preview case so the case briefing screen can be built and tested.
            currentCaseFile = PlaceholderCaseFactory.CreatePreviewCase();
            uiManager.ShowCaseBriefing(currentCaseFile);
            ShowScreen(GameScreen.CaseBriefing);
        }

        public void BeginInterrogation()
        {
            ShowScreen(GameScreen.Interrogation);
        }

        public void ReturnToMainMenu()
        {
            ShowScreen(GameScreen.MainMenu);
        }

        public void QuitGame()
        {
            Debug.Log("Quit requested from Main Menu.");
            Application.Quit();
        }

        public void ShowScreen(GameScreen screen)
        {
            CurrentScreen = screen;

            if (uiManager == null)
            {
                Debug.LogWarning("GameManager is missing a UIManager reference.");
                return;
            }

            uiManager.ShowScreen(screen);
        }
    }
}
