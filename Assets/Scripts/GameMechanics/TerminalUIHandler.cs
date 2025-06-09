using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private QuestionManager questionManager;  // Assume this is assigned or fetched elsewhere

    private void Awake()
    {
        questionManager = QuestionManager.Instance;
        
        if (questionManager == null)
        {
        Debug.LogError("QuestionManager not found in the scene!");
        }
    }


    // Called to open terminal UI for a given robot
    public void OpenTerminal(GameObject robot)
    {
        Time.timeScale = 0f;  // pause the game

        clickedRobot = robot;
        Debug.Log("questionManager is " + (questionManager == null ? "NULL" : "OK"));
        currentQuestion = questionManager.GetRandomQuestion();

        if (currentQuestion == null)
        {
            Debug.LogError("No question retrieved!");
            return;
        }

        questionText.text = currentQuestion.question;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < currentQuestion.answers.Count)
            {
                answerButtons[i].gameObject.SetActive(true);
                TMP_Text btnText = answerButtons[i].GetComponentInChildren<TMP_Text>();
                btnText.text = currentQuestion.answers[i];

                int index = i; // capture for closure
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

    

    // Called when an answer is selected
    public void OnAnswerSelected(int index)
    {
        Time.timeScale = 1f;  // resume game
        terminalUI.SetActive(false);

        if (index == currentQuestion.correctAnswerIndex)
        {
            ExplodeRobot(clickedRobot);

            if (timer != null)
            {
                timer.AddTime(15f);
            }
        }
    }

    // Handles robot explosion and cleanup
    private void ExplodeRobot(GameObject robot)
    {
        if (robot == null) return;

        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, robot.transform.position, Quaternion.identity);

            Animator animator = explosion.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("OnEnemyDeath");
            }

            AudioSource audioSource = explosion.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }

        Destroy(robot);
    }
}
