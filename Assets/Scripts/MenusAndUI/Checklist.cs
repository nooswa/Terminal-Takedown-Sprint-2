using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class Checklist : MonoBehaviour
{
    public List<GameObject> topicRows;
    public int gameSceneIndex = 5; 

    public async void OnStartGamePressed()
    {
        List<string> selectedTopics = new List<string>();
        foreach (var row in topicRows)
        {
            Toggle toggle = row.GetComponentInChildren<Toggle>();
            Text label = row.transform.Find("Label").GetComponent<Text>();
            if (toggle != null && toggle.isOn && label != null)
            {
                selectedTopics.Add(label.text);
            }
        }
        PlayerSelectionManager.Instance.SetTopics(selectedTopics);

        // Load questions before starting the game!
        await QuestionManager.Instance.InitializeWithAIQuestions(
            PlayerSelectionManager.Instance.SelectedClass,
            PlayerSelectionManager.Instance.SelectedTopics
        );

        // Only load the game scene if we are still in Play Mode
        if (Application.isPlaying)
        {
            _ = SceneManager.LoadSceneAsync(gameSceneIndex);
        }
    }
}