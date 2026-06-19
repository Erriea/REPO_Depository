using CaseFileLocalSuspect.AI;
using CaseFileLocalSuspect.Game;
using CaseFileLocalSuspect.UI;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CaseFileLocalSuspect.Editor
{
    public static class CaseFileSceneBuilder
    {
        private const string ScenePath = "Assets/Scenes/MainScene.unity";
        private const string BackgroundPath = "Assets/Art/Backgrounds/Background.png";
        private const string ResultDecorationPath = "Assets/Art/Backgrounds/Charizard-Pokemon-Fire-Dragon.png";
        private const string Suspect1Path = "Assets/Art/Suspects/FemCharacter1.png";
        private const string Suspect2Path = "Assets/Art/Suspects/FemCharacter2.png";
        private const string VictimPath = "Assets/Art/Suspects/MascCharacter1.png";
        private const string Suspect3Path = "Assets/Art/Suspects/MascCharacter2.png";
        private const string MainMenuMusicPath = "Assets/Audio/Music/openmindaudio-true-crime-a-name-crossed-out-in-black-ink-short-preview-493031.mp3";
        private const string NormalCaseMusicPath = "Assets/Audio/Music/zec53-quirky-awkward-detective-comedy-music-60-sec-499399.mp3";
        private const string TimedCaseMusicPath = "Assets/Audio/Music/openmindaudio-dark-suspense-thriller-cue-488018.mp3";
        private const string ButtonClickSfxPath = "Assets/Audio/SFX/soundreality-camera-shutter-171782.mp3";
        private const string CorrectOutcomeSfxPath = "Assets/Audio/SFX/pwlpl-applause-sound-effect-521104.mp3";
        private const string IncorrectOutcomeSfxPath = "Assets/Audio/SFX/dragon-studio-crowd-booing-494319.mp3";
        private const string DogtownFontPath = "Assets/Fonts/Dogtown/Dogtown Typewriter.ttf";
        private const string TypoWriterFontPath = "Assets/Fonts/TypoWriter/typo-writer.distressed-demo.otf";
        private const string DogtownTmpFontPath = "Assets/Fonts/Dogtown/Dogtown Typewriter TMP.asset";
        private const string TypoWriterTmpFontPath = "Assets/Fonts/TypoWriter/Typo Writer Distressed TMP.asset";
        private const float BodyFontScale = 1.35f;

        private static TMP_FontAsset headingFontAsset;
        private static TMP_FontAsset bodyFontAsset;
        private static UIClickSoundPlayer clickSoundPlayer;
        private static Sprite panelBackdropSprite;

        [MenuItem("Tools/CaseFile/Build Assignment Scene")]
        public static void BuildAssignmentScene()
        {
            AssetDatabase.Refresh();
            ConfigureSpriteImport(BackgroundPath);
            ConfigureSpriteImport(ResultDecorationPath);
            ConfigureSpriteImport(Suspect1Path);
            ConfigureSpriteImport(Suspect2Path);
            ConfigureSpriteImport(VictimPath);
            ConfigureSpriteImport(Suspect3Path);
            AssetDatabase.Refresh();
            headingFontAsset = EnsureTmpFontAsset(DogtownFontPath, DogtownTmpFontPath);
            bodyFontAsset = EnsureTmpFontAsset(TypoWriterFontPath, TypoWriterTmpFontPath);

            Sprite backgroundSprite = AssetDatabase.LoadAssetAtPath<Sprite>(BackgroundPath);
            Sprite resultDecorationSprite = AssetDatabase.LoadAssetAtPath<Sprite>(ResultDecorationPath);
            panelBackdropSprite = backgroundSprite;
            Sprite femCharacter1 = AssetDatabase.LoadAssetAtPath<Sprite>(Suspect1Path);
            Sprite femCharacter2 = AssetDatabase.LoadAssetAtPath<Sprite>(Suspect2Path);
            Sprite mascCharacter1 = AssetDatabase.LoadAssetAtPath<Sprite>(VictimPath);
            Sprite mascCharacter2 = AssetDatabase.LoadAssetAtPath<Sprite>(Suspect3Path);
            AudioClip mainMenuMusicClip = AssetDatabase.LoadAssetAtPath<AudioClip>(MainMenuMusicPath);
            AudioClip normalCaseMusicClip = AssetDatabase.LoadAssetAtPath<AudioClip>(NormalCaseMusicPath);
            AudioClip timedCaseMusicClip = AssetDatabase.LoadAssetAtPath<AudioClip>(TimedCaseMusicPath);
            AudioClip buttonClickSfx = AssetDatabase.LoadAssetAtPath<AudioClip>(ButtonClickSfxPath);
            AudioClip correctOutcomeSfx = AssetDatabase.LoadAssetAtPath<AudioClip>(CorrectOutcomeSfxPath);
            AudioClip incorrectOutcomeSfx = AssetDatabase.LoadAssetAtPath<AudioClip>(IncorrectOutcomeSfxPath);

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            GameObject cameraObject = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
            Camera camera = cameraObject.GetComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.08f, 0.08f, 0.1f, 1f);
            camera.orthographic = true;
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(0f, 0f, -10f);

            GameObject canvasObject = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler canvasScaler = canvasObject.GetComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
            canvasScaler.matchWidthOrHeight = 0.5f;

            new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));

            GameObject uiClickAudioObject = new GameObject("UIClickAudio");
            AudioSource uiClickAudioSource = uiClickAudioObject.AddComponent<AudioSource>();
            uiClickAudioSource.playOnAwake = false;
            uiClickAudioSource.loop = false;
            uiClickAudioSource.volume = 1f;
            clickSoundPlayer = uiClickAudioObject.AddComponent<UIClickSoundPlayer>();
            SetSerializedReference(clickSoundPlayer, "audioSource", uiClickAudioSource);
            SetSerializedReference(clickSoundPlayer, "clickClip", buttonClickSfx);

            GameObject backgroundObject = CreatePanel("Background", canvasObject.transform, new Color(0f, 0f, 0f, 0f));
            Image backgroundImage = backgroundObject.GetComponent<Image>();
            backgroundImage.sprite = backgroundSprite;
            backgroundImage.preserveAspect = false;
            backgroundImage.raycastTarget = false;

            GameObject overlayObject = CreatePanel("Overlay", canvasObject.transform, new Color(0.06f, 0.06f, 0.07f, 0.32f));
            overlayObject.GetComponent<Image>().raycastTarget = false;

            GameObject mainMenuPanel = CreatePanel("MainMenuPanel", canvasObject.transform, new Color(0f, 0f, 0f, 0f));
            ApplyBoardPanelBackdrop(mainMenuPanel, new Color(0.36f, 0.36f, 0.36f, 0.88f), new Vector2(1680f, 900f));
            CreateHeadingText("MenuTitle", mainMenuPanel.transform, "CaseFile: Local Suspect", 84, TextAlignmentOptions.Center, new Vector2(0.5f, 0.82f), new Vector2(1200f, 120f));
            CreateText("MenuSubtitle", mainMenuPanel.transform, "Review the board, inspect the evidence, spend your questions wisely, and make the arrest.", 30, TextAlignmentOptions.Center, new Vector2(0.5f, 0.70f), new Vector2(1240f, 90f), new Color(0.92f, 0.84f, 0.68f));
            Button newCaseButton = CreateButton("NewCaseButton", mainMenuPanel.transform, "New Case", new Vector2(0.5f, 0.54f), new Vector2(360f, 86f));
            Button timedCaseButton = CreateButton("TimedCaseButton", mainMenuPanel.transform, "Timed Case", new Vector2(0.5f, 0.42f), new Vector2(360f, 86f));
            Button quitButton = CreateButton("QuitButton", mainMenuPanel.transform, "Quit", new Vector2(0.5f, 0.30f), new Vector2(360f, 86f));

            TMP_Text timerText = CreateText("GlobalTimerText", canvasObject.transform, string.Empty, 28, TextAlignmentOptions.Right, new Vector2(0.84f, 0.96f), new Vector2(360f, 46f), new Color(0.97f, 0.91f, 0.74f));
            timerText.enableAutoSizing = false;
            timerText.gameObject.SetActive(false);

            GameObject crimeBoardPanel = CreatePanel("CrimeBoardPanel", canvasObject.transform, new Color(0f, 0f, 0f, 0f));
            ApplyBoardPanelBackdrop(crimeBoardPanel, new Color(0.23f, 0.22f, 0.22f, 0.72f), new Vector2(1760f, 940f));
            CrimeBoardPanelUI crimeBoardUI = crimeBoardPanel.AddComponent<CrimeBoardPanelUI>();
            CreateHeadingText("BoardHeading", crimeBoardPanel.transform, "Crime Board", 64, TextAlignmentOptions.Center, new Vector2(0.5f, 0.94f), new Vector2(1000f, 80f), new Color(0.97f, 0.91f, 0.74f));
            TMP_Text boardCaseTitle = CreateHeadingText("BoardCaseTitle", crimeBoardPanel.transform, "Case Title", 42, TextAlignmentOptions.Center, new Vector2(0.5f, 0.86f), new Vector2(1300f, 70f));
            TMP_Text boardSummary = CreateText("BoardSummary", crimeBoardPanel.transform, "Summary", 24, TextAlignmentOptions.TopLeft, new Vector2(0.33f, 0.64f), new Vector2(1080f, 270f));
            TMP_Text boardProgress = CreateText("BoardProgress", crimeBoardPanel.transform, "Progress", 24, TextAlignmentOptions.TopLeft, new Vector2(0.77f, 0.64f), new Vector2(460f, 240f), new Color(0.92f, 0.84f, 0.68f));
            TMP_Text arrestStatus = CreateText("ArrestStatus", crimeBoardPanel.transform, "Arrest status", 22, TextAlignmentOptions.TopLeft, new Vector2(0.77f, 0.40f), new Vector2(460f, 130f));
            Button crimeButton = CreateButton("CrimeButton", crimeBoardPanel.transform, "The Crime", new Vector2(0.22f, 0.18f), new Vector2(260f, 80f));
            Button suspectsButton = CreateButton("SuspectsButton", crimeBoardPanel.transform, "The Suspects", new Vector2(0.42f, 0.18f), new Vector2(260f, 80f));
            Button interrogationButton = CreateButton("InterrogationButton", crimeBoardPanel.transform, "Interrogation", new Vector2(0.62f, 0.18f), new Vector2(260f, 80f));
            Button arrestButton = CreateButton("ArrestButton", crimeBoardPanel.transform, "Arrest", new Vector2(0.82f, 0.18f), new Vector2(260f, 80f));
            Button boardMenuButton = CreateButton("BoardMenuButton", crimeBoardPanel.transform, "Main Menu", new Vector2(0.13f, 0.08f), new Vector2(220f, 64f));
            GameObject loadingOverlay = CreatePanel("LoadingOverlay", crimeBoardPanel.transform, new Color(0.02f, 0.02f, 0.03f, 0.72f));
            loadingOverlay.transform.SetAsLastSibling();
            Image loadingOverlayImage = loadingOverlay.GetComponent<Image>();
            loadingOverlayImage.raycastTarget = true;
            TMP_Text loadingOverlayText = CreateHeadingText("LoadingOverlayText", loadingOverlay.transform, "Case Files Loading...", 44, TextAlignmentOptions.Center, new Vector2(0.5f, 0.54f), new Vector2(960f, 220f), new Color(0.97f, 0.91f, 0.74f));
            loadingOverlayText.enableAutoSizing = false;
            loadingOverlayText.fontSize = 34f;
            loadingOverlay.SetActive(false);

            GameObject crimePanel = CreatePanel("CrimePanel", canvasObject.transform, new Color(0f, 0f, 0f, 0f));
            ApplyBoardPanelBackdrop(crimePanel, new Color(0.23f, 0.22f, 0.22f, 0.72f), new Vector2(1760f, 940f));
            CrimeDetailsPanelUI crimeDetailsUI = crimePanel.AddComponent<CrimeDetailsPanelUI>();
            CreateHeadingText("CrimeHeading", crimePanel.transform, "The Crime", 60, TextAlignmentOptions.Center, new Vector2(0.5f, 0.94f), new Vector2(1000f, 80f), new Color(0.97f, 0.91f, 0.74f));
            TMP_Text crimeCaseTitle = CreateHeadingText("CrimeCaseTitle", crimePanel.transform, "Case Title", 40, TextAlignmentOptions.Center, new Vector2(0.5f, 0.87f), new Vector2(1200f, 60f));
            Image victimPortrait = CreateImage("VictimPortrait", crimePanel.transform, null, new Vector2(0.10f, 0.61f), new Vector2(300f, 380f), true);
            TMP_Text locationText = CreateText("LocationText", crimePanel.transform, "Location", 25, TextAlignmentOptions.TopLeft, new Vector2(0.47f, 0.73f), new Vector2(900f, 100f));
            TMP_Text victimText = CreateText("VictimText", crimePanel.transform, "Victim", 24, TextAlignmentOptions.TopLeft, new Vector2(0.47f, 0.54f), new Vector2(900f, 200f));
            TMP_Text crimeText = CreateText("CrimeText", crimePanel.transform, "What happened", 22, TextAlignmentOptions.TopLeft, new Vector2(0.47f, 0.31f), new Vector2(900f, 170f));
            TMP_Text cluesText = CreateText("CluesText", crimePanel.transform, "Clues", 21, TextAlignmentOptions.TopLeft, new Vector2(0.41f, 0.18f), new Vector2(820f, 240f));
            TMP_Text suspectsSummaryText = CreateText("SuspectsSummaryText", crimePanel.transform, "Suspects", 21, TextAlignmentOptions.TopLeft, new Vector2(0.82f, 0.18f), new Vector2(300f, 240f));
            locationText.enableAutoSizing = false;
            locationText.fontSize = 22f;
            victimText.enableAutoSizing = false;
            victimText.fontSize = 18f;
            crimeText.enableAutoSizing = false;
            crimeText.fontSize = 18f;
            cluesText.enableAutoSizing = false;
            cluesText.fontSize = 18f;
            suspectsSummaryText.enableAutoSizing = false;
            suspectsSummaryText.fontSize = 18f;
            Button crimeBackButton = CreateButton("CrimeBackButton", crimePanel.transform, "Back to Board", new Vector2(0.16f, 0.08f), new Vector2(240f, 64f));

            GameObject suspectsPanel = CreatePanel("SuspectsPanel", canvasObject.transform, new Color(0f, 0f, 0f, 0f));
            ApplyBoardPanelBackdrop(suspectsPanel, new Color(0.23f, 0.22f, 0.22f, 0.70f), new Vector2(1760f, 940f));
            SuspectsPanelUI suspectsUI = suspectsPanel.AddComponent<SuspectsPanelUI>();
            CreateHeadingText("SuspectsHeading", suspectsPanel.transform, "The Suspects", 60, TextAlignmentOptions.Center, new Vector2(0.5f, 0.94f), new Vector2(1000f, 80f), new Color(0.97f, 0.91f, 0.74f));
            CreateText("SuspectsInstruction", suspectsPanel.transform, "Review one suspect at a time so the details stay readable.", 22, TextAlignmentOptions.Center, new Vector2(0.5f, 0.85f), new Vector2(900f, 40f), new Color(0.92f, 0.84f, 0.68f));
            GameObject suspectCardObject = CreatePanel("SuspectDetailCard", suspectsPanel.transform, new Color(0.10f, 0.10f, 0.10f, 0.78f));
            SetAnchoredRect(suspectCardObject.GetComponent<RectTransform>(), new Vector2(0.5f, 0.48f), new Vector2(940f, 760f));
            SuspectCardUI suspectCard = suspectCardObject.AddComponent<SuspectCardUI>();
            Image suspectPortrait = CreateImage("Portrait", suspectCardObject.transform, null, new Vector2(0.5f, 0.84f), new Vector2(220f, 220f), true);
            TMP_Text suspectNameText = CreateHeadingText("NameText", suspectCardObject.transform, "Suspect", 40, TextAlignmentOptions.Center, new Vector2(0.5f, 0.68f), new Vector2(700f, 54f), new Color(0.97f, 0.91f, 0.74f));
            GameObject profilePanel = CreatePanel("ProfilePanel", suspectCardObject.transform, new Color(0.14f, 0.14f, 0.13f, 0.92f));
            SetAnchoredRect(profilePanel.GetComponent<RectTransform>(), new Vector2(0.28f, 0.42f), new Vector2(360f, 220f));
            TMP_Text suspectDescriptionText = CreateText("SuspectDescriptionText", profilePanel.transform, "Description", 18, TextAlignmentOptions.TopLeft, new Vector2(0.5f, 0.5f), new Vector2(320f, 180f));
            suspectDescriptionText.enableAutoSizing = true;
            suspectDescriptionText.fontSizeMax = 18f;
            suspectDescriptionText.fontSizeMin = 13f;
            suspectDescriptionText.overflowMode = TextOverflowModes.Overflow;

            GameObject appearancePanel = CreatePanel("AppearancePanel", suspectCardObject.transform, new Color(0.14f, 0.14f, 0.13f, 0.92f));
            SetAnchoredRect(appearancePanel.GetComponent<RectTransform>(), new Vector2(0.72f, 0.42f), new Vector2(360f, 220f));
            TMP_Text suspectAppearanceText = CreateText("SuspectAppearanceText", appearancePanel.transform, "Appearance", 18, TextAlignmentOptions.TopLeft, new Vector2(0.5f, 0.5f), new Vector2(320f, 180f));
            suspectAppearanceText.enableAutoSizing = true;
            suspectAppearanceText.fontSizeMax = 18f;
            suspectAppearanceText.fontSizeMin = 13f;
            suspectAppearanceText.overflowMode = TextOverflowModes.Overflow;

            GameObject casePanel = CreatePanel("CasePanel", suspectCardObject.transform, new Color(0.14f, 0.14f, 0.13f, 0.92f));
            SetAnchoredRect(casePanel.GetComponent<RectTransform>(), new Vector2(0.28f, 0.19f), new Vector2(360f, 180f));
            TMP_Text suspectCaseText = CreateText("SuspectCaseText", casePanel.transform, "Case Notes", 18, TextAlignmentOptions.TopLeft, new Vector2(0.5f, 0.5f), new Vector2(320f, 140f));
            suspectCaseText.enableAutoSizing = true;
            suspectCaseText.fontSizeMax = 18f;
            suspectCaseText.fontSizeMin = 13f;
            suspectCaseText.overflowMode = TextOverflowModes.Overflow;

            GameObject motivePanel = CreatePanel("MotivePanel", suspectCardObject.transform, new Color(0.14f, 0.14f, 0.13f, 0.92f));
            SetAnchoredRect(motivePanel.GetComponent<RectTransform>(), new Vector2(0.72f, 0.19f), new Vector2(360f, 180f));
            TMP_Text suspectMotiveText = CreateText("SuspectMotiveText", motivePanel.transform, "Motive", 18, TextAlignmentOptions.TopLeft, new Vector2(0.5f, 0.5f), new Vector2(320f, 140f));
            suspectMotiveText.enableAutoSizing = true;
            suspectMotiveText.fontSizeMax = 18f;
            suspectMotiveText.fontSizeMin = 13f;
            suspectMotiveText.overflowMode = TextOverflowModes.Overflow;
            SetSerializedReference(suspectCard, "portraitImage", suspectPortrait);
            SetSerializedReference(suspectCard, "suspectNameText", suspectNameText);
            SetSerializedReference(suspectCard, "descriptionText", suspectDescriptionText);
            SetSerializedReference(suspectCard, "appearanceText", suspectAppearanceText);
            SetSerializedReference(suspectCard, "caseText", suspectCaseText);
            SetSerializedReference(suspectCard, "motiveText", suspectMotiveText);
            Button previousSuspectButton = CreateButton("PreviousSuspectButton", suspectsPanel.transform, "Previous Suspect", new Vector2(0.32f, 0.12f), new Vector2(280f, 68f));
            Button nextSuspectButton = CreateButton("NextSuspectButton", suspectsPanel.transform, "Next Suspect", new Vector2(0.68f, 0.12f), new Vector2(280f, 68f));
            Button suspectsBackButton = CreateButton("SuspectsBackButton", suspectsPanel.transform, "Back to Board", new Vector2(0.16f, 0.08f), new Vector2(240f, 64f));
            StyleAccentButton(previousSuspectButton);
            StyleAccentButton(nextSuspectButton);

            GameObject interrogationPanel = CreatePanel("InterrogationPanel", canvasObject.transform, new Color(0f, 0f, 0f, 0f));
            ApplyBoardPanelBackdrop(interrogationPanel, new Color(0.23f, 0.23f, 0.24f, 0.72f), new Vector2(1760f, 940f));
            InterrogationPanelUI interrogationUI = interrogationPanel.AddComponent<InterrogationPanelUI>();
            CreateHeadingText("InterrogationHeading", interrogationPanel.transform, "Interrogation", 60, TextAlignmentOptions.Center, new Vector2(0.5f, 0.94f), new Vector2(1000f, 80f), new Color(0.97f, 0.91f, 0.74f));
            SuspectChoiceButtonUI[] interrogationButtons = new SuspectChoiceButtonUI[3];
            for (int i = 0; i < interrogationButtons.Length; i++)
            {
                float x = 0.24f + (i * 0.20f);
                interrogationButtons[i] = CreateSuspectChoiceButton(interrogationPanel.transform, $"InterrogationSuspectButton{i + 1}", x, 0.79f, SuspectChoiceButtonUI.ChoiceMode.Interrogation, i);
                RectTransform buttonRect = interrogationButtons[i].GetComponent<RectTransform>();
                SetRectSize(buttonRect, new Vector2(205f, 130f));
                Transform portraitTransform = interrogationButtons[i].transform.Find("Portrait");
                if (portraitTransform != null)
                {
                    SetRectSize(portraitTransform.GetComponent<RectTransform>(), new Vector2(95f, 95f));
                }

                Transform frameTransform = interrogationButtons[i].transform.Find("SelectionFrame");
                if (frameTransform != null)
                {
                    SetRectSize(frameTransform.GetComponent<RectTransform>(), new Vector2(108f, 108f));
                }

                Transform labelTransform = interrogationButtons[i].transform.Find("Label");
                if (labelTransform != null)
                {
                    TMP_Text labelText = labelTransform.GetComponent<TMP_Text>();
                    labelText.enableAutoSizing = false;
                    labelText.fontSize = 15f;
                    SetRectSize(labelTransform.GetComponent<RectTransform>(), new Vector2(180f, 34f));
                }
            }

            Image interrogationPortrait = CreateImage("SelectedPortrait", interrogationPanel.transform, null, new Vector2(0.14f, 0.58f), new Vector2(300f, 360f), true);
            TMP_Text interrogationName = CreateHeadingText("SelectedSuspectName", interrogationPanel.transform, "Selected Suspect", 34, TextAlignmentOptions.Center, new Vector2(0.14f, 0.31f), new Vector2(320f, 50f), new Color(0.97f, 0.91f, 0.74f));
            TMP_Text interrogationRole = CreateText("SelectedSuspectRole", interrogationPanel.transform, "Who They Are", 18, TextAlignmentOptions.TopLeft, new Vector2(0.14f, 0.20f), new Vector2(340f, 120f));
            TMP_Text questionsRemaining = CreateText("QuestionsRemaining", interrogationPanel.transform, "Questions Remaining: 4", 24, TextAlignmentOptions.Center, new Vector2(0.52f, 0.72f), new Vector2(360f, 42f), new Color(0.97f, 0.91f, 0.74f));
            TMP_Text interrogationStatus = CreateText("InterrogationStatus", interrogationPanel.transform, "Interrogation status", 18, TextAlignmentOptions.TopLeft, new Vector2(0.82f, 0.76f), new Vector2(360f, 90f));
            TMP_Text answerGuide = CreateText("AnswerGuide", interrogationPanel.transform, "The full exchange appears below after you click a question.", 18, TextAlignmentOptions.Center, new Vector2(0.50f, 0.68f), new Vector2(760f, 36f), new Color(0.92f, 0.84f, 0.68f));
            TMP_Text conversationText = CreateText("ConversationText", interrogationPanel.transform, "Conversation", 24, TextAlignmentOptions.TopLeft, new Vector2(0.50f, 0.43f), new Vector2(900f, 430f));
            conversationText.enableAutoSizing = false;
            conversationText.fontSize = 18f;
            interrogationRole.enableAutoSizing = false;
            interrogationRole.fontSize = 16f;
            questionsRemaining.enableAutoSizing = false;
            questionsRemaining.fontSize = 18f;
            interrogationStatus.enableAutoSizing = false;
            interrogationStatus.fontSize = 16f;
            answerGuide.enableAutoSizing = false;
            answerGuide.fontSize = 16f;

            TMP_Text hintText = CreateText("HintText", interrogationPanel.transform, "Choose a suspect, then spend one of the four shared questions.", 20, TextAlignmentOptions.Center, new Vector2(0.50f, 0.12f), new Vector2(820f, 42f), new Color(0.85f, 0.82f, 0.7f));
            hintText.enableAutoSizing = false;
            hintText.fontSize = 16f;
            QuestionChoiceButtonUI[] questionButtons = new QuestionChoiceButtonUI[4];
            for (int i = 0; i < questionButtons.Length; i++)
            {
                float y = 0.66f - (i * 0.15f);
                questionButtons[i] = CreateQuestionButton(interrogationPanel.transform, $"QuestionButton{i + 1}", new Vector2(0.84f, y), new Vector2(380f, 120f), i);
            }
            Button interrogationBackButton = CreateButton("InterrogationBackButton", interrogationPanel.transform, "Back to Board", new Vector2(0.16f, 0.08f), new Vector2(240f, 64f));

            GameObject arrestPanel = CreatePanel("ArrestPanel", canvasObject.transform, new Color(0f, 0f, 0f, 0f));
            ApplyBoardPanelBackdrop(arrestPanel, new Color(0.23f, 0.22f, 0.23f, 0.72f), new Vector2(1760f, 940f));
            AccusationPanelUI accusationUI = arrestPanel.AddComponent<AccusationPanelUI>();
            TMP_Text accusationPrompt = CreateHeadingText("PromptText", arrestPanel.transform, "Choose the suspect you want to arrest.", 40, TextAlignmentOptions.Center, new Vector2(0.5f, 0.90f), new Vector2(1300f, 120f));
            SuspectChoiceButtonUI[] accusationButtons = new SuspectChoiceButtonUI[3];
            for (int i = 0; i < accusationButtons.Length; i++)
            {
                float x = 0.2f + (i * 0.3f);
                accusationButtons[i] = CreateSuspectChoiceButton(arrestPanel.transform, $"ArrestSuspectButton{i + 1}", x, 0.52f, SuspectChoiceButtonUI.ChoiceMode.Accusation, i);
                SetRectSize(accusationButtons[i].GetComponent<RectTransform>(), new Vector2(360f, 470f));
            }
            Button confirmAccusationButton = CreateButton("ConfirmAccusationButton", arrestPanel.transform, "Confirm Arrest", new Vector2(0.68f, 0.12f), new Vector2(330f, 76f));
            Button backToBoardButton = CreateButton("BackButton", arrestPanel.transform, "Back to Board", new Vector2(0.32f, 0.12f), new Vector2(240f, 76f));

            GameObject resultPanel = CreatePanel("ResultPanel", canvasObject.transform, new Color(0f, 0f, 0f, 0f));
            ApplyBoardPanelBackdrop(resultPanel, new Color(0.22f, 0.22f, 0.23f, 0.72f), new Vector2(1760f, 940f));
            ResultPanelUI resultUI = resultPanel.AddComponent<ResultPanelUI>();
            TMP_Text resultHeader = CreateHeadingText("ResultHeader", resultPanel.transform, "Outcome", 62, TextAlignmentOptions.Center, new Vector2(0.5f, 0.93f), new Vector2(1000f, 82f), new Color(0.97f, 0.91f, 0.74f));
            Image guiltyPortrait = CreateImage("GuiltyPortrait", resultPanel.transform, null, new Vector2(0.18f, 0.58f), new Vector2(320f, 400f), true);
            Image resultDecorationLeft = CreateImage("ResultDecorationLeft", resultPanel.transform, resultDecorationSprite, new Vector2(0.14f, 0.11f), new Vector2(360f, 280f), true, Color.white);
            Image resultDecorationRight = CreateImage("ResultDecorationRight", resultPanel.transform, resultDecorationSprite, new Vector2(0.86f, 0.11f), new Vector2(360f, 280f), true, Color.white);
            resultDecorationLeft.raycastTarget = false;
            resultDecorationRight.raycastTarget = false;
            TMP_Text accusedText = CreateText("AccusedText", resultPanel.transform, "You accused:", 28, TextAlignmentOptions.Left, new Vector2(0.60f, 0.77f), new Vector2(820f, 50f));
            TMP_Text guiltyText = CreateText("GuiltyText", resultPanel.transform, "Actual culprit:", 28, TextAlignmentOptions.Left, new Vector2(0.60f, 0.69f), new Vector2(820f, 70f));
            TMP_Text clueText = CreateText("ClueText", resultPanel.transform, "Clues", 24, TextAlignmentOptions.TopLeft, new Vector2(0.60f, 0.51f), new Vector2(820f, 150f), new Color(0.97f, 0.91f, 0.74f));
            TMP_Text explanationText = CreateText("ExplanationText", resultPanel.transform, "Explanation", 22, TextAlignmentOptions.TopLeft, new Vector2(0.60f, 0.28f), new Vector2(820f, 220f));
            Button resultMenuButton = CreateButton("ResultMenuButton", resultPanel.transform, "Main Menu", new Vector2(0.5f, 0.10f), new Vector2(280f, 76f));
            resultDecorationLeft.transform.SetAsFirstSibling();
            resultDecorationRight.transform.SetAsFirstSibling();

            GameObject gameManagerObject = new GameObject("GameManager");
            GameManager gameManager = gameManagerObject.AddComponent<GameManager>();
            GameObject ollamaClientObject = new GameObject("OllamaClient");
            OllamaClient ollamaClient = ollamaClientObject.AddComponent<OllamaClient>();
            GameObject uiManagerObject = new GameObject("UIManager");
            UIManager uiManager = uiManagerObject.AddComponent<UIManager>();
            GameObject portraitLibraryObject = new GameObject("PortraitLibrary");
            PortraitLibrary portraitLibrary = portraitLibraryObject.AddComponent<PortraitLibrary>();
            GameObject menuMusicObject = new GameObject("MainMenuMusic");
            AudioSource menuMusicSource = menuMusicObject.AddComponent<AudioSource>();
            menuMusicSource.playOnAwake = false;
            menuMusicSource.loop = true;
            menuMusicSource.clip = mainMenuMusicClip;
            menuMusicSource.volume = 0.45f;
            GameObject resultAudioObject = new GameObject("ResultAudio");
            AudioSource resultAudioSource = resultAudioObject.AddComponent<AudioSource>();
            resultAudioSource.playOnAwake = false;
            resultAudioSource.loop = false;
            resultAudioSource.volume = 1f;

            SetPortraitLibrary(portraitLibrary, femCharacter1, femCharacter2, mascCharacter1, mascCharacter2);

            SetSerializedReference(uiManager, "mainMenuPanel", mainMenuPanel);
            SetSerializedReference(uiManager, "crimeBoardPanel", crimeBoardPanel);
            SetSerializedReference(uiManager, "crimePanel", crimePanel);
            SetSerializedReference(uiManager, "suspectsPanel", suspectsPanel);
            SetSerializedReference(uiManager, "interrogationPanel", interrogationPanel);
            SetSerializedReference(uiManager, "arrestPanel", arrestPanel);
            SetSerializedReference(uiManager, "resultPanel", resultPanel);
            SetSerializedReference(uiManager, "portraitLibrary", portraitLibrary);
            SetSerializedReference(uiManager, "crimeBoardPanelUI", crimeBoardUI);
            SetSerializedReference(uiManager, "crimeDetailsPanelUI", crimeDetailsUI);
            SetSerializedReference(uiManager, "suspectsPanelUI", suspectsUI);
            SetSerializedReference(uiManager, "interrogationPanelUI", interrogationUI);
            SetSerializedReference(uiManager, "accusationPanelUI", accusationUI);
            SetSerializedReference(uiManager, "resultPanelUI", resultUI);
            SetSerializedReference(uiManager, "timerText", timerText);
            SetSerializedReference(uiManager, "musicSource", menuMusicSource);
            SetSerializedReference(uiManager, "loadingMusicClip", mainMenuMusicClip);
            SetSerializedReference(uiManager, "normalCaseMusicClip", normalCaseMusicClip);
            SetSerializedReference(uiManager, "timedCaseMusicClip", timedCaseMusicClip);
            SetSerializedReference(gameManager, "uiManager", uiManager);
            SetSerializedReference(gameManager, "ollamaClient", ollamaClient);

            SetSerializedReference(crimeBoardUI, "caseTitleText", boardCaseTitle);
            SetSerializedReference(crimeBoardUI, "summaryText", boardSummary);
            SetSerializedReference(crimeBoardUI, "progressText", boardProgress);
            SetSerializedReference(crimeBoardUI, "arrestStatusText", arrestStatus);
            SetSerializedReference(crimeBoardUI, "arrestButton", arrestButton);
            SetSerializedReference(crimeBoardUI, "loadingOverlay", loadingOverlay);
            SetSerializedReference(crimeBoardUI, "loadingOverlayText", loadingOverlayText);

            SetSerializedReference(crimeDetailsUI, "caseTitleText", crimeCaseTitle);
            SetSerializedReference(crimeDetailsUI, "locationText", locationText);
            SetSerializedReference(crimeDetailsUI, "victimText", victimText);
            SetSerializedReference(crimeDetailsUI, "crimeText", crimeText);
            SetSerializedReference(crimeDetailsUI, "cluesText", cluesText);
            SetSerializedReference(crimeDetailsUI, "suspectsText", suspectsSummaryText);
            SetSerializedReference(crimeDetailsUI, "victimPortraitImage", victimPortrait);

            SetSerializedReference(suspectsUI, "suspectCard", suspectCard);
            SetSerializedReference(suspectsUI, "previousButton", previousSuspectButton);
            SetSerializedReference(suspectsUI, "nextButton", nextSuspectButton);

            SetSerializedReference(interrogationUI, "selectedSuspectNameText", interrogationName);
            SetSerializedReference(interrogationUI, "selectedSuspectRoleText", interrogationRole);
            SetSerializedReference(interrogationUI, "questionsRemainingText", questionsRemaining);
            SetSerializedReference(interrogationUI, "conversationHistoryText", conversationText);
            SetSerializedReference(interrogationUI, "hintText", hintText);
            SetSerializedReference(interrogationUI, "interrogationStatusText", interrogationStatus);
            SetSerializedReference(interrogationUI, "answerGuideText", answerGuide);
            SetSerializedReference(interrogationUI, "selectedSuspectPortraitImage", interrogationPortrait);
            SetSerializedArray(interrogationUI, "suspectButtons", interrogationButtons);
            SetSerializedArray(interrogationUI, "questionButtons", questionButtons);

            SetSerializedReference(accusationUI, "gameManager", gameManager);
            SetSerializedReference(accusationUI, "promptText", accusationPrompt);
            SetSerializedReference(accusationUI, "confirmButton", confirmAccusationButton);
            SetSerializedReference(accusationUI, "panelBackground", arrestPanel.GetComponent<Image>());
            SetSerializedArray(accusationUI, "suspectButtons", accusationButtons);

            SetSerializedReference(resultUI, "gameManager", gameManager);
            SetSerializedReference(resultUI, "resultHeaderText", resultHeader);
            SetSerializedReference(resultUI, "accusedSuspectText", accusedText);
            SetSerializedReference(resultUI, "guiltySuspectText", guiltyText);
            SetSerializedReference(resultUI, "keyClueText", clueText);
            SetSerializedReference(resultUI, "explanationText", explanationText);
            SetSerializedReference(resultUI, "guiltySuspectPortraitImage", guiltyPortrait);
            SetSerializedReference(resultUI, "outcomeAudioSource", resultAudioSource);
            SetSerializedReference(resultUI, "correctOutcomeClip", correctOutcomeSfx);
            SetSerializedReference(resultUI, "incorrectOutcomeClip", incorrectOutcomeSfx);

            for (int i = 0; i < interrogationButtons.Length; i++)
            {
                SetSerializedReference(interrogationButtons[i], "gameManager", gameManager);
            }

            for (int i = 0; i < accusationButtons.Length; i++)
            {
                SetSerializedReference(accusationButtons[i], "gameManager", gameManager);
            }

            for (int i = 0; i < questionButtons.Length; i++)
            {
                SetSerializedReference(questionButtons[i], "gameManager", gameManager);
            }

            AddButtonListener(newCaseButton, gameManager, nameof(GameManager.StartNewCase));
            AddButtonListener(timedCaseButton, gameManager, nameof(GameManager.StartTimedCase));
            AddButtonListener(quitButton, gameManager, nameof(GameManager.QuitGame));
            AddButtonListener(crimeButton, gameManager, nameof(GameManager.OpenCrimeScreen));
            AddButtonListener(suspectsButton, gameManager, nameof(GameManager.OpenSuspectsScreen));
            AddButtonListener(interrogationButton, gameManager, nameof(GameManager.OpenInterrogationScreen));
            AddButtonListener(arrestButton, gameManager, nameof(GameManager.OpenAccusation));
            AddButtonListener(boardMenuButton, gameManager, nameof(GameManager.ReturnToMainMenu));
            AddButtonListener(crimeBackButton, gameManager, nameof(GameManager.ReturnToCrimeBoard));
            AddButtonListener(previousSuspectButton, suspectsUI, nameof(SuspectsPanelUI.ShowPreviousSuspect));
            AddButtonListener(nextSuspectButton, suspectsUI, nameof(SuspectsPanelUI.ShowNextSuspect));
            AddButtonListener(suspectsBackButton, gameManager, nameof(GameManager.ReturnToCrimeBoard));
            AddButtonListener(interrogationBackButton, gameManager, nameof(GameManager.ReturnToCrimeBoard));
            AddButtonListener(confirmAccusationButton, accusationUI, nameof(AccusationPanelUI.ConfirmAccusationPressed));
            AddButtonListener(backToBoardButton, accusationUI, nameof(AccusationPanelUI.BackPressed));
            AddButtonListener(resultMenuButton, resultUI, nameof(ResultPanelUI.StartNewCasePressed));

            SetScenePanelDefaults(mainMenuPanel, crimeBoardPanel, crimePanel, suspectsPanel, interrogationPanel, arrestPanel, resultPanel);

            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(ScenePath, true)
            };
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("CaseFile", "MainScene has been rebuilt for the investigation-board flow.", "OK");
        }

        private static void ConfigureSpriteImport(string assetPath)
        {
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer == null)
            {
                return;
            }

            if (importer.textureType != TextureImporterType.Sprite || importer.spriteImportMode != SpriteImportMode.Single)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.SaveAndReimport();
            }
        }

        private static TMP_FontAsset EnsureTmpFontAsset(string sourceFontPath, string tmpFontAssetPath)
        {
            TMP_FontAsset existingFontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(tmpFontAssetPath);
            if (IsValidTmpFontAsset(existingFontAsset))
            {
                return existingFontAsset;
            }

            if (existingFontAsset != null)
            {
                AssetDatabase.DeleteAsset(tmpFontAssetPath);
                AssetDatabase.Refresh();
            }

            Font sourceFont = AssetDatabase.LoadAssetAtPath<Font>(sourceFontPath);
            if (sourceFont == null)
            {
                return TMP_Settings.defaultFontAsset;
            }

            TMP_FontAsset tmpFontAsset = TMP_FontAsset.CreateFontAsset(sourceFont);
            tmpFontAsset.atlasPopulationMode = AtlasPopulationMode.Dynamic;
            AssetDatabase.CreateAsset(tmpFontAsset, tmpFontAssetPath);
            AddFontSubAssets(tmpFontAsset);
            AssetDatabase.ImportAsset(tmpFontAssetPath, ImportAssetOptions.ForceUpdate);
            AssetDatabase.SaveAssets();
            return AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(tmpFontAssetPath);
        }

        private static bool IsValidTmpFontAsset(TMP_FontAsset fontAsset)
        {
            return fontAsset != null
                && fontAsset.material != null
                && fontAsset.atlasTextures != null
                && fontAsset.atlasTextures.Length > 0
                && fontAsset.atlasTextures[0] != null;
        }

        private static void AddFontSubAssets(TMP_FontAsset fontAsset)
        {
            if (fontAsset == null)
            {
                return;
            }

            if (fontAsset.material != null && !AssetDatabase.Contains(fontAsset.material))
            {
                AssetDatabase.AddObjectToAsset(fontAsset.material, fontAsset);
            }

            Texture2D[] atlasTextures = fontAsset.atlasTextures;
            if (atlasTextures == null)
            {
                return;
            }

            for (int i = 0; i < atlasTextures.Length; i++)
            {
                Texture2D atlasTexture = atlasTextures[i];
                if (atlasTexture != null && !AssetDatabase.Contains(atlasTexture))
                {
                    AssetDatabase.AddObjectToAsset(atlasTexture, fontAsset);
                }
            }
        }

        private static void SetScenePanelDefaults(params GameObject[] panels)
        {
            for (int i = 0; i < panels.Length; i++)
            {
                if (panels[i] != null)
                {
                    panels[i].SetActive(i == 0);
                }
            }
        }

        private static void SetPortraitLibrary(PortraitLibrary portraitLibrary, Sprite femCharacter1, Sprite femCharacter2, Sprite mascCharacter1, Sprite mascCharacter2)
        {
            SerializedObject serializedObject = new SerializedObject(portraitLibrary);
            SerializedProperty portraits = serializedObject.FindProperty("portraits");
            portraits.arraySize = 4;

            SetPortraitEntry(portraits.GetArrayElementAtIndex(0), "FemCharacter1", femCharacter1);
            SetPortraitEntry(portraits.GetArrayElementAtIndex(1), "FemCharacter2", femCharacter2);
            SetPortraitEntry(portraits.GetArrayElementAtIndex(2), "MascCharacter1", mascCharacter1);
            SetPortraitEntry(portraits.GetArrayElementAtIndex(3), "MascCharacter2", mascCharacter2);

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetPortraitEntry(SerializedProperty element, string id, Sprite sprite)
        {
            element.FindPropertyRelative("id").stringValue = id;
            element.FindPropertyRelative("sprite").objectReferenceValue = sprite;
        }

        private static SuspectChoiceButtonUI CreateSuspectChoiceButton(Transform parent, string name, float anchorX, float anchorY, SuspectChoiceButtonUI.ChoiceMode choiceMode, int suspectIndex)
        {
            GameObject buttonObject = CreatePanel(name, parent, new Color(0.11f, 0.11f, 0.11f, 0.95f));
            SetAnchoredRect(buttonObject.GetComponent<RectTransform>(), new Vector2(anchorX, anchorY), new Vector2(260f, 190f));
            Image background = buttonObject.GetComponent<Image>();
            background.raycastTarget = true;
            Button button = buttonObject.AddComponent<Button>();
            button.targetGraphic = background;

            SuspectChoiceButtonUI choiceButtonUI = buttonObject.AddComponent<SuspectChoiceButtonUI>();
            SetSerializedInt(choiceButtonUI, "suspectIndex", suspectIndex);
            SetSerializedEnum(choiceButtonUI, "choiceMode", (int)choiceMode);
            SetSerializedReference(choiceButtonUI, "button", button);

            Image portrait = CreateImage("Portrait", buttonObject.transform, null, new Vector2(0.5f, 0.63f), new Vector2(145f, 145f), true);
            Image selectionFrame = CreateImage("SelectionFrame", buttonObject.transform, null, new Vector2(0.5f, 0.63f), new Vector2(154f, 154f), false, new Color(0.28f, 0.28f, 0.28f, 1f));
            selectionFrame.transform.SetAsFirstSibling();
            TMP_Text label = CreateText("Label", buttonObject.transform, "Suspect", 22, TextAlignmentOptions.Center, new Vector2(0.5f, 0.16f), new Vector2(210f, 42f));

            SetSerializedReference(choiceButtonUI, "portraitImage", portrait);
            SetSerializedReference(choiceButtonUI, "selectionFrame", selectionFrame);
            SetSerializedReference(choiceButtonUI, "labelText", label);

            AddButtonListener(button, choiceButtonUI, nameof(SuspectChoiceButtonUI.Press));
            AddClickSound(button);
            return choiceButtonUI;
        }

        private static QuestionChoiceButtonUI CreateQuestionButton(Transform parent, string name, Vector2 anchor, Vector2 size, int questionIndex)
        {
            GameObject buttonObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            buttonObject.transform.SetParent(parent, false);
            RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = anchor;
            rectTransform.anchorMax = anchor;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = size;

            Image image = buttonObject.GetComponent<Image>();
            image.color = new Color(0.18f, 0.18f, 0.18f, 0.96f);
            Button button = buttonObject.GetComponent<Button>();
            button.targetGraphic = image;

            QuestionChoiceButtonUI questionButtonUI = buttonObject.AddComponent<QuestionChoiceButtonUI>();
            SetSerializedInt(questionButtonUI, "questionIndex", questionIndex);
            SetSerializedReference(questionButtonUI, "button", button);
            SetSerializedReference(questionButtonUI, "backgroundImage", image);

            TMP_Text label = CreateText("Label", buttonObject.transform, "Question", 20, TextAlignmentOptions.Center, new Vector2(0.5f, 0.5f), size - new Vector2(24f, 20f), new Color(0.97f, 0.91f, 0.74f));
            label.raycastTarget = false;
            SetSerializedReference(questionButtonUI, "labelText", label);

            AddButtonListener(button, questionButtonUI, nameof(QuestionChoiceButtonUI.Press));
            AddClickSound(button);
            return questionButtonUI;
        }

        private static GameObject CreatePanel(string name, Transform parent, Color color)
        {
            GameObject panel = new GameObject(name, typeof(RectTransform), typeof(Image));
            panel.transform.SetParent(parent, false);
            RectTransform rectTransform = panel.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            panel.GetComponent<Image>().color = color;
            return panel;
        }

        private static void ApplyBoardPanelBackdrop(GameObject panel, Color tint, Vector2? insetSize)
        {
            if (panel == null || panelBackdropSprite == null)
            {
                return;
            }

            Image panelImage = panel.GetComponent<Image>();
            if (panelImage != null)
            {
                panelImage.color = Color.clear;
                panelImage.sprite = null;
            }

            GameObject backdropObject = new GameObject("PanelBackdrop", typeof(RectTransform), typeof(Image));
            backdropObject.transform.SetParent(panel.transform, false);
            backdropObject.transform.SetAsFirstSibling();

            RectTransform backdropRect = backdropObject.GetComponent<RectTransform>();
            if (insetSize.HasValue)
            {
                backdropRect.anchorMin = new Vector2(0.5f, 0.5f);
                backdropRect.anchorMax = new Vector2(0.5f, 0.5f);
                backdropRect.anchoredPosition = Vector2.zero;
                backdropRect.sizeDelta = insetSize.Value;
            }
            else
            {
                backdropRect.anchorMin = Vector2.zero;
                backdropRect.anchorMax = Vector2.one;
                backdropRect.offsetMin = Vector2.zero;
                backdropRect.offsetMax = Vector2.zero;
            }

            Image backdropImage = backdropObject.GetComponent<Image>();
            backdropImage.sprite = panelBackdropSprite;
            backdropImage.type = Image.Type.Simple;
            backdropImage.preserveAspect = false;
            backdropImage.color = tint;
            backdropImage.raycastTarget = false;
        }

        private static TMP_Text CreateHeadingText(string name, Transform parent, string text, float fontSize, TextAlignmentOptions alignment, Vector2 anchor, Vector2 size, Color? color = null)
        {
            return CreateStyledText(name, parent, text, fontSize, alignment, anchor, size, headingFontAsset, color);
        }

        private static TMP_Text CreateText(string name, Transform parent, string text, float fontSize, TextAlignmentOptions alignment, Vector2 anchor, Vector2 size, Color? color = null)
        {
            return CreateStyledText(name, parent, text, fontSize * BodyFontScale, alignment, anchor, size, bodyFontAsset, color);
        }

        private static TMP_Text CreateStyledText(string name, Transform parent, string text, float fontSize, TextAlignmentOptions alignment, Vector2 anchor, Vector2 size, TMP_FontAsset fontAsset, Color? color = null)
        {
            GameObject textObject = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
            textObject.transform.SetParent(parent, false);
            RectTransform rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = anchor;
            rectTransform.anchorMax = anchor;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = size;

            TextMeshProUGUI textComponent = textObject.GetComponent<TextMeshProUGUI>();
            textComponent.text = text;
            textComponent.font = fontAsset != null ? fontAsset : TMP_Settings.defaultFontAsset;
            textComponent.fontSize = fontSize;
            textComponent.enableAutoSizing = true;
            textComponent.fontSizeMax = fontSize;
            textComponent.fontSizeMin = Mathf.Max(16f, fontSize * 0.55f);
            textComponent.alignment = alignment;
            textComponent.color = color ?? Color.white;
            textComponent.textWrappingMode = TextWrappingModes.Normal;
            textComponent.overflowMode = TextOverflowModes.Overflow;
            return textComponent;
        }

        private static Image CreateImage(string name, Transform parent, Sprite sprite, Vector2 anchor, Vector2 size, bool preserveAspect, Color? color = null)
        {
            GameObject imageObject = new GameObject(name, typeof(RectTransform), typeof(Image));
            imageObject.transform.SetParent(parent, false);
            RectTransform rectTransform = imageObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = anchor;
            rectTransform.anchorMax = anchor;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = size;

            Image image = imageObject.GetComponent<Image>();
            image.sprite = sprite;
            image.color = color ?? Color.white;
            image.preserveAspect = preserveAspect;
            return image;
        }

        private static Button CreateButton(string name, Transform parent, string label, Vector2 anchor, Vector2 size)
        {
            GameObject buttonObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            buttonObject.transform.SetParent(parent, false);
            RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = anchor;
            rectTransform.anchorMax = anchor;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = size;

            Image image = buttonObject.GetComponent<Image>();
            image.color = new Color(0.19f, 0.19f, 0.18f, 0.95f);

            Button button = buttonObject.GetComponent<Button>();
            button.targetGraphic = image;

            TMP_Text labelText = CreateText("Label", buttonObject.transform, label, 28, TextAlignmentOptions.Center, new Vector2(0.5f, 0.5f), size - new Vector2(20f, 14f), new Color(0.97f, 0.91f, 0.74f));
            labelText.raycastTarget = false;
            AddClickSound(button);
            return button;
        }

        private static void AddClickSound(Button button)
        {
            if (button == null || clickSoundPlayer == null)
            {
                return;
            }

            UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, clickSoundPlayer.PlayClick);
        }

        private static void StyleAccentButton(Button button)
        {
            if (button == null)
            {
                return;
            }

            Image image = button.GetComponent<Image>();
            if (image != null)
            {
                image.color = new Color(0.97f, 0.91f, 0.74f, 0.95f);
            }

            Transform labelTransform = button.transform.Find("Label");
            if (labelTransform != null)
            {
                TMP_Text labelText = labelTransform.GetComponent<TMP_Text>();
                if (labelText != null)
                {
                    labelText.color = new Color(0.19f, 0.19f, 0.18f, 0.95f);
                }
            }
        }

        private static ScrollRect CreateConversationScrollView(string name, Transform parent, Vector2 anchor, Vector2 size, out RectTransform contentRect, out TMP_Text conversationText)
        {
            GameObject root = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(ScrollRect));
            root.transform.SetParent(parent, false);
            RectTransform rootRect = root.GetComponent<RectTransform>();
            rootRect.anchorMin = anchor;
            rootRect.anchorMax = anchor;
            rootRect.anchoredPosition = Vector2.zero;
            rootRect.sizeDelta = size;

            Image rootImage = root.GetComponent<Image>();
            rootImage.color = new Color(0.12f, 0.11f, 0.1f, 0.95f);

            GameObject viewport = new GameObject("Viewport", typeof(RectTransform), typeof(Image), typeof(Mask));
            viewport.transform.SetParent(root.transform, false);
            RectTransform viewportRect = viewport.GetComponent<RectTransform>();
            viewportRect.anchorMin = Vector2.zero;
            viewportRect.anchorMax = Vector2.one;
            viewportRect.offsetMin = new Vector2(16f, 16f);
            viewportRect.offsetMax = new Vector2(-16f, -16f);
            viewport.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
            viewport.GetComponent<Mask>().showMaskGraphic = false;

            GameObject content = new GameObject("Content", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter));
            content.transform.SetParent(viewport.transform, false);
            contentRect = content.GetComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0f, 1f);
            contentRect.anchorMax = new Vector2(1f, 1f);
            contentRect.pivot = new Vector2(0.5f, 1f);
            contentRect.anchoredPosition = Vector2.zero;
            contentRect.sizeDelta = new Vector2(0f, 360f);
            VerticalLayoutGroup layoutGroup = content.GetComponent<VerticalLayoutGroup>();
            layoutGroup.padding = new RectOffset(0, 0, 0, 0);
            layoutGroup.spacing = 0f;
            layoutGroup.childAlignment = TextAnchor.UpperLeft;
            layoutGroup.childControlHeight = true;
            layoutGroup.childControlWidth = true;
            layoutGroup.childForceExpandHeight = false;
            layoutGroup.childForceExpandWidth = false;
            ContentSizeFitter contentSizeFitter = content.GetComponent<ContentSizeFitter>();
            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            conversationText = CreateText("ConversationText", content.transform, "Conversation", 22, TextAlignmentOptions.TopLeft, new Vector2(0.5f, 1f), new Vector2(size.x - 70f, 360f));
            RectTransform textRect = conversationText.rectTransform;
            textRect.anchorMin = new Vector2(0f, 1f);
            textRect.anchorMax = new Vector2(1f, 1f);
            textRect.pivot = new Vector2(0.5f, 1f);
            conversationText.enableAutoSizing = false;
            conversationText.overflowMode = TextOverflowModes.Overflow;
            LayoutElement layoutElement = conversationText.gameObject.AddComponent<LayoutElement>();
            layoutElement.minHeight = 360f;
            layoutElement.preferredWidth = size.x - 70f;

            ScrollRect scrollRect = root.GetComponent<ScrollRect>();
            scrollRect.viewport = viewportRect;
            scrollRect.content = contentRect;
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.scrollSensitivity = 28f;
            return scrollRect;
        }

        private static void AddButtonListener(Button button, Object target, string methodName)
        {
            switch (target)
            {
                case GameManager gameManager when methodName == nameof(GameManager.StartNewCase):
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, gameManager.StartNewCase);
                    break;
                case GameManager gameManager when methodName == nameof(GameManager.StartTimedCase):
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, gameManager.StartTimedCase);
                    break;
                case GameManager gameManager when methodName == nameof(GameManager.QuitGame):
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, gameManager.QuitGame);
                    break;
                case GameManager gameManager when methodName == nameof(GameManager.OpenCrimeScreen):
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, gameManager.OpenCrimeScreen);
                    break;
                case GameManager gameManager when methodName == nameof(GameManager.OpenSuspectsScreen):
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, gameManager.OpenSuspectsScreen);
                    break;
                case GameManager gameManager when methodName == nameof(GameManager.OpenInterrogationScreen):
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, gameManager.OpenInterrogationScreen);
                    break;
                case GameManager gameManager when methodName == nameof(GameManager.OpenAccusation):
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, gameManager.OpenAccusation);
                    break;
                case GameManager gameManager when methodName == nameof(GameManager.ReturnToCrimeBoard):
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, gameManager.ReturnToCrimeBoard);
                    break;
                case GameManager gameManager when methodName == nameof(GameManager.ReturnToMainMenu):
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, gameManager.ReturnToMainMenu);
                    break;
                case SuspectsPanelUI suspectsPanelUI when methodName == nameof(SuspectsPanelUI.ShowPreviousSuspect):
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, suspectsPanelUI.ShowPreviousSuspect);
                    break;
                case SuspectsPanelUI suspectsPanelUI when methodName == nameof(SuspectsPanelUI.ShowNextSuspect):
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, suspectsPanelUI.ShowNextSuspect);
                    break;
                case SuspectChoiceButtonUI suspectChoiceButtonUI when methodName == nameof(SuspectChoiceButtonUI.Press):
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, suspectChoiceButtonUI.Press);
                    break;
                case QuestionChoiceButtonUI questionChoiceButtonUI when methodName == nameof(QuestionChoiceButtonUI.Press):
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, questionChoiceButtonUI.Press);
                    break;
                case AccusationPanelUI accusationPanelUI when methodName == nameof(AccusationPanelUI.ConfirmAccusationPressed):
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, accusationPanelUI.ConfirmAccusationPressed);
                    break;
                case AccusationPanelUI accusationPanelUI when methodName == nameof(AccusationPanelUI.BackPressed):
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, accusationPanelUI.BackPressed);
                    break;
                case ResultPanelUI resultPanelUI when methodName == nameof(ResultPanelUI.StartNewCasePressed):
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, resultPanelUI.StartNewCasePressed);
                    break;
            }
        }

        private static void SetSerializedReference(Object target, string propertyName, Object value)
        {
            SerializedObject serializedObject = new SerializedObject(target);
            serializedObject.FindProperty(propertyName).objectReferenceValue = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetSerializedInt(Object target, string propertyName, int value)
        {
            SerializedObject serializedObject = new SerializedObject(target);
            serializedObject.FindProperty(propertyName).intValue = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetSerializedEnum(Object target, string propertyName, int value)
        {
            SerializedObject serializedObject = new SerializedObject(target);
            serializedObject.FindProperty(propertyName).enumValueIndex = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetSerializedArray<T>(Object target, string propertyName, T[] values) where T : Object
        {
            SerializedObject serializedObject = new SerializedObject(target);
            SerializedProperty property = serializedObject.FindProperty(propertyName);
            property.arraySize = values.Length;
            for (int i = 0; i < values.Length; i++)
            {
                property.GetArrayElementAtIndex(i).objectReferenceValue = values[i];
            }

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetAnchoredRect(RectTransform rectTransform, Vector2 anchor, Vector2 size)
        {
            rectTransform.anchorMin = anchor;
            rectTransform.anchorMax = anchor;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = size;
        }

        private static void SetRectSize(RectTransform rectTransform, Vector2 size)
        {
            rectTransform.sizeDelta = size;
        }
    }
}
