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

    private bool hasRevive = false; //revive flag (tracks revive have)
    public GameObject reviveIconUI;

    public bool TryAddRevive() //attempt to pick up revive method
    {
        if (hasRevive)
        {
            return false; //already have revive
        }
        else
        {
            hasRevive = true;
            UpdateReviveUI();
            Debug.Log("Obtained revive");
            return true;
        }
    }

    public bool HasRevive() //checks for revive own
    {
        return hasRevive;
    }

    public void UseRevive() //revive usage on death
    {
        if (!hasRevive) return;

        //stuff that happens upon revive trigger!
        hasRevive = false;
        isDead = false;
        healthAmount = 100f;
        healthBar.fillAmount = 1f;
        UpdateReviveUI();
        Debug.Log("Player revived with full health!");
    }

    void Start()
    {
        if (reviveIconUI == null)
        {
            reviveIconUI = GameObject.Find("ReviveIcon"); //searches for reviveicon
        }

        if (playAgain != null)
        {
            playAgain.SetActive(false); // hide Play Again UI at start
        }

        _damageFlash = GetComponent<DamageFlash>();
        UpdateReviveUI();
        shieldAudio = GetComponent<AudioSource>();
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
        if (isDead || isInvulnerable) return;

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

    private void UpdateReviveUI()
    {
        if (reviveIconUI != null)
        {
            reviveIconUI.SetActive(hasRevive);
        }
    }

    // invulnerability flag and shield reference
    private bool isInvulnerable = false;
    public GameObject shield;

    // gives invulnerability and shows shield for duration seconds
    public void GrantInvulnerability(float duration)
    {

        if (shieldPopClip != null && shieldAudio != null) //plays shield noise
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
    
    public AudioClip shieldPopClip; 
    private AudioSource shieldAudio;   // the AudioSource on the player object

}
