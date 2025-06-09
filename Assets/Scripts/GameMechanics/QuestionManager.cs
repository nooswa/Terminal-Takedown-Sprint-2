using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance { get; private set; }

    // Holds the current list of questions (loaded or AI-generated)
    public List<Question> questions = new List<Question>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (transform.parent != null)
                transform.SetParent(null); // Detach from parent
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Returns a random question from the list
    public Question GetRandomQuestion()
    {
        if (questions == null || questions.Count == 0)
        {
            Debug.LogError("No questions available!");
            return null;
        }

        int randomIndex = Random.Range(0, questions.Count);
        return questions[randomIndex];
    }

    // Replaces the current question list with a new one
    public void LoadQuestions(List<Question> newQuestions)
    {
        if (newQuestions != null && newQuestions.Count > 0)
        {
            questions = newQuestions;
        }
        else
        {
            Debug.LogWarning("Tried to load empty or null question list.");
        }
    }

    // 🔄 NEW: Loads AI-generated questions based on selected class and topics
    public async Task InitializeWithAIQuestions(string disciplineClass, List<string> topics)
    {
        Debug.Log($"Generating AI questions for {disciplineClass} with topics: {string.Join(", ", topics)}");

        try
        {
            var generated = await AIQuestionService.Instance.GenerateQuestionsAsync(disciplineClass, topics);
            LoadQuestions(generated);
            Debug.Log($"Loaded {generated.Count} AI-generated questions.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to generate AI questions: " + ex.Message);
        }
    }
}
