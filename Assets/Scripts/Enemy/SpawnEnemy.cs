using UnityEngine;

// Spawns enemies around the player at regular intervals and plays a spatial sound based on proximity.
public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public Transform player; 
    public float spawnRadius = 0.5f;
    public float spawnInterval = 0.5f;

    public AudioClip spawnSound; 
    public float maxVolume = 1f;
    public float minVolume = 0.1f;
    public float maxDistance = 10f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemies();
            timer = 0f;
        }
    }

    void SpawnEnemies()
    {
        if (player == null || enemyPrefab == null) return;

        // Random position within spawnRadius around the player
        Vector2 offset = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = player.position + new Vector3(offset.x, offset.y, 0f);

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        PlaySpawnSound(spawnPos);
    }

    void PlaySpawnSound(Vector3 spawnPosition)
    {
        if (spawnSound == null) return;

        float distance = Vector3.Distance(player.position, spawnPosition);

        // Scaling volume based on distance to the player
        float volume = Mathf.Lerp(maxVolume, minVolume, distance / maxDistance);

        AudioSource.PlayClipAtPoint(spawnSound, spawnPosition, volume);
    }
}
