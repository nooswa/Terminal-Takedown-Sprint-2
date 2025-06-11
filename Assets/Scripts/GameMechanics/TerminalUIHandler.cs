using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Threading.Tasks;

// Manages question display and player interaction on a terminal UI
public class TerminalUIHandler : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text questionText;
    public Button[] answerButtons;
    public GameObject terminalUI;

    [Header("Gameplay")]
    public Timer timer;
    public GameObject explosionPrefab;

    private Question currentQuestion;
    private GameObject clickedRobot;
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
        // No loading overlay logic
        var qm = questionManager;
        if (qm == null)
        {
            Debug.LogError("QuestionManager not found. Cannot proceed.");
            return;
        }

        // Wait until questions are loaded
        while (!qm.HasQuestions)
        {
            await Task.Delay(100); // Poll every 100ms
        }

        Debug.Log("Questions are loaded, game is ready!");
    }

    // Called to open terminal UI for a given robot
    public void OpenTerminal(GameObject robot)
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

        questionText.text = currentQuestion.question;

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

                int index = i; // Local copy for closure
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

    public void OnAnswerSelected(int index)
    {
        Time.timeScale = 1f;
        terminalUI?.SetActive(false);

        if (index == currentQuestion.correctAnswerIndex)
        {
            ExplodeRobot(clickedRobot);
            timer?.AddTime(15f);
        }
    }

    private void ExplodeRobot(GameObject robot)
    {
        if (robot == null) return;

        if (explosionPrefab != null)
        {
            var explosion = Instantiate(explosionPrefab, robot.transform.position, Quaternion.identity);

            explosion.GetComponent<Animator>()?.SetTrigger("OnEnemyDeath");
            explosion.GetComponent<AudioSource>()?.Play();
        }

        Destroy(robot);
    }
}