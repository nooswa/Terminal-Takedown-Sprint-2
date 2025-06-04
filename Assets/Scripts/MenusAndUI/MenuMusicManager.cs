using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusicManager : MonoBehaviour{
    public static MenuMusicManager instance;
    private AudioSource audioSource;
    // store last loaded scene index to track scene transitions
    private int previousSceneIndex = -1; 

    void Awake() 
    {   //this makes sure only 1 instance of the script exists at a time
        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //object stays alive betwenn scenes
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true; 
            audioSource.Play();
        }
        else{
            Destroy(gameObject);
        }
    }
    void OnEnable() //tracks scene changes
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() //stops tracking when object is destroyed
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int[] menuScenes = { 0, 1, 2, 4 }; //scenes that play menu music

        //restarts menu music when transitioning from softdev in-game scene back to main menu
        if (previousSceneIndex == 5 && scene.buildIndex == 0) {
            audioSource.time = 0f; // reset to start
            audioSource.Play();
        }

        //restarts menu music when transitioning from cybersec in-game scene back to main menu
        else if (previousSceneIndex == 3 && scene.buildIndex == 0){
            audioSource.time = 0f; // reset to start
            audioSource.Play();
        }
        //if the current scene is a scene that plays menu music, makes sure music is playing
        if (System.Array.Exists(menuScenes, index => index == scene.buildIndex)){
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else{ //stops menu music in scenes that are not menu screens
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }

        previousSceneIndex = scene.buildIndex; // updates the scene index
    }
}




