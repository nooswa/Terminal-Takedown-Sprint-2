using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float time; // Current time remaining
    public Text TimerText; // Reference to UI text component
    public Image Fill; // Reference to UI fill image
    public float Max; // Max value of timer

    private HealthManager healthManager; // Reference to HealthManager
    private bool gameEnded = false; // Prevent multiple triggers

    private float lastRealTime;

    public Slider TimeSlider;

    void Start()
    {
        // Initializing the timer 
        time = Max;

        if (TimeSlider != null)
        {
            TimeSlider.minValue = 15f; //lowest possible time from setting
            TimeSlider.maxValue = 80f; //highest
            TimeSlider.value = Max;

            TimeSlider.onValueChanged.AddListener(UpdateMaxTime);
        }

        healthManager = FindAnyObjectByType<HealthManager>();
        lastRealTime = Time.realtimeSinceStartup;
    }

    void Update()
    {
        if (gameEnded || time <= 0f) return;

        // Calculating elapsed real time, unaffected by timeScale
        float currentRealTime = Time.realtimeSinceStartup;
        float elapsed = currentRealTime - lastRealTime;
        lastRealTime = currentRealTime;

        time -= elapsed; // Using real time

        TimerText.text = "" + Mathf.CeilToInt(time);
        Fill.fillAmount = time / Max;

        if (time <= 0f)
        {
            time = 0f;
            gameEnded = true;

            if (healthManager != null && !healthManager.IsDead())
            {
                healthManager.MarkAsDead(); // Ends the game
            }
        }
    }

   // Increase Time
    public void AddTime(float additionalTime)
    {
        time += additionalTime;

        if (time > Max)
        {
            time = Max;
        }
    }

    public void UpdateMaxTime(float newMax) //assigns max with the newmax value
    {
        Max = newMax;
    }

    public void Apply(float apply) //FOR UPDATING THE TIME (FOR APLYING CHANGES UPON EXITING THE SETTINGS!)
    {
        time += apply;

        // Ensuring the timer doesn't exceed its maximum
        if (time > Max)
        {
            time = Max;
        }
    }

}