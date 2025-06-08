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
    public GameObject bossHealthBarUI;
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
            // Correct answer
            if (clickedRobot.CompareTag("Boss"))
            {
                BossHealth bossHealth = clickedRobot.GetComponent<BossHealth>();
                if (bossHealth != null)
                {
                    bossHealth.TakeDamage();
                    // Add reduced time for boss hits (optional)
                    if (timer != null)
                    {
                        timer.AddTime(3f); // Give 3 seconds per correct answer instead of 15
                    }
                }
            }
            else
            {
                // Handle loot for regular enemies on correct answer
                LootBag loot = clickedRobot.GetComponent<LootBag>();
                if (loot != null)
                {
                    loot.InstantiateLoot(clickedRobot.transform.position);
                }
                else
                {
                    Debug.LogWarning("No Loot Dropped!");
                }

                ExplodeRobot(clickedRobot);
                // Add 15 seconds to the timer for regular enemies
                if (timer != null)
                {
                    timer.AddTime(15f);
                }
            }
        }
        else
        {
            // Incorrect answer
            if (clickedRobot.CompareTag("Boss"))
            {
                // Make the boss throw an axe when answer is incorrect
                BossAxeThrower axeThrower = clickedRobot.GetComponent<BossAxeThrower>();
                if (axeThrower != null)
                {
                    // Force the boss to throw an axe immediately
                    axeThrower.ThrowAxeAtPlayer();
                }
            }
            // For incorrect answers, don't destroy regular robots
            // Only bosses have consequences for wrong answers
        }
    }

    // Method to destroy (explode) the robot
    public void ExplodeRobot(GameObject robot)
    {
        if (robot == null) return;
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, robot.transform.position, Quaternion.identity);
            Animator explosionAnimator = explosion.GetComponent<Animator>();
            if (explosionAnimator != null)
            {
                explosionAnimator.SetTrigger("OnEnemyDeath");
            }
            AudioSource audioSource = explosion.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
        Destroy(robot);
        // Only register death for non-boss enemies
        if (!robot.CompareTag("Boss"))
        {
            EnemyTracker.Instance?.RegisterEnemyDeath();
        }
    }
}