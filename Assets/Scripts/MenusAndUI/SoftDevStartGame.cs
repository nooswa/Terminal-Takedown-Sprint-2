using UnityEngine;
using UnityEngine.SceneManagement;

public class SoftDevStartGame : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadSceneAsync(5); // Master Scene index
    }
}
