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
    private PlayerBugShooter bugShooter;

    [System.Obsolete]
    void Start()
    {
        // Find the PlayerBugShooter component in the scene
        bugShooter = FindObjectOfType<PlayerBugShooter>();
    }

    // Method to show terminal
    public void OpenTerminal(GameObject robot)
    {
        Time.timeScale = 0; // Freeze the game

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
            // Correct answer - shoot bug at the target
            if (bugShooter != null && clickedRobot != null)
            {
                bugShooter.ShootBugAtEnemy(clickedRobot.transform, this); // Pass reference to this handler
            }
            else
            {
                // Fallback to direct destruction if bug shooter not available
                HandleCorrectAnswer();
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

    // Fallback method for handling correct answers without bug
    private void HandleCorrectAnswer()
    {
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

            ExplodeRobot(clickedRobot);
            // Add 3 secs to the timer
            if (timer != null)
            {
                timer.AddTime(3f);
            }
        }
    }

    // Method to destroy (explode) the robot - called by PlayerBug when it hits target
    public void ExplodeRobot(GameObject robot)
    {
        if (robot == null)
        {
            return;
        }

        // Handle boss damage
        if (robot.CompareTag("Boss"))
        {
            BossHealth bossHealth = robot.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage();
                // Add reduced time for boss hits
                if (timer != null)
                {
                    timer.AddTime(3f);
                }
                // Don't destroy the boss, just damage it
                return;
            }
        }

        // Handle loot dropping before explosion for regular enemies
        if (!robot.CompareTag("Boss"))
        {
            LootBag loot = robot.GetComponent<LootBag>();
            if (loot != null)
            {
                loot.InstantiateLoot(robot.transform.position);
            }
        }

        // Create explosion effect
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

        // Add time bonus for regular enemies
        if (!robot.CompareTag("Boss") && timer != null)
        {
            timer.AddTime(15f);
        }

        // Destroy the robot
        Destroy(robot);

        // Only register death for non-boss enemies
        if (!robot.CompareTag("Boss"))
        {
            EnemyTracker.Instance?.RegisterEnemyDeath();
        }
    }
}