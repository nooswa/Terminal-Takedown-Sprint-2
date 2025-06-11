using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Checklist : MonoBehaviour
{
    [Header("Checklist UI")]
    public List<GameObject> topicRows;
    public int gameSceneIndex = 5;

    private void Awake()
    {
        // Subscribe to question load completion
        if (QuestionManager.Instance != null)
            QuestionManager.Instance.OnQuestionsLoaded += OnQuestionsLoaded;
    }

    private void OnDestroy()
    {
        if (QuestionManager.Instance != null)
            QuestionManager.Instance.OnQuestionsLoaded -= OnQuestionsLoaded;
    }

    public void OnStartGamePressed()
    {
        // Gather selected topics from UI
        List<string> selectedTopics = new List<string>();
        foreach (var row in topicRows)
        {
            Toggle toggle = row.GetComponentInChildren<Toggle>();
            Text label = row.transform.Find("Label").GetComponent<Text>();
            if (toggle != null && toggle.isOn && label != null)
                selectedTopics.Add(label.text);
        }
        PlayerSelectionManager.Instance.SetTopics(selectedTopics);

        Debug.Log("Checklist: Passing selection to QuestionManager...");
        QuestionManager.Instance.BeginLoadingQuestions(
            PlayerSelectionManager.Instance.SelectedClass,
            PlayerSelectionManager.Instance.SelectedTopics
        );
    }

    private async void OnQuestionsLoaded(bool success)
    {
        if (!success)
        {
            Debug.LogError("Checklist: Failed to load questions! Not transitioning.");
            return;
        }
        Debug.Log("Checklist: Questions loaded, loading game scene...");
        await SceneManager.LoadSceneAsync(gameSceneIndex);
    }
}