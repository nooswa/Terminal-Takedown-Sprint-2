using UnityEngine;
using System.Collections.Generic;

public class PlayerSelectionManager : MonoBehaviour
{
    public static PlayerSelectionManager Instance { get; private set; }

    public string SelectedClass { get; private set; }
    public List<string> SelectedTopics { get; private set; } = new List<string>();

    private void Awake()
    {
        if (Instance == null) //ensures only 1 instance is running
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //ensures this instance keeps running
        }
        else
        {
            Destroy(gameObject); //destroys an extras
        }
        
    }

#if UNITY_EDITOR
    // Test-only: forcibly reset the singleton
    public static void ResetInstanceForTests()
    {
        Instance = null;
    }
#endif

    public void SetClass(string className) //assigns class name
    {
        SelectedClass = className;
    }

    public void SetTopics(List<string> topics) //selected topics
    {
        SelectedTopics = new List<string>(topics);
    }

    public void ClearSelections() //clears selected topics
    {
        SelectedClass = null;
        SelectedTopics.Clear();
    }
}