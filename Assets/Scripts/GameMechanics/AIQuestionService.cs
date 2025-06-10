using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Linq;

// This class requests AI-generated questions and parses them correctly for Unity
public class AIQuestionService : MonoBehaviour
{
    public static AIQuestionService Instance { get; private set; }
    private readonly string apiUrl = "http://localhost:11434/api/generate";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (transform.parent != null)
                transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [System.Serializable]
    private class OllamaResponse
    {
        public string response;
    }

    [System.Serializable]
    private class GenerateRequest
    {
        public string model;
        public string prompt;
        public bool stream;
    }

    public async Task<List<Question>> GenerateQuestionsAsync(string disciplineClass, List<string> topics)
    {
        string prompt = ComposePrompt(disciplineClass, topics);

        var requestData = new GenerateRequest
        {
            model = "phi3:mini",
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

            var operation = req.SendWebRequest();
            while (!operation.isDone) await Task.Yield();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("AI API error: " + req.error + " | URL: " + apiUrl);
                throw new System.Exception("AI API error: " + req.error);
            }

            string rawResponse = req.downloadHandler.text;
            Debug.Log("AI API raw response: " + rawResponse);

            var responseWrapper = JsonUtility.FromJson<OllamaResponse>(rawResponse);

            string jsonArray = ExtractJsonArray(responseWrapper.response);
            Debug.Log("Extracted JSON array: " + jsonArray);

            // Correct usage: just pass the raw array, JsonHelper wraps it internally
            var arr = JsonHelper.FromJson<Question>(jsonArray);
            if (arr == null)
            {
                Debug.LogError("Deserialization returned null! JSON: " + jsonArray);
                return new List<Question>();
            }
            List<Question> questions = arr.ToList();

            for (int i = 0; i < questions.Count; i++)
            {
                Debug.Log($"Question {i + 1}: {questions[i].question} | Answers: {string.Join(", ", questions[i].answers)} | Correct: {questions[i].correctAnswerIndex}");
            }

            return questions;
        }
    }

    private string ComposePrompt(string disciplineClass, List<string> topics)
    {
        string topicsStr = string.Join(", ", topics);
        return $"Generate 1 multiple-choice technical interview questions for {disciplineClass}, covering these topics: {topicsStr}. Each question should have 4 answer options (A, B, C, D) that are no longer than 20 characters each and indicate the correct option (index 0-3). Output ONLY a valid JSON array of objects with fields: question (string), answers (array of string), correctAnswerIndex (int). Do NOT include any explanation, markdown, or extra text—just the raw JSON array.";
    }

    // Helper method to extract JSON array from code block or markdown
    private static string ExtractJsonArray(string responseText)
    {
        // Try to find the first [ and last ] in the response
        int arrayStart = responseText.IndexOf('[');
        int arrayEnd = responseText.LastIndexOf(']');
        if (arrayStart != -1 && arrayEnd != -1 && arrayEnd > arrayStart)
            return responseText.Substring(arrayStart, arrayEnd - arrayStart + 1);
        // If not found, return an empty array
        return "[]";
    }
}