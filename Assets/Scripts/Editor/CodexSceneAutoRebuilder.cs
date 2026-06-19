using UnityEditor;
using UnityEditor.SceneManagement;

namespace CaseFileLocalSuspect.Editor
{
    [InitializeOnLoad]
    public static class CodexSceneAutoRebuilder
    {
        private const string SessionKey = "CaseFileLocalSuspect.CodexSceneAutoRebuilder.ResultOutcomeSfx";

        static CodexSceneAutoRebuilder()
        {
            EditorApplication.delayCall += RunOnceAfterReload;
        }

        private static void RunOnceAfterReload()
        {
            EditorApplication.delayCall -= RunOnceAfterReload;

            if (SessionState.GetBool(SessionKey, false))
            {
                return;
            }

            SessionState.SetBool(SessionKey, true);
            CaseFileSceneBuilder.BuildAssignmentScene();
            EditorSceneManager.OpenScene("Assets/Scenes/MainScene.unity");
        }
    }
}
