using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Handles displaying questions, receiving player input, and managing robot interactions
public class TerminalUIHandler : MonoBehaviour
{
    public TMP_Text questionText; 
    public Button[] answerButtons; 
    public GameObject terminalUI; 
    private SoftDevQuestion currentQuestion;
    private GameObject clickedRobot; 

    public SoftDevQuestionManager questionManager; 
    public GameObject explosionPrefab; 

    public Timer timer;

    // Method to show terminal
    public void OpenTerminal(GameObject robot)
    {
        Time.timeScale = 0;

        clickedRobot = robot;

        currentQuestion = questionManager.GetRandomQuestion();

        questionText.text = currentQuestion.question;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < currentQuestion.answers.Count)
            {
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].GetComponentInChildren<TMP_Text>().text = currentQuestion.answers[i];

                int index = i; 
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false); 
            }
        }

        terminalUI.SetActive(true);
    }

    // Method for when an answer button is clicked
    public void OnAnswerSelected(int index)
    {
        Time.timeScale = 1;

        terminalUI.SetActive(false);

        if (index == currentQuestion.correctAnswerIndex)
        {
            ExplodeRobot(clickedRobot);
            // Add 15 seconds to the timer
            if (timer != null)
            {
                timer.AddTime(15f);
            }
        }
    }
    
    // Method to destroy (explode) the robot
    private void ExplodeRobot(GameObject robot)
    {
        // Instantiate the explosion animation at the robot's position
        if (robot != null)
        {
            
            if (explosionPrefab != null)
            {
                // Instantiate the explosion prefab
                GameObject explosion = Instantiate(explosionPrefab, robot.transform.position, Quaternion.identity);

                // Trigger the explosion animation
                Animator explosionAnimator = explosion.GetComponent<Animator>();
                if (explosionAnimator != null)
                {
                    explosionAnimator.SetTrigger("OnEnemyDeath");
                }
               

                // Play the explosion sound effect
                AudioSource audioSource = explosion.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.Play();
                }
            
            }
            
            // Destroy the robot GameObject after the explosion
            Destroy(robot);

        }
    }
}