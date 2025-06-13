using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Threading.Tasks;

// Handles displaying questions, receiving player input, and managing robot interactions (including bosses)
public class TerminalUIHandler : MonoBehaviour
{
    [Header("UI Elements")] //all ui
    public TMP_Text questionText;
    public Button[] answerButtons;
    public GameObject terminalUI;
    public GameObject bossHealthBarUI;

    [Header("Gameplay")] //all gameplay based objects
    public Timer timer;
    public GameObject explosionPrefab;

    private Question currentQuestion;
    private GameObject clickedRobot;
    private PlayerBugShooter bugShooter;
    private QuestionManager _questionManager;

    private QuestionManager questionManager
    {
        get
        {
            if (_questionManager == null)
            {
                _questionManager = QuestionManager.Instance ?? FindAnyObjectByType<QuestionManager>();
                if (_questionManager == null)
                {
                    Debug.LogError("QuestionManager NOT found in the scene or via singleton!");
                }
            }
            return _questionManager;
        }
    }

    private async void Start() 
    {
        bugShooter = FindObjectOfType<PlayerBugShooter>(); //bug shooter reference

        var qm = questionManager; 
        if (qm == null) //debugs for if null
        {
            Debug.LogError("QuestionManager not found. Cannot proceed.");
            return;
        }

        while (!qm.HasQuestions)  //debugs for if questions exist after waiting for them to load
        {
            await Task.Delay(100);
        }

        Debug.Log("Questions are loaded, game is ready!");
    }

    public void OpenTerminal(GameObject robot) //when player intereacts with a robot, it opens the terminal
    {
        Time.timeScale = 0f;
        clickedRobot = robot;

        if (questionManager == null || !questionManager.HasQuestions)
        {
            Debug.LogError("Cannot open terminal. QuestionManager is null or has no questions.");
            return;
        }

        currentQuestion = questionManager.GetRandomQuestion();
        if (currentQuestion == null)
        {
            Debug.LogError("No question retrieved!");
            return;
        }

        questionText.text = currentQuestion.question;//questions

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < currentQuestion.answers.Length)
            {
                var btn = answerButtons[i];
                if (btn == null) continue;

                TMP_Text btnText = btn.GetComponentInChildren<TMP_Text>();
                if (btnText == null) continue;

                btn.gameObject.SetActive(true);
                btnText.text = currentQuestion.answers[i];

                int index = i; //index for correct answer
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => OnAnswerSelected(index));
            }
            else if (answerButtons[i] != null)
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }

        terminalUI?.SetActive(true);
    }

    public void OnAnswerSelected(int index) //method for when player answers question
    {
        Time.timeScale = 1f; //continues timer.
        terminalUI?.SetActive(false);

        if (index == currentQuestion.correctAnswerIndex)//correct answer is chosen
        {
            if (bugShooter != null && clickedRobot != null)
            {
                bugShooter.ShootBugAtEnemy(clickedRobot.transform, this); // Use bug shooter if available
            }
            else
            {
                HandleCorrectAnswer(); // Fallback if bug shooter not found
            }
        }
        else //incorrrect answer chosen,
        {
            if (clickedRobot != null && clickedRobot.CompareTag("Boss"))
            {
                BossAxeThrower axeThrower = clickedRobot.GetComponent<BossAxeThrower>(); //access to boss axe throw
                axeThrower?.ThrowAxeAtPlayer(); //cals axethrow method if it is boss.
            }
        }
    }

    private void HandleCorrectAnswer() //for handling correct answer
    {
        if (clickedRobot == null) return;

        if (clickedRobot.CompareTag("Boss"))
        {
            BossHealth bossHealth = clickedRobot.GetComponent<BossHealth>(); //access to bosshealth
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(); //calls method to damage boss
                timer?.AddTime(3f); //gives u extra time
            }
        }
        else
        {
            LootBag loot = clickedRobot.GetComponent<LootBag>(); //access to lootbag!
            loot?.InstantiateLoot(clickedRobot.transform.position); //FOR LOOT TO DROP UPON KILL

            ExplodeRobot(clickedRobot); //explode robot 
            timer?.AddTime(15f); //gives player time
        }
    }

    public void ExplodeRobot(GameObject robot) //robot explodes
    {
        if (robot == null) return;

        if (robot.CompareTag("Boss"))
        {
            BossHealth bossHealth = robot.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(); //boss gets damaged
                timer?.AddTime(3f);
                return; // Do not destroy the boss
            }
        }

        LootBag loot = robot.GetComponent<LootBag>();
        loot?.InstantiateLoot(robot.transform.position); //loot drops for normal robot kills

        if (explosionPrefab != null)
        {
            var explosion = Instantiate(explosionPrefab, robot.transform.position, Quaternion.identity);
            explosion.GetComponent<Animator>()?.SetTrigger("OnEnemyDeath");
            explosion.GetComponent<AudioSource>()?.Play();
        }

        timer?.AddTime(15f);
        Destroy(robot);

        if (!robot.CompareTag("Boss")) //boss checker.
        {
            EnemyTracker.Instance?.RegisterEnemyDeath();
        }
    }
}