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
        [SerializeField] private int requestTimeoutSeconds = 180;
        [SerializeField] private int timeoutRetryCount = 1;

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
            StartCoroutine(GenerateTextRoutine(prompt, expectsJson, jsonSchema, onSuccess, onError, timeoutRetryCount + 1));
        }

        private IEnumerator GenerateTextRoutine(string prompt, bool expectsJson, string jsonSchema, Action<string> onSuccess, Action<string> onError, int attemptsRemaining)
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
                    bool timedOut = request.result == UnityWebRequest.Result.ConnectionError
                        && !string.IsNullOrWhiteSpace(request.error)
                        && request.error.IndexOf("timed out", StringComparison.OrdinalIgnoreCase) >= 0;

                    if (timedOut && attemptsRemaining > 1)
                    {
                        Debug.LogWarning($"Ollama timed out after {requestTimeoutSeconds} seconds. Retrying once more because the model may still be warming up.");
                        yield return GenerateTextRoutine(prompt, expectsJson, jsonSchema, onSuccess, onError, attemptsRemaining - 1);
                        yield break;
                    }

                    onError?.Invoke(BuildFailureMessage(request.error, timedOut));
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

        private string BuildFailureMessage(string requestError, bool timedOut)
        {
            if (timedOut)
            {
                return
                    $"Ollama request timed out after {requestTimeoutSeconds} seconds at {endpointUrl} using model {defaultModel}. " +
                    "The local endpoint is probably reachable, but the model is still warming up. Try again after Ollama has loaded the model into memory.";
            }

            return $"Ollama request failed at {endpointUrl} using model {defaultModel}: {requestError}";
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
                "\"keep_alive\":\"10m\"," +
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
