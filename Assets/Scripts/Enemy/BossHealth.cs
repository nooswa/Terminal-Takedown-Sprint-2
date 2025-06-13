using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 5; //maxhealth for boss
    private int currentHealth; //current health count
    public GameObject healthBarUI; //ui
    public Image healthBarFill; //ui fill
    public GameObject explosionPrefab; //explosion effect

    private void Start()
    {
        currentHealth = maxHealth;

        // Show the health bar when boss spawns and update it to full health
        if (healthBarUI != null)
        {
            healthBarUI.SetActive(true);
        }

        // Initialize the health bar to show full health
        UpdateHealthUI();
    }

    public void TakeDamage() //calls when boss takes damage
    {
        currentHealth--; //loses health and updates
        UpdateHealthUI();

        if (currentHealth <= 0) //checks for boss death criteria
        {
            Die();
        }
    }

    private void UpdateHealthUI() //updates the health ui
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    public void ResetHealth() //resets boss health for new fights
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        // Make sure health bar is visible when resetting
        if (healthBarUI != null)
        {
            healthBarUI.SetActive(true);
        }
    }

    private void Die() //boss death method (what happens when boss dies)
    {
        // Stop boss music and return to previous music
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.StopBossMusic();
        }

        // Hide the health bar when boss dies
        if (healthBarUI != null)
        {
            healthBarUI.SetActive(false);
        }

        // Wipe all enemies when boss dies
        EnemyTracker.Instance?.WipeAllEnemies();

        // Create explosion for boss
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
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

        gameObject.SetActive(false);
    }

    private void OnDisable() //reassures boss music stops playing upon boss disable.
    {
        // Ensure boss music stops when boss is disabled
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.StopBossMusic();
        }
    }
}