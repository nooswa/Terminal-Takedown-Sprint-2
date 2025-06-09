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

        // Ensure this GameObject is not a child, otherwise DontDestroyOnLoad won't work
        if (transform.parent != null)
        {
            transform.SetParent(null);
        }

        DontDestroyOnLoad(gameObject);
    }
    else
    {
        Destroy(gameObject);
    }
}


    // Asynchronously generates a list of AI questions based on the discipline and topic list
    public async Task<List<Question>> GenerateQuestionsAsync(string disciplineClass, List<string> topics)
    {
        string prompt = ComposePrompt(disciplineClass, topics);

        var requestData = new
        {
            model = "llama3.2",
            prompt = prompt,
            stream = false
        };

        string json = JsonUtility.ToJson(requestData);

        using (UnityWebRequest req = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            req.uploadHandler = new UploadHandlerRaw(bodyRaw);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            var operation = req.SendWebRequest();
            while (!operation.isDone) await Task.Yield();

            if (req.result != UnityWebRequest.Result.Success)
                throw new System.Exception("AI API error: " + req.error);

            string rawResponse = req.downloadHandler.text;

            // Parse the API's JSON wrapper response
            var responseWrapper = JsonUtility.FromJson<OllamaResponse>(rawResponse);

            // Convert array to List and return
            return JsonHelper.FromJson<Question>(responseWrapper.response).ToList();
        }
    }

    // Composes a prompt string for the AI based on discipline and topics
    private string ComposePrompt(string disciplineClass, List<string> topics)
    {
        string topicsStr = string.Join(", ", topics);
        return $"Generate 30 multiple-choice technical interview questions for {disciplineClass}, covering these topics: {topicsStr}. Each question should have 4 answer options (A, B, C, D) that are no longer than 15 characters each and indicate the correct option (index 0-3). Format the output as a JSON array of objects with fields: question (string), answers (array of string), correctAnswerIndex (int).";
    }

    [System.Serializable]
    private class OllamaResponse
    {
        public string response;
    }
}
