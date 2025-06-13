using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    public static EnemyTracker Instance { get; private set; } 
    [SerializeField] private Transform player; //player pos
    public float spawnRadius = 0.5f; //spawn distance
    private int enemyDeathCount = 0; //death count
    private const int BOSS_SPAWN_THRESHOLD = 10;
    private GameObject bossInstance; // Reference to the scene's boss

    private void Awake() //ensures for only 1 instance of enemytracker
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

    public void RegisterBossInstance(GameObject boss) //instance for boss assigner
    {
        bossInstance = boss;
    }

    public void RegisterEnemyDeath() //enemy death count
    {
        enemyDeathCount++;
        if (enemyDeathCount >= BOSS_SPAWN_THRESHOLD && bossInstance != null) //check for boss spawn meets requirements
        {
            SpawnBoss();
            enemyDeathCount = 0;
        }
    }

    public void WipeAllEnemies() //CLEARS THE ENTIRE MAP OF ENEMIES! RIDS THEM!
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) //turn off every enemy
        {
            if (enemy != bossInstance) // Don't destroy the boss itself
            {
                enemy.SetActive(false);
            }
        }
    }

    private void SpawnBoss() //boss spawner!
    {
        if (MusicManager.Instance != null) //makes sure boss music plays
        {
            MusicManager.Instance.PlayBossMusic();
        }

        if (player == null || bossInstance == null) return; //if no player exists or boss instance then returns
        Vector2 offset = Random.insideUnitCircle.normalized * spawnRadius; //boss spawn distance from player
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