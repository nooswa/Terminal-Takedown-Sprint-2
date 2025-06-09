using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusicManager : MonoBehaviour
{
    public static MenuMusicManager instance;
    private AudioSource audioSource;
    // store last loaded scene index to track scene transitions
    private int previousSceneIndex = -1; 

    // scene index constants
    private const int MAIN_MENU          = 0;
    private const int CLASS_SELECTION    = 1;
    private const int CYBERSEC_CHECKLIST = 2;
    private const int SOFTDEV_CHECKLIST  = 4;
    private const int CYBERSEC_GAME      = 3;
    private const int SOFTDEV_GAME       = 5;

    // scenes that play menu music
    private readonly int[] menuScenes = {
        MAIN_MENU,
        CLASS_SELECTION,
        CYBERSEC_CHECKLIST,
        SOFTDEV_CHECKLIST
    };

    void Awake() 
    {   // this makes sure only 1 instance of the script exists at a time
        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // object stays alive between scenes
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true; 
            audioSource.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable() // tracks scene changes
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() // stops tracking when object is destroyed
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int currentScene = scene.buildIndex;

        // restart music when returning to main menu from any game scene
        if (currentScene == MAIN_MENU &&
            (previousSceneIndex == CYBERSEC_GAME || previousSceneIndex == SOFTDEV_GAME))
        {
            restartMusic();
        }

        // play or pause depending on whether this is a menu scene
        if (System.Array.Exists(menuScenes, i => i == currentScene))
            ensureMusicPlaying();
        else
            pauseMusic();

        previousSceneIndex = currentScene; // updates the scene index
    }

    private void restartMusic()
    {
        audioSource.time = 0f;
        audioSource.Play();
    }

    private void ensureMusicPlaying()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    private void pauseMusic()
    {
        if (audioSource.isPlaying)
            audioSource.Pause();
    }
}




