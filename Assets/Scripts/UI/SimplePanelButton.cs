using CaseFileLocalSuspect.Game;
using UnityEngine;

namespace CaseFileLocalSuspect.UI
{
    public class SimplePanelButton : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameScreen targetScreen = GameScreen.MainMenu;

        public void ShowTargetScreen()
        {
            if (gameManager == null)
            {
                Debug.LogWarning("SimplePanelButton is missing a GameManager reference.");
                return;
            }

            gameManager.ShowScreen(targetScreen);
        }
    }
}
