using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    public static EnemyTracker Instance { get; private set; }
    [SerializeField] private Transform player;
    public float spawnRadius = 0.5f;
    private int enemyDeathCount = 0;
    private const int BOSS_SPAWN_THRESHOLD = 5;
    private GameObject bossInstance; // Reference to the scene's boss

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterBossInstance(GameObject boss)
    {
        bossInstance = boss;
    }

    public void RegisterEnemyDeath()
    {
        enemyDeathCount++;
        if (enemyDeathCount >= BOSS_SPAWN_THRESHOLD && bossInstance != null)
        {
            SpawnBoss();
            enemyDeathCount = 0;
        }
    }

    public void WipeAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (enemy != bossInstance) // Don't destroy the boss itself
            {
                enemy.SetActive(false);
            }
        }
    }

    private void SpawnBoss()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayBossMusic();
        }

        if (player == null || bossInstance == null) return;
        Vector2 offset = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = player.position + new Vector3(offset.x, offset.y, 0f);
        // Activate and position the boss
        bossInstance.transform.position = spawnPos;
        bossInstance.SetActive(true);
        // Reset boss health when respawning
        BossHealth bossHealth = bossInstance.GetComponent<BossHealth>();
        if (bossHealth != null)
        {
            bossHealth.ResetHealth();
            if (bossHealth.healthBarUI != null)
            {
                bossHealth.healthBarUI.SetActive(true);
            }
        }
    }
}