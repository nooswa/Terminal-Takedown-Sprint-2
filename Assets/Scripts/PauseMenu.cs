using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu; //reference to pause menu UI panel
    [SerializeField] AudioSource backgroundMusic; //reference to the background music AudioSource

    public void Pause() //called when game is paused
    {
        pauseMenu.SetActive(true); //show the pause menu
        Time.timeScale = 0; //freeze the game

        if (backgroundMusic != null && backgroundMusic.isPlaying) //pause background music if playing
        {
            backgroundMusic.Pause();
        }
    }

    public void Menu() //placeholder for sprint 2
    {

    }

    public void Resume() //called when player resumes the game
    {
        pauseMenu.SetActive(false); //hide the pause menu UI
        Time.timeScale = 1; //resumes the game

        if (backgroundMusic != null && !backgroundMusic.isPlaying) //resumes background music if paused
        {
            backgroundMusic.Play();
        }
    }

    public void Options() //placeholder for sprint 2
    {

    }
}
