using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance { get; private set; }

    // Holds the current list of questions (loaded or AI-generated)
    public List<Question> questions = new List<Question>();
    
    // Track loading state
    public bool IsLoading { get; private set; }
    public bool HasQuestions => questions != null && questions.Count > 0;

    private void Awake()
    {
        Debug.Log("QuestionManager Awake!");

        if (Instance == null)
        {
            Instance = this;
            // Don't detach from parent unnecessarily
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
        if (!HasQuestions)
        {
            Debug.LogError("No questions available! Make sure questions are loaded before calling GetRandomQuestion()");
            return null;
        }

        int randomIndex = Random.Range(0, questions.Count);
        Question selectedQuestion = questions[randomIndex];
        
        Debug.Log($"Selected question: {selectedQuestion.question}");
        return selectedQuestion;
    }

    // Replaces the current question list with a new one
    public void LoadQuestions(List<Question> newQuestions)
    {
        if (newQuestions != null && newQuestions.Count > 0)
        {
            questions = new List<Question>(newQuestions); // Create a copy
            Debug.Log($"Loaded {questions.Count} questions successfully");
        }
        else
        {
            Debug.LogWarning("Tried to load empty or null question list.");
            questions = new List<Question>(); // Ensure it's not null
        }
    }

    // Loads AI-generated questions based on selected class and topics
    public async Task InitializeWithAIQuestions(string disciplineClass, List<string> topics)
    {
        if (IsLoading)
        {
            Debug.LogWarning("Already loading questions, please wait...");
            return;
        }
        
        IsLoading = true;
        
        try
        {
            Debug.Log($"Generating AI questions for {disciplineClass} with topics: {string.Join(", ", topics)}");

            // Validate inputs
            if (string.IsNullOrEmpty(disciplineClass))
            {
                Debug.LogError("Discipline class is null or empty!");
                return;
            }
            
            if (topics == null || topics.Count == 0)
            {
                Debug.LogError("No topics provided!");
                return;
            }

            // Check if AIQuestionService is available
            if (AIQuestionService.Instance == null)
            {
                Debug.LogError("AIQuestionService.Instance is NULL! Make sure AIQuestionService GameObject is in the scene and properly initialized.");
                return;
            }

            // Generate questions
            var generated = await AIQuestionService.Instance.GenerateQuestionsAsync(disciplineClass, topics);
            
            if (generated == null || generated.Count == 0)
            {
                Debug.LogError("AI returned no questions! Check your AI service configuration and prompt.");
                return;
            }

            // Load the generated questions
            LoadQuestions(generated);
            Debug.Log($"Successfully loaded {generated.Count} AI-generated questions.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to generate AI questions: {ex.Message}\nStackTrace: {ex.StackTrace}");
        }
        finally
        {
            IsLoading = false;
        }
    }
    
    // Method to clear questions (useful for testing or resetting)
    public void ClearQuestions()
    {
        questions.Clear();
        Debug.Log("Questions cleared");
    }
}