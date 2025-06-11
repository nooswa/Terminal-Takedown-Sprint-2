using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance { get; private set; }

    public List<Question> questions = new List<Question>();
    public bool IsLoading { get; private set; }
    public bool HasQuestions => questions != null && questions.Count > 0;

    public event Action<bool> OnQuestionsLoaded; // true = success, false = fail

    private void Awake()
    {
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

    /// <summary>
    /// Called by UI to start loading questions based on user selection.
    /// </summary>
    public void BeginLoadingQuestions(string disciplineClass, List<string> topics)
    {
        _ = InitializeWithAIQuestions(disciplineClass, topics);
    }

    private async Task InitializeWithAIQuestions(string disciplineClass, List<string> topics)
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

        try
        {
            var generated = await AIQuestionService.Instance.GenerateQuestionsAsync(disciplineClass, topics);
            if (generated == null || generated.Count == 0)
            {
                Debug.LogError("QuestionManager: AI returned no questions");
                OnQuestionsLoaded?.Invoke(false);
                return;
            }

            questions = new List<Question>(generated);
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

    public void ClearQuestions()
    {
        questions.Clear();
        Debug.Log("Questions cleared");
    }
}