using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Linq;

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
        float startTime = Time.realtimeSinceStartup;

        string prompt = ComposePrompt(disciplineClass, topics);

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

            // Modern Unity: use SendWebRequest().completed + TaskCompletionSource for proper async
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
            if (arr == null)
            {
                Debug.LogError("Deserialization returned null! JSON: " + jsonArray);
                return new List<Question>();
            }
            List<Question> questions = arr.ToList();

            float afterDeserialization = Time.realtimeSinceStartup;
            Debug.Log($"AIQuestionService: Deserialization duration: {afterDeserialization - afterDeserializationStart:F2} seconds");

            for (int i = 0; i < questions.Count; i++)
            {
                Debug.Log($"Question {i + 1}: {questions[i].question} | Answers: {string.Join(", ", questions[i].answers)} | Correct: {questions[i].correctAnswerIndex}");
            }

            Debug.Log($"AIQuestionService: Total duration: {afterDeserialization - startTime:F2} seconds");

            return questions;
        }
    }

    private string ComposePrompt(string disciplineClass, List<string> topics)
    {
        string topicsStr = string.Join(", ", topics);
        return $"Generate exactly 10 concise multiple-choice technical interview questions for the subject \"{disciplineClass}\". Each question must focus on a single topic, randomly selected from the list: [{topicsStr}]. Each question must have: - A `question` field (string). - An `answers` field (array of 4 strings, each answer no longer than 20 characters). - A `correctAnswerIndex` field (integer 0–3). Output must be only a raw JSON array of 10 objects, each with the fields: question, answers, correctAnswerIndex. Do not include any explanations, markdown, extra text, or formatting—only the raw JSON array. Example: [{{\"question\":\"What is a binary search?\",\"answers\":[\"A search tree\",\"A sorted array algorithm\",\"A linear scan\",\"A hash lookup\"],\"correctAnswerIndex\":1}}]";    }

    // Helper method to extract JSON array from code block or markdown
    private static string ExtractCleanJsonArray(string responseText)
    {
        int arrayStart = responseText.IndexOf('[');
        int arrayEnd = responseText.LastIndexOf(']');
        if (arrayStart == -1 || arrayEnd == -1 || arrayEnd <= arrayStart)
            return "[]";

        string arrayText = responseText.Substring(arrayStart, arrayEnd - arrayStart + 1);

        // Remove non-object elements
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