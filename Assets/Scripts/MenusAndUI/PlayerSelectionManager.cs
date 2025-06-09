using UnityEngine;
using System.Collections.Generic;

public class PlayerSelectionManager : MonoBehaviour
{
    public static PlayerSelectionManager Instance { get; private set; }

    public string SelectedClass { get; private set; }
    public List<string> SelectedTopics { get; private set; } = new List<string>();

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

#if UNITY_EDITOR
    // Test-only: forcibly reset the singleton
    public static void ResetInstanceForTests()
    {
        Instance = null;
    }
#endif

    public void SetClass(string className)
    {
        SelectedClass = className;
    }

    public void SetTopics(List<string> topics)
    {
        SelectedTopics = new List<string>(topics);
    }

    public void ClearSelections()
    {
        SelectedClass = null;
        SelectedTopics.Clear();
    }
}