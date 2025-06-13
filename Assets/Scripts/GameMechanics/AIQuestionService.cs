using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Linq;


// Service to generate AI-driven multiple-choice questions via local LLM API.

public class AIQuestionService : MonoBehaviour
{
    public static AIQuestionService Instance { get; private set; }
    private readonly string apiUrl = "http://localhost:11434/api/generate"; //ai api url

    private void Awake()
    {
        // Singleton pattern for easy global access ensures only 1 instance is running
        if (Instance == null)
        {
            Instance = this;
            if (transform.parent != null)
                transform.SetParent(null);
            DontDestroyOnLoad(gameObject); //makes sure only this object is running
        }
        else
        {
            Destroy(gameObject); // deletes duplicates
        }
    }

    [System.Serializable]
    private class OllamaResponse
    {
        public string response; //ai reply
    }

    [System.Serializable]
    private class GenerateRequest //generate request to ai api format
    {
        public string model;
        public string prompt;
        public bool stream;
    }


    // Generate a single multiple-choice question from AI.

    public async Task<Question> GenerateSingleQuestionAsync(string disciplineClass, List<string> topics)
    {
        float startTime = Time.realtimeSinceStartup;

        string prompt = ComposePrompt(disciplineClass, topics); //prompt

        var requestData = new GenerateRequest
        {
            model = "llama3:8b",
            prompt = prompt,
            stream = false
        };

        string json = JsonUtility.ToJson(requestData);

        Debug.Log("AI API URL: " + apiUrl);
        Debug.Log("AI API request JSON: " + json);

        using (UnityWebRequest req = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            req.uploadHandler = new UploadHandlerRaw(bodyRaw);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            // Await the response
            var operation = req.SendWebRequest();
            var tcs = new TaskCompletionSource<bool>();
            operation.completed += _ => tcs.SetResult(true);
            await tcs.Task;

            float afterRequest = Time.realtimeSinceStartup;
            Debug.Log($"AIQuestionService: API call duration: {afterRequest - startTime:F2} seconds");

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("AI API error: " + req.error + " | URL: " + apiUrl);
                throw new System.Exception("AI API error: " + req.error);
            }

            string rawResponse = req.downloadHandler.text;
            Debug.Log("AI API raw response: " + rawResponse);

            float afterDeserializationStart = Time.realtimeSinceStartup;

            var responseWrapper = JsonUtility.FromJson<OllamaResponse>(rawResponse);
            string jsonArray = ExtractCleanJsonArray(responseWrapper.response);
            Debug.Log("Extracted JSON array: " + jsonArray);

            var arr = JsonHelper.FromJson<Question>(jsonArray);
            if (arr == null || arr.Length == 0)
            {
                Debug.LogError("Deserialization returned null or empty! JSON: " + jsonArray);
                return null;
            }
            Question question = arr[0];

            float afterDeserialization = Time.realtimeSinceStartup;
            Debug.Log($"AIQuestionService: Deserialization duration: {afterDeserialization - afterDeserializationStart:F2} seconds");

            Debug.Log($"Question: {question.question} | Answers: {string.Join(", ", question.answers)} | Correct: {question.correctAnswerIndex}");
            Debug.Log($"AIQuestionService: Total duration: {afterDeserialization - startTime:F2} seconds");

            return question;
        }
    }


    // Create the AI prompt for a single question.

    private string ComposePrompt(string disciplineClass, List<string> topics)
    {
        string topicsStr = string.Join(", ", topics);
        return $"Write 1 concise multiple-choice interview question about \"{disciplineClass}\" (topic: one random from [{topicsStr}]). Respond with a raw JSON array: [{{\"question\":\"...\",\"answers\":[\"...\",...],\"correctAnswerIndex\":N}}] (answers: 4, ≤20 chars each, N: 0–3). No explanations or extra text.";
    }


    // Extracts a JSON array from a raw string (LLM response).

    private static string ExtractCleanJsonArray(string responseText)
    {
        int arrayStart = responseText.IndexOf('[');
        int arrayEnd = responseText.LastIndexOf(']');
        if (arrayStart == -1 || arrayEnd == -1 || arrayEnd <= arrayStart)
            return "[]";

        string arrayText = responseText.Substring(arrayStart, arrayEnd - arrayStart + 1);

        // Extract all JSON objects from the array string
        var elements = new List<string>();
        int idx = 0;
        while (idx < arrayText.Length)
        {
            int objStart = arrayText.IndexOf('{', idx);
            if (objStart == -1) break;
            int objEnd = arrayText.IndexOf('}', objStart);
            if (objEnd == -1) break;
            elements.Add(arrayText.Substring(objStart, objEnd - objStart + 1));
            idx = objEnd + 1;
        }
        if (elements.Count == 0)
            return "[]";
        return "[" + string.Join(",", elements) + "]";
    }
}