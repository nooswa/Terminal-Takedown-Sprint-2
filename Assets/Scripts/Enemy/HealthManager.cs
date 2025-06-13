using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class HealthManager : MonoBehaviour
{
    public Image healthBar; //health ref
    public float healthAmount = 100f; //100hp start
    public GameObject playAgain; //play ui menu

    private bool isDead = false; //flags to keep track
    private bool isInvulnerable = false;
    private bool hasRevive = false;

    //ui, red flash, heartbeet sound, crithealth text
    public GameObject reviveIconUI; 
    public GameObject redFlashOverlay;
    public GameObject criticalHealthText;
    public AudioSource heartbeatSound;

    //shield vis, sound of shield pops
    public GameObject shield;
    public AudioClip shieldPopClip;

    //shield audio, damage flash, tracker for flash
    private AudioSource shieldAudio;
    private DamageFlash _damageFlash;
    private Coroutine flashingCoroutine = null;

    void Start()
    {
        _damageFlash = GetComponent<DamageFlash>(); //component grab
        shieldAudio = GetComponent<AudioSource>();

        if (reviveIconUI == null) //finds revive icon in the scene to assign
            reviveIconUI = GameObject.Find("ReviveIcon");

        if (playAgain != null) //hides playagain menu
            playAgain.SetActive(false);

        if (redFlashOverlay != null) //sets flash overlay to false from start
            redFlashOverlay.SetActive(false);

        if (criticalHealthText != null) //sets criticalhealth text to false from start
            criticalHealthText.SetActive(false);

        UpdateReviveUI(); //updates revive icon whenever revive have
    }

    void Update()
    {
        if (healthAmount <= 0 && !isDead) //if health is below or at 0 and ur not dead
        {
            if (hasRevive) //if player has revive uses it
            {
                UseRevive();
            }
            else //if not, they die.
            {
                isDead = true;
                HandleDeath();
            }
        }

        if (healthAmount <= 30f && !isDead) //if player health is low but not dead, flashes with heartbeat sound warning death.
        {
            if (flashingCoroutine == null)
                flashingCoroutine = StartCoroutine(FlashCriticalWarning());

            if (heartbeatSound != null && !heartbeatSound.isPlaying)
                heartbeatSound.Play();

            MusicManager.SetVolume(0.3f);
        }
        else //else if not low, stops the flashes and heart beat sound
        {
            if (flashingCoroutine != null)
            {
                StopCoroutine(flashingCoroutine);
                flashingCoroutine = null;
            }

            if (heartbeatSound != null && heartbeatSound.isPlaying)
                heartbeatSound.Stop();

            if (redFlashOverlay != null)
                redFlashOverlay.SetActive(false);

            if (criticalHealthText != null)
                criticalHealthText.SetActive(false);

            MusicManager.SetVolume(1f);
        }

        if (Input.GetKeyDown(KeyCode.Return)) TakeDamage(20f); //testers for testing damage taken and health heal in game.
        if (Input.GetKeyDown(KeyCode.Space)) Heal(10f);
    }

    public void TakeDamage(float damage) //take damage method for player
    {
        if (isDead || isInvulnerable) return;

        healthAmount -= damage;//updates health
        healthAmount = Mathf.Clamp(healthAmount, 0, 100f);
        healthBar.fillAmount = healthAmount / 100f; //updates ui

        if (_damageFlash != null) //damage flash
            _damageFlash.CallDamageFlash();
    }

    public void Heal(float healingAmount) //for if player heals
    {
        if (isDead) return;

        healthAmount += healingAmount; //adds healing amount set
        healthAmount = Mathf.Clamp(healthAmount, 0, 100f);
        healthBar.fillAmount = healthAmount / 100f; //updates health ui
    }

    private void HandleDeath() //dwhat happens upon player death
    {
        if (MusicManager.Instance != null) //play death music
            MusicManager.Instance.PlayDeathMusic();

        if (playAgain != null) //shows play again ui
        {
            playAgain.SetActive(true);
            Time.timeScale = 0f;
        }
        else //if no play again ui then reload scene
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void RestartGame() //restart game
    {
        Time.timeScale = 1f;
        if (MusicManager.Instance != null)
            MusicManager.Instance.PlayBackgroundMusic(true, MusicManager.Instance.backgroundMusic);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool IsDead() => isDead; //returns if player died

    public void MarkAsDead() //opens playagain ui if player dead = true
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

    public void LoadMainMenu() //main menu loader and stops all settings
    {
        Time.timeScale = 1f;
        if (MusicManager.Instance != null)
            MusicManager.Instance.StopMusic();

        SceneManager.LoadScene(0);
    }

    private IEnumerator FlashCriticalWarning() //flash method for while health is low and not dead
    {
        while (healthAmount <= 30f && !isDead)
        {
            if (redFlashOverlay != null) redFlashOverlay.SetActive(true);
            if (criticalHealthText != null) criticalHealthText.SetActive(true);
            yield return new WaitForSeconds(0.2f);

            if (redFlashOverlay != null) redFlashOverlay.SetActive(false);
            if (criticalHealthText != null) criticalHealthText.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }

        if (redFlashOverlay != null) redFlashOverlay.SetActive(false);
        if (criticalHealthText != null) criticalHealthText.SetActive(false);
    }

    public bool TryAddRevive()//if player doesnt already have a revive, then give them one.
    {
        if (hasRevive) return false; //if already has then returns.

        hasRevive = true;
        UpdateReviveUI(); //updates revive icon.
        Debug.Log("Obtained revive");
        return true;
    }

    public bool HasRevive() => hasRevive; //returns if player already has

    public void UseRevive() //userevive gives player back to full health and death escape.
    {
        if (!hasRevive) return;

        hasRevive = false;
        isDead = false;
        healthAmount = 100f;
        healthBar.fillAmount = 1f;
        UpdateReviveUI();
        Debug.Log("Player revived with full health!");
    }

    private void UpdateReviveUI() //for updating revive icon
    {
        if (reviveIconUI != null)
            reviveIconUI.SetActive(hasRevive);
    }

    public void GrantInvulnerability(float duration) //for invulnerability with the shield powerup.
    {
        if (shieldPopClip != null && shieldAudio != null)
            shieldAudio.PlayOneShot(shieldPopClip);
        if (shield != null)
            shield.SetActive(true);

        isInvulnerable = true;
        StartCoroutine(InvulnerabilityCoroutine(duration));
    }

    private IEnumerator InvulnerabilityCoroutine(float duration) //turns off invincibility after a bit.
    {
        yield return new WaitForSeconds(duration);
        isInvulnerable = false;
        if (shield != null)
            shield.SetActive(false);
    }
}

