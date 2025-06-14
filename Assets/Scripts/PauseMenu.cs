using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu; //reference to pause menu UI panel
    [SerializeField] AudioSource backgroundMusic; //reference to the background music AudioSource
    public GameObject Settings; //reference to settings ui panel

    public Slider TimerSlider; //reference to uislider
    public Text TimerSliderValue; //reference to timerslidervalue

    void Start()
    {
        UpdateValue(TimerSlider.value);
        TimerSlider.onValueChanged.AddListener(UpdateValue); //listener
    }

    public void UpdateValue(float value)
    {
        TimerSliderValue.text = Mathf.RoundToInt(value) + "s"; //displays the value of timer
    }

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

    public void OptionsOn() //placeholder for sprint 2
    {
        Settings.SetActive(true); //turns on the options screen
    }

    public void OptionsOff()
    {
        Settings.SetActive(false); //turns off options screen
    }
}
