using CaseFileLocalSuspect.Game;
using TMPro;
using UnityEngine;

namespace CaseFileLocalSuspect.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject crimeBoardPanel;
        [SerializeField] private GameObject crimePanel;
        [SerializeField] private GameObject suspectsPanel;
        [SerializeField] private GameObject interrogationPanel;
        [SerializeField] private GameObject arrestPanel;
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioClip loadingMusicClip;
        [SerializeField] private AudioClip normalCaseMusicClip;
        [SerializeField] private AudioClip timedCaseMusicClip;

        [Header("Panel Controllers")]
        [SerializeField] private PortraitLibrary portraitLibrary;
        [SerializeField] private CrimeBoardPanelUI crimeBoardPanelUI;
        [SerializeField] private CrimeDetailsPanelUI crimeDetailsPanelUI;
        [SerializeField] private SuspectsPanelUI suspectsPanelUI;
        [SerializeField] private InterrogationPanelUI interrogationPanelUI;
        [SerializeField] private AccusationPanelUI accusationPanelUI;
        [SerializeField] private ResultPanelUI resultPanelUI;
        [SerializeField] private Color timerNormalColor = new Color(0.97f, 0.91f, 0.74f, 1f);
        [SerializeField] private Color timerUrgentColor = new Color(0.93f, 0.39f, 0.31f, 1f);

        public void ShowScreen(GameScreen screen)
        {
            SetPanelActive(mainMenuPanel, screen == GameScreen.MainMenu);
            SetPanelActive(crimeBoardPanel, screen == GameScreen.CrimeBoard);
            SetPanelActive(crimePanel, screen == GameScreen.Crime);
            SetPanelActive(suspectsPanel, screen == GameScreen.Suspects);
            SetPanelActive(interrogationPanel, screen == GameScreen.Interrogation);
            SetPanelActive(arrestPanel, screen == GameScreen.Arrest);
            SetPanelActive(resultPanel, screen == GameScreen.Result);
        }

        public void ShowCrimeBoard(CaseFile caseFile, bool viewedCrime, bool viewedSuspects, bool completedInterrogation, bool arrestUnlocked, int questionsRemaining, string systemMessage, bool isLoadingCase = false)
        {
            if (crimeBoardPanelUI != null)
            {
                crimeBoardPanelUI.ShowState(caseFile, viewedCrime, viewedSuspects, completedInterrogation, arrestUnlocked, questionsRemaining, systemMessage, isLoadingCase);
            }
        }

        public void ShowCrimeDetails(CaseFile caseFile)
        {
            if (crimeDetailsPanelUI != null)
            {
                crimeDetailsPanelUI.ShowCase(caseFile, portraitLibrary);
            }
        }

        public void ShowSuspects(CaseFile caseFile)
        {
            if (suspectsPanelUI != null)
            {
                suspectsPanelUI.ShowSuspects(caseFile, portraitLibrary);
            }
        }

        public void ShowInterrogation(CaseFile caseFile, int selectedSuspectIndex, int questionsRemaining, string conversationHistory, bool completedInterrogationStep, bool[] askedQuestionFlags)
        {
            if (interrogationPanelUI != null)
            {
                interrogationPanelUI.ShowState(caseFile, portraitLibrary, selectedSuspectIndex, questionsRemaining, conversationHistory, completedInterrogationStep, askedQuestionFlags);
            }
        }

        public void ShowInterrogationMessage(string message)
        {
            if (interrogationPanelUI != null)
            {
                interrogationPanelUI.ShowHint(message);
            }
        }

        public void ShowAccusation(CaseFile caseFile, int selectedAccusationIndex, bool forcedByTimeout)
        {
            if (accusationPanelUI != null)
            {
                accusationPanelUI.ShowChoices(caseFile, portraitLibrary, selectedAccusationIndex, forcedByTimeout);
            }
        }

        public void ShowResult(CaseFile caseFile, int accusedIndex, bool isCorrect)
        {
            if (resultPanelUI != null)
            {
                resultPanelUI.ShowResult(caseFile, portraitLibrary, accusedIndex, isCorrect);
            }
        }

        public void ShowTimer(bool isVisible, string message, bool isUrgent)
        {
            if (timerText == null)
            {
                return;
            }

            timerText.gameObject.SetActive(isVisible);
            timerText.text = message;
            timerText.color = isUrgent ? timerUrgentColor : timerNormalColor;
        }

        public void UpdateMusicForState(bool isGeneratingCase, bool hasCase, bool isTimedMode, GameScreen screen)
        {
            if (musicSource == null)
            {
                return;
            }

            AudioClip targetClip = null;

            if (screen == GameScreen.MainMenu || isGeneratingCase || !hasCase)
            {
                targetClip = loadingMusicClip;
            }
            else if (isTimedMode)
            {
                targetClip = timedCaseMusicClip;
            }
            else
            {
                targetClip = normalCaseMusicClip;
            }

            if (targetClip == null)
            {
                if (musicSource.isPlaying)
                {
                    musicSource.Stop();
                }

                musicSource.clip = null;
                return;
            }

            if (musicSource.clip != targetClip)
            {
                musicSource.Stop();
                musicSource.clip = targetClip;
            }

            if (!musicSource.isPlaying)
            {
                musicSource.loop = true;
                musicSource.Play();
            }
        }

        private static void SetPanelActive(GameObject panel, bool isActive)
        {
            if (panel != null)
            {
                panel.SetActive(isActive);
            }
        }
    }
}
