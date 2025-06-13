using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonHandler : MonoBehaviour
{
        public int sceneIndexToLoad;

    public void LoadPreviousScene() //loads previous scene
    {
        SceneManager.LoadScene(sceneIndexToLoad); 
    }
}
