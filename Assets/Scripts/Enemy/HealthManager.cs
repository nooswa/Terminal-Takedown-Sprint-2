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

    // Revive System
    private bool hasRevive = false;
    public GameObject reviveIconUI;

    // Shield System
    private bool isInvulnerable = false;
    public GameObject shield;
    public AudioClip shieldPopClip;
    private AudioSource shieldAudio;

    // Critical Health Warning System
    public GameObject redFlashOverlay;
    public GameObject criticalHealthText;
    public AudioSource heartbeatSound;
    private Coroutine flashingCoroutine = null;

    void Start()
    {
        if (reviveIconUI == null)
        {
            reviveIconUI = GameObject.Find("ReviveIcon");
        }

        if (playAgain != null)
        {
            playAgain.SetActive(false);
        }

        _damageFlash = GetComponent<DamageFlash>();
        shieldAudio = GetComponent<AudioSource>();
        UpdateReviveUI();

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
            if (hasRevive)
            {
                UseRevive();
            }
            else
            {
                isDead = true;
                HandleDeath();
            }
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

            MusicManager.SetVolume(0.3f);
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

            MusicManager.SetVolume(1f);
        }

        // Manual testing inputs
        if (Input.GetKeyDown(KeyCode.Return)) TakeDamage(20f);
        if (Input.GetKeyDown(KeyCode.Space)) Heal(10f);
    }

    public void TakeDamage(float damage)
    {
        if (isDead || isInvulnerable) return;

        healthAmount -= damage;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100f);
        healthBar.fillAmount = healthAmount / 100f;

        if (_damageFlash != null) _damageFlash.CallDamageFlash();
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
            MusicManager.Instance.PlayDeathMusic();

        if (playAgain != null)
        {
            playAgain.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        if (MusicManager.Instance != null)
            MusicManager.Instance.PlayBackgroundMusic(true, MusicManager.Instance.backgroundMusic);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool IsDead() => isDead;

    public void MarkAsDead()
    {
        isDead = true;

        if (playAgain != null)
        {
            playAgain.SetActive(true);
            Time.timeScale = 0f;
        }

        if (MusicManager.Instance != null)
            MusicManager.Instance.PlayDeathMusic();
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;

        if (MusicManager.Instance != null)
            MusicManager.Instance.StopMusic();

        SceneManager.LoadScene(0);
    }

    private void UpdateReviveUI()
    {
        if (reviveIconUI != null)
            reviveIconUI.SetActive(hasRevive);
    }

    public bool TryAddRevive()
    {
        if (hasRevive) return false;

        hasRevive = true;
        UpdateReviveUI();
        Debug.Log("Obtained revive");
        return true;
    }

    public bool HasRevive() => hasRevive;

    public void UseRevive()
    {
        if (!hasRevive) return;

        hasRevive = false;
        isDead = false;
        healthAmount = 100f;
        healthBar.fillAmount = 1f;
        UpdateReviveUI();
        Debug.Log("Player revived with full health!");
    }

    public void GrantInvulnerability(float duration)
    {
        if (shieldPopClip != null && shieldAudio != null)
            shieldAudio.PlayOneShot(shieldPopClip);

        if (shield != null)
            shield.SetActive(true);

        isInvulnerable = true;
        StartCoroutine(InvulnerabilityCoroutine(duration));
    }

    private IEnumerator InvulnerabilityCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        isInvulnerable = false;
        if (shield != null)
            shield.SetActive(false);
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

        if (redFlashOverlay != null)
            redFlashOverlay.SetActive(false);

        if (criticalHealthText != null)
            criticalHealthText.SetActive(false);
    }
}
