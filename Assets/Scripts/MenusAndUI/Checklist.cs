using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Checklist : MonoBehaviour
{
    public List<GameObject> topicRows;

    public void OnStartGamePressed()
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
        // SceneManager.LoadSceneAsync( ... );
    }
}