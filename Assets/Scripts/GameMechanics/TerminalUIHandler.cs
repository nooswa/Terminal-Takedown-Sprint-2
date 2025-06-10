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

    // Backing field for lazy getter
    private QuestionManager _questionManager;

    // Lazy property to get QuestionManager instance or find in scene
    private QuestionManager questionManager
    {
        get
        {
            if (_questionManager == null)
            {
                _questionManager = QuestionManager.Instance;
                if (_questionManager == null)
                {
                    _questionManager = FindAnyObjectByType<QuestionManager>();
                }
                if (_questionManager == null)
                {
                    Debug.LogError("QuestionManager NOT found in the scene or via singleton!");
                }
            }
            return _questionManager;
        }
    }

    private void Start()
    {
        // Trigger the getter to cache the reference early
        var qm = questionManager;
    }

    // Called to open terminal UI for a given robot
    public void OpenTerminal(GameObject robot)
    {
        Time.timeScale = 0f;

        clickedRobot = robot;

        if (questionManager == null)
        {
            Debug.LogError("questionManager is NULL");
            return;
        }

        if (!QuestionManager.Instance.HasQuestions)
    {
        Debug.LogError("No questions available! Cannot open terminal.");
        return;
    }

        currentQuestion = questionManager.GetRandomQuestion();

        if (currentQuestion == null)
        {
            Debug.LogError("No question retrieved!");
            return;
        }

        if (questionText == null)
        {
            Debug.LogError("questionText is NULL");
            return;
        }

        questionText.text = currentQuestion.question;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < currentQuestion.answers.Length)
            {
                if (answerButtons[i] == null)
                {
                    Debug.LogError($"answerButtons[{i}] is NULL");
                    continue;
                }

                TMP_Text btnText = answerButtons[i].GetComponentInChildren<TMP_Text>();
                if (btnText == null)
                {
                    Debug.LogError($"TMP_Text is missing in button {i}");
                    continue;
                }

                answerButtons[i].gameObject.SetActive(true);
                btnText.text = currentQuestion.answers[i];

                int index = i;
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
            }
            else if (answerButtons[i] != null)
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }

        if (terminalUI == null)
        {
            Debug.LogError("terminalUI is NULL");
            return;
        }

        terminalUI.SetActive(true);
    }

    public void OnAnswerSelected(int index)
    {
        Time.timeScale = 1f;

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
