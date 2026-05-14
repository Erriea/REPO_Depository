using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace CaseFileLocalSuspect.AI
{
    public class OllamaClient : MonoBehaviour
    {
        [Serializable]
        private class OllamaGenerateRequest
        {
            public string model;
            public string prompt;
            public bool stream;

            public OllamaGenerateRequest(string modelName, string promptText)
            {
                model = modelName;
                prompt = promptText;
                stream = false;
            }
        }

        [Serializable]
        private class OllamaGenerateResponse
        {
            public string response;
            public string error;
        }

        [SerializeField] private string endpointUrl = "http://localhost:11434/api/generate";
        [SerializeField] private string defaultModel = "llama3.2";
        [SerializeField] private int requestTimeoutSeconds = 60;

        public void GenerateText(string prompt, Action<string> onSuccess, Action<string> onError)
        {
            StartCoroutine(GenerateTextRoutine(prompt, onSuccess, onError));
        }

        private IEnumerator GenerateTextRoutine(string prompt, Action<string> onSuccess, Action<string> onError)
        {
            OllamaGenerateRequest requestBody = new OllamaGenerateRequest(defaultModel, prompt);
            string json = JsonUtility.ToJson(requestBody);
            byte[] payload = Encoding.UTF8.GetBytes(json);

            using (UnityWebRequest request = new UnityWebRequest(endpointUrl, UnityWebRequest.kHttpVerbPOST))
            {
                request.uploadHandler = new UploadHandlerRaw(payload);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.timeout = requestTimeoutSeconds;
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    onError?.Invoke($"Ollama request failed: {request.error}");
                    yield break;
                }

                string rawResponse = request.downloadHandler.text;
                OllamaGenerateResponse parsedResponse = JsonUtility.FromJson<OllamaGenerateResponse>(rawResponse);

                if (parsedResponse == null)
                {
                    onError?.Invoke("Ollama returned an unreadable response.");
                    yield break;
                }

                if (!string.IsNullOrWhiteSpace(parsedResponse.error))
                {
                    onError?.Invoke(parsedResponse.error);
                    yield break;
                }

                onSuccess?.Invoke(parsedResponse.response);
            }
        }
    }
}
