using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ClassSelection : MonoBehaviour
{

    public void PlayClass1() //loads cybersec checklist screen
    {
        SceneManager.LoadSceneAsync(2); // scene index 
    }

    public void PlayClass2() //loads softdev checklist screen
    {
        SceneManager.LoadSceneAsync(4); // scene index
    }
}

