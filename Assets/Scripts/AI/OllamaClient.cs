using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace CaseFileLocalSuspect.AI
{
    public class OllamaClient : MonoBehaviour
    {
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
            GenerateText(prompt, false, null, onSuccess, onError);
        }

        public void GenerateText(string prompt, bool expectsJson, Action<string> onSuccess, Action<string> onError)
        {
            GenerateText(prompt, expectsJson, null, onSuccess, onError);
        }

        public void GenerateStructuredJson(string prompt, string jsonSchema, Action<string> onSuccess, Action<string> onError)
        {
            GenerateText(prompt, true, jsonSchema, onSuccess, onError);
        }

        private void GenerateText(string prompt, bool expectsJson, string jsonSchema, Action<string> onSuccess, Action<string> onError)
        {
            StartCoroutine(GenerateTextRoutine(prompt, expectsJson, jsonSchema, onSuccess, onError));
        }

        private IEnumerator GenerateTextRoutine(string prompt, bool expectsJson, string jsonSchema, Action<string> onSuccess, Action<string> onError)
        {
            string requestJson = BuildRequestJson(prompt, expectsJson, jsonSchema);
            byte[] payload = System.Text.Encoding.UTF8.GetBytes(requestJson);

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

        private string BuildRequestJson(string prompt, bool expectsJson, string jsonSchema)
        {
            string formatJson = "\"format\":\"json\"";

            if (!string.IsNullOrWhiteSpace(jsonSchema))
            {
                formatJson = $"\"format\":{jsonSchema}";
            }
            else if (!expectsJson)
            {
                formatJson = "\"format\":null";
            }

            return
                "{" +
                $"\"model\":\"{EscapeJson(defaultModel)}\"," +
                $"\"prompt\":\"{EscapeJson(prompt)}\"," +
                "\"stream\":false," +
                $"{formatJson}," +
                "\"options\":{\"temperature\":0}" +
                "}";
        }

        private static string EscapeJson(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return value
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\r", "\\r")
                .Replace("\n", "\\n")
                .Replace("\t", "\\t");
        }
    }
}
