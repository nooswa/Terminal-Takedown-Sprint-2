using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;


// Manages loading and retrieval of AI-generated questions.

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance { get; private set; }
    public List<Question> questions = new List<Question>();
    public bool IsLoading { get; private set; }
    public bool HasQuestions => questions != null && questions.Count > 0;

    public event Action<bool> OnQuestionsLoaded; // true = success, false = fail

    private void Awake()
    {
        // Singleton pattern for global access
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Begins loading questions asynchronously given class and topics.

    public void BeginLoadingQuestions(string disciplineClass, List<string> topics, int desiredCount = 10)
    {
        _ = InitializeWithAIQuestions(disciplineClass, topics, desiredCount);
    }


    // Prompts AI for one question at a time, building the list dynamically.

    private async Task InitializeWithAIQuestions(string disciplineClass, List<string> topics, int desiredCount)
    {
        if (IsLoading)
        {
            Debug.LogWarning("QuestionManager: Already loading or initialized");
            OnQuestionsLoaded?.Invoke(false);
            return;
        }

        if (string.IsNullOrEmpty(disciplineClass) || topics == null || topics.Count == 0)
        {
            Debug.LogError("QuestionManager: Invalid inputs");
            OnQuestionsLoaded?.Invoke(false);
            return;
        }

        if (AIQuestionService.Instance == null)
        {
            Debug.LogError("QuestionManager: Missing AIQuestionService");
            OnQuestionsLoaded?.Invoke(false);
            return;
        }

        IsLoading = true;
        questions = new List<Question>();

        try
        {
            for (int i = 0; i < desiredCount; i++)
            {
                var question = await AIQuestionService.Instance.GenerateSingleQuestionAsync(disciplineClass, topics);
                if (question == null)
                {
                    Debug.LogError($"QuestionManager: AI returned no question at iteration {i + 1}");
                    OnQuestionsLoaded?.Invoke(false);
                    return;
                }
                questions.Add(question);
                Debug.Log($"QuestionManager: Added question {i + 1}/{desiredCount}");
            }
            Debug.Log($"QuestionManager: Loaded {questions.Count} questions");
            OnQuestionsLoaded?.Invoke(true);
        }
        catch (Exception ex)
        {
            Debug.LogError($"QuestionManager: Question init failed: {ex.Message}");
            OnQuestionsLoaded?.Invoke(false);
        }
        finally
        {
            IsLoading = false;
        }
    }

    // Returns a random question from the list.
    public Question GetRandomQuestion()
    {
        if (!HasQuestions)
        {
            Debug.LogError("No questions available!");
            return null;
        }
        int randomIndex = UnityEngine.Random.Range(0, questions.Count);
        return questions[randomIndex];
    }

    // Clears all loaded questions.
    public void ClearQuestions()
    {
        questions.Clear();
        Debug.Log("Questions cleared");
    }
}