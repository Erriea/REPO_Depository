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
        private const string Suspect1Path = "Assets/Art/Suspects/FemCharacter1.png";
        private const string Suspect2Path = "Assets/Art/Suspects/FemCharacter2.png";
        private const string VictimPath = "Assets/Art/Suspects/MascCharacter1.png";
        private const string Suspect3Path = "Assets/Art/Suspects/MascCharacter2.png";

        [MenuItem("Tools/CaseFile/Build Assignment Scene")]
        public static void BuildAssignmentScene()
        {
            AssetDatabase.Refresh();
            ConfigureSpriteImport(BackgroundPath);
            ConfigureSpriteImport(Suspect1Path);
            ConfigureSpriteImport(Suspect2Path);
            ConfigureSpriteImport(VictimPath);
            ConfigureSpriteImport(Suspect3Path);
            AssetDatabase.Refresh();

            Sprite backgroundSprite = AssetDatabase.LoadAssetAtPath<Sprite>(BackgroundPath);
            Sprite femCharacter1 = AssetDatabase.LoadAssetAtPath<Sprite>(Suspect1Path);
            Sprite femCharacter2 = AssetDatabase.LoadAssetAtPath<Sprite>(Suspect2Path);
            Sprite mascCharacter1 = AssetDatabase.LoadAssetAtPath<Sprite>(VictimPath);
            Sprite mascCharacter2 = AssetDatabase.LoadAssetAtPath<Sprite>(Suspect3Path);

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

            GameObject backgroundObject = CreatePanel("Background", canvasObject.transform, new Color(0f, 0f, 0f, 0f));
            Image backgroundImage = backgroundObject.GetComponent<Image>();
            backgroundImage.sprite = backgroundSprite;
            backgroundImage.preserveAspect = false;
            backgroundImage.raycastTarget = false;

            GameObject overlayObject = CreatePanel("Overlay", canvasObject.transform, new Color(0.05f, 0.05f, 0.06f, 0.52f));
            overlayObject.GetComponent<Image>().raycastTarget = false;

            GameObject mainMenuPanel = CreatePanel("MainMenuPanel", canvasObject.transform, new Color(0f, 0f, 0f, 0f));
            CreateText("MenuTitle", mainMenuPanel.transform, "CaseFile: Local Suspect", 88, TextAlignmentOptions.Center, new Vector2(0.5f, 0.84f), new Vector2(1100f, 130f));
            CreateText("MenuSubtitle", mainMenuPanel.transform, "Investigate a local case, question each suspect, and make the final accusation.", 34, TextAlignmentOptions.Center, new Vector2(0.5f, 0.73f), new Vector2(1200f, 100f), new Color(0.92f, 0.84f, 0.68f));
            Button newCaseButton = CreateButton("NewCaseButton", mainMenuPanel.transform, "New Case", new Vector2(0.5f, 0.52f), new Vector2(320f, 80f));
            Button quitButton = CreateButton("QuitButton", mainMenuPanel.transform, "Quit", new Vector2(0.5f, 0.40f), new Vector2(320f, 80f));

            GameObject briefingPanel = CreatePanel("CaseBriefingPanel", canvasObject.transform, new Color(0.09f, 0.08f, 0.06f, 0.72f));
            CaseBriefingPanelUI briefingUI = briefingPanel.AddComponent<CaseBriefingPanelUI>();
            TMP_Text briefingTitle = CreateText("Heading", briefingPanel.transform, "Case Briefing", 62, TextAlignmentOptions.Center, new Vector2(0.5f, 0.95f), new Vector2(1100f, 86f));
            TMP_Text caseTitle = CreateText("CaseTitle", briefingPanel.transform, "Case Title", 50, TextAlignmentOptions.Center, new Vector2(0.5f, 0.87f), new Vector2(1400f, 80f), new Color(0.97f, 0.91f, 0.74f));
            TMP_Text crimeText = CreateText("CrimeText", briefingPanel.transform, "Crime", 28, TextAlignmentOptions.TopLeft, new Vector2(0.56f, 0.74f), new Vector2(1080f, 150f));
            TMP_Text victimText = CreateText("VictimText", briefingPanel.transform, "Victim", 26, TextAlignmentOptions.TopLeft, new Vector2(0.56f, 0.63f), new Vector2(1080f, 54f));
            TMP_Text locationText = CreateText("LocationText", briefingPanel.transform, "Location", 26, TextAlignmentOptions.TopLeft, new Vector2(0.56f, 0.58f), new Vector2(1080f, 54f));
            Image victimPortrait = CreateImage("VictimPortrait", briefingPanel.transform, null, new Vector2(0.12f, 0.66f), new Vector2(240f, 310f), true);
            CreateText("VictimLabel", briefingPanel.transform, "Victim", 28, TextAlignmentOptions.Center, new Vector2(0.12f, 0.47f), new Vector2(220f, 44f), new Color(0.97f, 0.91f, 0.74f));

            SuspectCardUI[] suspectCards = new SuspectCardUI[3];
            for (int i = 0; i < suspectCards.Length; i++)
            {
                float x = 0.20f + (i * 0.30f);
                GameObject card = CreatePanel($"SuspectCard{i + 1}", briefingPanel.transform, new Color(0.12f, 0.12f, 0.1f, 0.9f));
                SetAnchoredRect(card.GetComponent<RectTransform>(), new Vector2(x, 0.25f), new Vector2(360f, 300f));
                SuspectCardUI cardUI = card.AddComponent<SuspectCardUI>();
                Image portrait = CreateImage("Portrait", card.transform, null, new Vector2(0.5f, 0.79f), new Vector2(120f, 120f), true);
                TMP_Text nameText = CreateText("NameText", card.transform, "Suspect", 28, TextAlignmentOptions.Center, new Vector2(0.5f, 0.56f), new Vector2(320f, 38f), new Color(0.97f, 0.91f, 0.74f));
                TMP_Text roleText = CreateText("RoleText", card.transform, "Role", 18, TextAlignmentOptions.Center, new Vector2(0.5f, 0.48f), new Vector2(320f, 30f));
                TMP_Text motiveText = CreateText("MotiveText", card.transform, "Motive", 16, TextAlignmentOptions.TopLeft, new Vector2(0.5f, 0.26f), new Vector2(320f, 58f));
                TMP_Text alibiText = CreateText("AlibiText", card.transform, "Alibi", 16, TextAlignmentOptions.TopLeft, new Vector2(0.5f, 0.12f), new Vector2(320f, 58f));
                SetSerializedReference(cardUI, "portraitImage", portrait);
                SetSerializedReference(cardUI, "suspectNameText", nameText);
                SetSerializedReference(cardUI, "suspectRoleText", roleText);
                SetSerializedReference(cardUI, "suspectMotiveText", motiveText);
                SetSerializedReference(cardUI, "suspectAlibiText", alibiText);
                suspectCards[i] = cardUI;
            }

            Button beginInterrogationButton = CreateButton("BeginInterrogationButton", briefingPanel.transform, "Begin Interrogation", new Vector2(0.80f, 0.07f), new Vector2(320f, 66f));
            Button backToMenuFromBriefing = CreateButton("BackToMenuButton", briefingPanel.transform, "Main Menu", new Vector2(0.20f, 0.07f), new Vector2(230f, 66f));

            GameObject interrogationPanel = CreatePanel("InterrogationPanel", canvasObject.transform, new Color(0.08f, 0.08f, 0.09f, 0.76f));
            InterrogationPanelUI interrogationUI = interrogationPanel.AddComponent<InterrogationPanelUI>();
            CreateText("InterrogationHeading", interrogationPanel.transform, "Interrogation Room", 60, TextAlignmentOptions.Center, new Vector2(0.5f, 0.95f), new Vector2(1000f, 82f));

            SuspectChoiceButtonUI[] interrogationButtons = new SuspectChoiceButtonUI[3];
            for (int i = 0; i < interrogationButtons.Length; i++)
            {
                float x = 0.22f + (i * 0.28f);
                interrogationButtons[i] = CreateSuspectChoiceButton(interrogationPanel.transform, $"InterrogationSuspectButton{i + 1}", x, 0.82f, SuspectChoiceButtonUI.ChoiceMode.Interrogation, i);
            }

            Image interrogationPortrait = CreateImage("SelectedPortrait", interrogationPanel.transform, null, new Vector2(0.17f, 0.51f), new Vector2(300f, 360f), true);
            TMP_Text interrogationName = CreateText("SelectedSuspectName", interrogationPanel.transform, "Selected Suspect", 40, TextAlignmentOptions.Center, new Vector2(0.17f, 0.26f), new Vector2(360f, 60f), new Color(0.97f, 0.91f, 0.74f));
            TMP_Text interrogationRole = CreateText("SelectedSuspectRole", interrogationPanel.transform, "Role", 24, TextAlignmentOptions.Center, new Vector2(0.17f, 0.21f), new Vector2(360f, 40f));
            TMP_Text questionsRemaining = CreateText("QuestionsRemaining", interrogationPanel.transform, "Questions Remaining: 6", 26, TextAlignmentOptions.Center, new Vector2(0.63f, 0.74f), new Vector2(540f, 44f), new Color(0.97f, 0.91f, 0.74f));

            ScrollRect conversationScrollRect = CreateScrollView("ConversationScrollView", interrogationPanel.transform, new Vector2(0.63f, 0.49f), new Vector2(900f, 410f), out RectTransform conversationContentRect, out TMP_Text conversationText);
            TMP_InputField questionInput = CreateInputField("QuestionInput", interrogationPanel.transform, new Vector2(0.59f, 0.12f), new Vector2(820f, 78f));
            TMP_Text hintText = CreateText("HintText", interrogationPanel.transform, "Ask about alibis, motives, the study, the victim, or the missing ledger.", 20, TextAlignmentOptions.Center, new Vector2(0.60f, 0.19f), new Vector2(860f, 40f), new Color(0.85f, 0.82f, 0.7f));
            Button submitQuestionButton = CreateButton("SubmitQuestionButton", interrogationPanel.transform, "Submit Question", new Vector2(0.88f, 0.12f), new Vector2(250f, 78f));
            Button accuseButton = CreateButton("AccuseButton", interrogationPanel.transform, "Make Accusation", new Vector2(0.17f, 0.11f), new Vector2(300f, 78f));

            GameObject accusationPanel = CreatePanel("AccusationPanel", canvasObject.transform, new Color(0.08f, 0.07f, 0.07f, 0.82f));
            AccusationPanelUI accusationUI = accusationPanel.AddComponent<AccusationPanelUI>();
            TMP_Text accusationPrompt = CreateText("PromptText", accusationPanel.transform, "Choose the suspect you believe stole the ledger.", 48, TextAlignmentOptions.Center, new Vector2(0.5f, 0.90f), new Vector2(1200f, 84f));
            SuspectChoiceButtonUI[] accusationButtons = new SuspectChoiceButtonUI[3];
            for (int i = 0; i < accusationButtons.Length; i++)
            {
                float x = 0.2f + (i * 0.3f);
                accusationButtons[i] = CreateSuspectChoiceButton(accusationPanel.transform, $"AccusationSuspectButton{i + 1}", x, 0.52f, SuspectChoiceButtonUI.ChoiceMode.Accusation, i);
                SetRectSize(accusationButtons[i].GetComponent<RectTransform>(), new Vector2(320f, 430f));
            }

            Button confirmAccusationButton = CreateButton("ConfirmAccusationButton", accusationPanel.transform, "Confirm Accusation", new Vector2(0.68f, 0.12f), new Vector2(330f, 76f));
            Button backToInterrogationButton = CreateButton("BackButton", accusationPanel.transform, "Back", new Vector2(0.32f, 0.12f), new Vector2(240f, 76f));

            GameObject resultPanel = CreatePanel("ResultPanel", canvasObject.transform, new Color(0.06f, 0.06f, 0.06f, 0.84f));
            ResultPanelUI resultUI = resultPanel.AddComponent<ResultPanelUI>();
            TMP_Text resultHeader = CreateText("ResultHeader", resultPanel.transform, "Result", 62, TextAlignmentOptions.Center, new Vector2(0.5f, 0.93f), new Vector2(1000f, 82f), new Color(0.97f, 0.91f, 0.74f));
            Image guiltyPortrait = CreateImage("GuiltyPortrait", resultPanel.transform, null, new Vector2(0.18f, 0.58f), new Vector2(320f, 400f), true);
            TMP_Text accusedText = CreateText("AccusedText", resultPanel.transform, "You accused:", 28, TextAlignmentOptions.Left, new Vector2(0.60f, 0.77f), new Vector2(820f, 50f));
            TMP_Text guiltyText = CreateText("GuiltyText", resultPanel.transform, "Actual culprit:", 28, TextAlignmentOptions.Left, new Vector2(0.60f, 0.70f), new Vector2(820f, 50f));
            TMP_Text clueText = CreateText("ClueText", resultPanel.transform, "Key clue:", 24, TextAlignmentOptions.TopLeft, new Vector2(0.60f, 0.56f), new Vector2(820f, 120f), new Color(0.97f, 0.91f, 0.74f));
            TMP_Text explanationText = CreateText("ExplanationText", resultPanel.transform, "Explanation", 22, TextAlignmentOptions.TopLeft, new Vector2(0.60f, 0.34f), new Vector2(820f, 230f));
            Button restartButton = CreateButton("RestartButton", resultPanel.transform, "New Case", new Vector2(0.5f, 0.10f), new Vector2(280f, 76f));

            GameObject gameManagerObject = new GameObject("GameManager");
            GameManager gameManager = gameManagerObject.AddComponent<GameManager>();
            GameObject uiManagerObject = new GameObject("UIManager");
            UIManager uiManager = uiManagerObject.AddComponent<UIManager>();
            GameObject portraitLibraryObject = new GameObject("PortraitLibrary");
            PortraitLibrary portraitLibrary = portraitLibraryObject.AddComponent<PortraitLibrary>();

            SetPortraitLibrary(portraitLibrary, femCharacter1, femCharacter2, mascCharacter1, mascCharacter2);

            SetSerializedReference(uiManager, "mainMenuPanel", mainMenuPanel);
            SetSerializedReference(uiManager, "caseBriefingPanel", briefingPanel);
            SetSerializedReference(uiManager, "interrogationPanel", interrogationPanel);
            SetSerializedReference(uiManager, "accusationPanel", accusationPanel);
            SetSerializedReference(uiManager, "resultPanel", resultPanel);
            SetSerializedReference(uiManager, "portraitLibrary", portraitLibrary);
            SetSerializedReference(uiManager, "caseBriefingPanelUI", briefingUI);
            SetSerializedReference(uiManager, "interrogationPanelUI", interrogationUI);
            SetSerializedReference(uiManager, "accusationPanelUI", accusationUI);
            SetSerializedReference(uiManager, "resultPanelUI", resultUI);
            SetSerializedReference(gameManager, "uiManager", uiManager);

            SetSerializedReference(briefingUI, "caseTitleText", caseTitle);
            SetSerializedReference(briefingUI, "crimeText", crimeText);
            SetSerializedReference(briefingUI, "victimText", victimText);
            SetSerializedReference(briefingUI, "locationText", locationText);
            SetSerializedReference(briefingUI, "victimPortraitImage", victimPortrait);
            SetSerializedArray(briefingUI, "suspectCards", suspectCards);

            SetSerializedReference(interrogationUI, "selectedSuspectNameText", interrogationName);
            SetSerializedReference(interrogationUI, "selectedSuspectRoleText", interrogationRole);
            SetSerializedReference(interrogationUI, "questionsRemainingText", questionsRemaining);
            SetSerializedReference(interrogationUI, "conversationHistoryText", conversationText);
            SetSerializedReference(interrogationUI, "hintText", hintText);
            SetSerializedReference(interrogationUI, "selectedSuspectPortraitImage", interrogationPortrait);
            SetSerializedReference(interrogationUI, "conversationScrollRect", conversationScrollRect);
            SetSerializedReference(interrogationUI, "conversationContentRect", conversationContentRect);
            SetSerializedReference(interrogationUI, "questionInputField", questionInput);
            SetSerializedReference(interrogationUI, "submitQuestionButton", submitQuestionButton);
            SetSerializedReference(interrogationUI, "accuseButton", accuseButton);
            SetSerializedArray(interrogationUI, "suspectButtons", interrogationButtons);

            SetSerializedReference(accusationUI, "promptText", accusationPrompt);
            SetSerializedArray(accusationUI, "suspectButtons", accusationButtons);

            SetSerializedReference(resultUI, "resultHeaderText", resultHeader);
            SetSerializedReference(resultUI, "accusedSuspectText", accusedText);
            SetSerializedReference(resultUI, "guiltySuspectText", guiltyText);
            SetSerializedReference(resultUI, "keyClueText", clueText);
            SetSerializedReference(resultUI, "explanationText", explanationText);
            SetSerializedReference(resultUI, "guiltySuspectPortraitImage", guiltyPortrait);

            SetSerializedReference(interrogationUI, "gameManager", gameManager);
            SetSerializedReference(accusationUI, "gameManager", gameManager);
            SetSerializedReference(resultUI, "gameManager", gameManager);

            for (int i = 0; i < interrogationButtons.Length; i++)
            {
                SetSerializedReference(interrogationButtons[i], "gameManager", gameManager);
            }

            for (int i = 0; i < accusationButtons.Length; i++)
            {
                SetSerializedReference(accusationButtons[i], "gameManager", gameManager);
            }

            AddButtonListener(newCaseButton, gameManager, nameof(GameManager.StartNewCase));
            AddButtonListener(quitButton, gameManager, nameof(GameManager.QuitGame));
            AddButtonListener(beginInterrogationButton, gameManager, nameof(GameManager.BeginInterrogation));
            AddButtonListener(backToMenuFromBriefing, gameManager, nameof(GameManager.ReturnToMainMenu));
            AddButtonListener(submitQuestionButton, interrogationUI, nameof(InterrogationPanelUI.SubmitQuestionPressed));
            AddButtonListener(accuseButton, interrogationUI, nameof(InterrogationPanelUI.OpenAccusationPressed));
            AddButtonListener(confirmAccusationButton, accusationUI, nameof(AccusationPanelUI.ConfirmAccusationPressed));
            AddButtonListener(backToInterrogationButton, accusationUI, nameof(AccusationPanelUI.BackPressed));
            AddButtonListener(restartButton, resultUI, nameof(ResultPanelUI.StartNewCasePressed));

            SetScenePanelDefaults(mainMenuPanel, briefingPanel, interrogationPanel, accusationPanel, resultPanel);

            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(ScenePath, true)
            };
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("CaseFile Builder", "MainScene has been built and saved. Open Assets/Scenes/MainScene.unity and press Play.", "OK");
        }

        private static void SetScenePanelDefaults(GameObject mainMenuPanel, GameObject briefingPanel, GameObject interrogationPanel, GameObject accusationPanel, GameObject resultPanel)
        {
            mainMenuPanel.SetActive(true);
            briefingPanel.SetActive(false);
            interrogationPanel.SetActive(false);
            accusationPanel.SetActive(false);
            resultPanel.SetActive(false);
        }

        private static void ConfigureSpriteImport(string path)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null)
            {
                return;
            }

            bool changed = false;
            if (importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                changed = true;
            }

            if (importer.alphaIsTransparency == false)
            {
                importer.alphaIsTransparency = true;
                changed = true;
            }

            if (importer.mipmapEnabled)
            {
                importer.mipmapEnabled = false;
                changed = true;
            }

            if (importer.filterMode != FilterMode.Bilinear)
            {
                importer.filterMode = FilterMode.Bilinear;
                changed = true;
            }

            if (importer.textureCompression != TextureImporterCompression.Uncompressed)
            {
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                changed = true;
            }

            if (importer.maxTextureSize < 4096)
            {
                importer.maxTextureSize = 4096;
                changed = true;
            }

            if (changed)
            {
                importer.SaveAndReimport();
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
            SetAnchoredRect(buttonObject.GetComponent<RectTransform>(), new Vector2(anchorX, anchorY), new Vector2(280f, 180f));
            Image background = buttonObject.GetComponent<Image>();
            background.raycastTarget = true;
            Button button = buttonObject.AddComponent<Button>();
            button.targetGraphic = background;

            SuspectChoiceButtonUI choiceButtonUI = buttonObject.AddComponent<SuspectChoiceButtonUI>();
            SetSerializedInt(choiceButtonUI, "suspectIndex", suspectIndex);
            SetSerializedEnum(choiceButtonUI, "choiceMode", (int)choiceMode);
            SetSerializedReference(choiceButtonUI, "button", button);

            Image portrait = CreateImage("Portrait", buttonObject.transform, null, new Vector2(0.5f, 0.62f), new Vector2(120f, 120f), true);
            Image selectionFrame = CreateImage("SelectionFrame", buttonObject.transform, null, new Vector2(0.5f, 0.62f), new Vector2(128f, 128f), false, new Color(0.28f, 0.28f, 0.28f, 1f));
            selectionFrame.transform.SetAsFirstSibling();
            TMP_Text label = CreateText("Label", buttonObject.transform, "Suspect", 22, TextAlignmentOptions.Center, new Vector2(0.5f, 0.16f), new Vector2(230f, 42f));

            SetSerializedReference(choiceButtonUI, "portraitImage", portrait);
            SetSerializedReference(choiceButtonUI, "selectionFrame", selectionFrame);
            SetSerializedReference(choiceButtonUI, "labelText", label);

            AddButtonListener(button, choiceButtonUI, nameof(SuspectChoiceButtonUI.Press));
            return choiceButtonUI;
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

        private static TMP_Text CreateText(string name, Transform parent, string text, float fontSize, TextAlignmentOptions alignment, Vector2 anchor, Vector2 size, Color? color = null)
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
            textComponent.font = TMP_Settings.defaultFontAsset;
            textComponent.fontSize = fontSize;
            textComponent.enableAutoSizing = true;
            textComponent.fontSizeMax = fontSize;
            textComponent.fontSizeMin = Mathf.Max(16f, fontSize * 0.55f);
            textComponent.alignment = alignment;
            textComponent.color = color ?? Color.white;
            textComponent.enableWordWrapping = true;
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
            return button;
        }

        private static TMP_InputField CreateInputField(string name, Transform parent, Vector2 anchor, Vector2 size)
        {
            GameObject root = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(TMP_InputField));
            root.transform.SetParent(parent, false);
            RectTransform rootRect = root.GetComponent<RectTransform>();
            rootRect.anchorMin = anchor;
            rootRect.anchorMax = anchor;
            rootRect.anchoredPosition = Vector2.zero;
            rootRect.sizeDelta = size;

            Image background = root.GetComponent<Image>();
            background.color = new Color(0.14f, 0.14f, 0.14f, 0.96f);

            GameObject textArea = new GameObject("Text Area", typeof(RectTransform));
            textArea.transform.SetParent(root.transform, false);
            RectTransform textAreaRect = textArea.GetComponent<RectTransform>();
            textAreaRect.anchorMin = Vector2.zero;
            textAreaRect.anchorMax = Vector2.one;
            textAreaRect.offsetMin = new Vector2(20f, 12f);
            textAreaRect.offsetMax = new Vector2(-20f, -12f);

            TMP_Text placeholder = CreateText("Placeholder", textArea.transform, "Type your question here...", 24, TextAlignmentOptions.Left, new Vector2(0.5f, 0.5f), new Vector2(size.x - 40f, size.y - 20f), new Color(0.7f, 0.7f, 0.7f, 0.75f));
            TMP_Text text = CreateText("Text", textArea.transform, string.Empty, 24, TextAlignmentOptions.Left, new Vector2(0.5f, 0.5f), new Vector2(size.x - 40f, size.y - 20f));

            TMP_InputField inputField = root.GetComponent<TMP_InputField>();
            inputField.textViewport = textAreaRect;
            inputField.textComponent = text as TextMeshProUGUI;
            inputField.placeholder = placeholder;
            inputField.lineType = TMP_InputField.LineType.SingleLine;
            inputField.pointSize = 24;
            return inputField;
        }

        private static ScrollRect CreateScrollView(string name, Transform parent, Vector2 anchor, Vector2 size, out RectTransform contentRect, out TMP_Text contentText)
        {
            GameObject root = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Mask), typeof(ScrollRect));
            root.transform.SetParent(parent, false);
            RectTransform rootRect = root.GetComponent<RectTransform>();
            rootRect.anchorMin = anchor;
            rootRect.anchorMax = anchor;
            rootRect.anchoredPosition = Vector2.zero;
            rootRect.sizeDelta = size;

            Image rootImage = root.GetComponent<Image>();
            rootImage.color = new Color(0.12f, 0.11f, 0.1f, 0.95f);
            root.GetComponent<Mask>().showMaskGraphic = true;

            GameObject viewport = new GameObject("Viewport", typeof(RectTransform), typeof(Image), typeof(Mask));
            viewport.transform.SetParent(root.transform, false);
            RectTransform viewportRect = viewport.GetComponent<RectTransform>();
            viewportRect.anchorMin = Vector2.zero;
            viewportRect.anchorMax = Vector2.one;
            viewportRect.offsetMin = new Vector2(16f, 16f);
            viewportRect.offsetMax = new Vector2(-16f, -16f);
            viewport.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
            viewport.GetComponent<Mask>().showMaskGraphic = false;

            GameObject content = new GameObject("Content", typeof(RectTransform));
            content.transform.SetParent(viewport.transform, false);
            contentRect = content.GetComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0f, 1f);
            contentRect.anchorMax = new Vector2(1f, 1f);
            contentRect.pivot = new Vector2(0.5f, 1f);
            contentRect.anchoredPosition = Vector2.zero;
            contentRect.sizeDelta = new Vector2(0f, 200f);

            contentText = CreateText("ConversationText", content.transform, "Conversation", 22, TextAlignmentOptions.TopLeft, new Vector2(0.5f, 1f), new Vector2(size.x - 70f, 200f));
            RectTransform textRect = contentText.GetComponent<RectTransform>();
            textRect.pivot = new Vector2(0.5f, 1f);
            textRect.anchoredPosition = Vector2.zero;
            contentText.overflowMode = TextOverflowModes.Overflow;

            ScrollRect scrollRect = root.GetComponent<ScrollRect>();
            scrollRect.viewport = viewportRect;
            scrollRect.content = contentRect;
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.scrollSensitivity = 30f;

            return scrollRect;
        }

        private static void AddButtonListener(Button button, Object target, string methodName)
        {
            switch (target)
            {
                case GameManager gameManager when methodName == nameof(GameManager.StartNewCase):
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, gameManager.StartNewCase);
                    break;
                case GameManager gameManager when methodName == nameof(GameManager.QuitGame):
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, gameManager.QuitGame);
                    break;
                case GameManager gameManager when methodName == nameof(GameManager.BeginInterrogation):
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, gameManager.BeginInterrogation);
                    break;
                case GameManager gameManager when methodName == nameof(GameManager.ReturnToMainMenu):
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, gameManager.ReturnToMainMenu);
                    break;
                case SuspectChoiceButtonUI suspectChoiceButtonUI when methodName == nameof(SuspectChoiceButtonUI.Press):
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, suspectChoiceButtonUI.Press);
                    break;
                case InterrogationPanelUI interrogationPanelUI when methodName == nameof(InterrogationPanelUI.SubmitQuestionPressed):
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, interrogationPanelUI.SubmitQuestionPressed);
                    break;
                case InterrogationPanelUI interrogationPanelUI when methodName == nameof(InterrogationPanelUI.OpenAccusationPressed):
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, interrogationPanelUI.OpenAccusationPressed);
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
