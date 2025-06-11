using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float time; //current time remaining
    public Text TimerText; //reference to UI text component
    public Image Fill; //reference to UI fill image
    public float Max; //max value of timer

    void Start()
    {
        // Initializing the timer 
        time = Max;
    }

    void Update() //called once per frame
    {
        time -= Time.deltaTime; //decrease timer by time passed since last frame
        TimerText.text = "" + Mathf.CeilToInt(time); // Using CeilToInt to round up to 120 secs
        Fill.fillAmount = time / Max; //update fill image

        if (time < 0) //clamp time to prevent going to 0
            time = 0;
    }

    // Method to add time to the timer
    public void AddTime(float additionalTime)
    {
        time += additionalTime;

        // Ensuring the timer doesn't exceed its maximum
        if (time > Max)
        {
            time = Max;
        }
    }
}