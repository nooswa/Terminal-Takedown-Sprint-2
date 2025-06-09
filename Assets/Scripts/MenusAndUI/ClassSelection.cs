using UnityEngine;
using UnityEngine.SceneManagement;

public class ClassSelection : MonoBehaviour
{
    public void PlayClass1() // loads cybersec checklist screen
    {
        PlayerSelectionManager.Instance.SetClass("Cybersecurity");
        SceneManager.LoadSceneAsync(2); // scene index for cybersec checklist
    }

    public void PlayClass2() // loads softdev checklist screen
    {
        PlayerSelectionManager.Instance.SetClass("SoftwareDev");
        SceneManager.LoadSceneAsync(4); // scene index for softdev checklist
    }
}