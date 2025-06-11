using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;
    public GameObject healthBarUI;
    public Image healthBarFill;
    public GameObject explosionPrefab;

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

    public void TakeDamage()
    {
        currentHealth--;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        // Make sure health bar is visible when resetting
        if (healthBarUI != null)
        {
            healthBarUI.SetActive(true);
        }
    }

    private void Die()
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

    private void OnDisable()
    {
        // Ensure boss music stops when boss is disabled
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.StopBossMusic();
        }
    }
}