using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float healthAmount = 100f;
    public GameObject playAgain;

    private bool isDead = false;
    private DamageFlash _damageFlash;

    public GameObject redFlashOverlay;
    public GameObject criticalHealthText;
    public AudioSource heartbeatSound;


    private bool hasShownCriticalWarning = false;
    private Coroutine flashingCoroutine = null;

    void Start()
    {
        if (playAgain != null)
        {
            playAgain.SetActive(false); // hide Play Again UI at start
        }

        _damageFlash = GetComponent<DamageFlash>();

        if (redFlashOverlay != null)
            redFlashOverlay.SetActive(false);

        if (criticalHealthText != null)
            criticalHealthText.SetActive(false);
    }

    void Update()
    {
        // Death check
        if (healthAmount <= 0 && !isDead)
        {
            isDead = true;
            HandleDeath();
        }

        if (healthAmount <= 30f && !isDead)
        {
            if (flashingCoroutine == null)
            {
                flashingCoroutine = StartCoroutine(FlashCriticalWarning());
            }

            if (heartbeatSound != null && !heartbeatSound.isPlaying)
            {
                heartbeatSound.Play();
            }

            MusicManager.SetVolume(0.3f); // Reduce background subtly
        }
        else
        {
            if (flashingCoroutine != null)
            {
                StopCoroutine(flashingCoroutine);
                flashingCoroutine = null;
            }

            if (heartbeatSound != null && heartbeatSound.isPlaying)
            {
                heartbeatSound.Stop();
            }

            if (redFlashOverlay != null) redFlashOverlay.SetActive(false);
            if (criticalHealthText != null) criticalHealthText.SetActive(false);

            MusicManager.SetVolume(1f); // Restore full volume
        }


        // Manual testing inputs
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TakeDamage(20f); // simulate damage
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Heal(10f); // simulate healing
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        healthAmount -= damage;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100f);
        healthBar.fillAmount = healthAmount / 100f;

        if (_damageFlash != null)
        {
            _damageFlash.CallDamageFlash(); // flash screen on damage
        }
    }

    public void Heal(float healingAmount)
    {
        if (isDead) return;

        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100f);
        healthBar.fillAmount = healthAmount / 100f;
    }

    private void HandleDeath()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayDeathMusic();
        }

        if (playAgain != null)
        {
            playAgain.SetActive(true);
            Time.timeScale = 0f; // pause the game
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // fallback: reload scene
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayBackgroundMusic(true, MusicManager.Instance.backgroundMusic);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void MarkAsDead()
    {
        isDead = true;

        if (playAgain != null)
        {
            playAgain.SetActive(true);
            Time.timeScale = 0f;
        }

        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayDeathMusic();
        }
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;

        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.StopMusic();
        }

        SceneManager.LoadScene(0); // loads menu scene index
    }

    private IEnumerator FlashCriticalWarning()
    {
        while (healthAmount <= 30f && !isDead)
        {
            if (redFlashOverlay != null)
                redFlashOverlay.SetActive(true);

            if (criticalHealthText != null)
                criticalHealthText.SetActive(true);

            yield return new WaitForSeconds(0.2f);

            if (redFlashOverlay != null)
                redFlashOverlay.SetActive(false);

            if (criticalHealthText != null)
                criticalHealthText.SetActive(false);

            yield return new WaitForSeconds(0.2f);
        }

        // Ensure they're hidden when done
        if (redFlashOverlay != null)
            redFlashOverlay.SetActive(false);
        if (criticalHealthText != null)
            criticalHealthText.SetActive(false);
    }
}
