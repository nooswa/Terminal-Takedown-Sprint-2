using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusicManager : MonoBehaviour
{
    public static MenuMusicManager instance;
    private AudioSource audioSource;
    private int previousSceneIndex = -1; // store last loaded scene index

    void Awake()
    {
        if (instance == null) //instance to ensure only 1 is running
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //ensures only this object is running
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            Destroy(gameObject); //destroys extra
        }
    }

    void OnEnable() //enables scene load
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() //disables sceneloads
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int[] menuScenes = { 0, 1, 2, 4 };

        // If coming from scene 5 to scene 0, restart the music
        if (previousSceneIndex == 5 && scene.buildIndex == 0)
        {
            audioSource.time = 0f; // reset to start
            audioSource.Play();
        }

        // If coming from scene 3 to scene 0, restart the music
        else if (previousSceneIndex == 3 && scene.buildIndex == 0)
        {
            audioSource.time = 0f; // reset to start
            audioSource.Play();
        }

        if (System.Array.Exists(menuScenes, index => index == scene.buildIndex)) //plays music based on scene
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }

        previousSceneIndex = scene.buildIndex; // update last scene index
    }
}




