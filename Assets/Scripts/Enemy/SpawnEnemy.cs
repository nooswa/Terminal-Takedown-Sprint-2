using UnityEngine;
using UnityEngine.Audio;

// Spawns enemies around the player at regular intervals and plays a spatial sound based on proximity.
public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public Transform player; 
    public float spawnRadius = 0.5f;
    public float spawnInterval = 0.5f;
    public AudioMixerGroup masterMixerGroup;
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

    void SpawnEnemies() //method for spawning enemy
    {
        if (player == null || enemyPrefab == null) return;

        // Random position within spawnRadius around the player
        Vector2 offset = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = player.position + new Vector3(offset.x, offset.y, 0f);

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        PlaySpawnSound(spawnPos);
    }

    void PlaySpawnSound(Vector3 spawnPosition) //method for playing spawn sounds
    {
        if (spawnSound == null) return;

        float distance = Vector3.Distance(player.position, spawnPosition);

        // Scaling volume based on distance to the player
        float volume = Mathf.Lerp(maxVolume, minVolume, distance / maxDistance);

        GameObject spawnN = new GameObject("spawnNoise"); //audiosource holder
        spawnN.transform.position = spawnPosition; //spawns the source at the spawn location

        AudioSource audioS = spawnN.AddComponent<AudioSource>();
        audioS.clip = spawnSound; //sets sound to play
        audioS.volume = volume; //volume based on distance
        audioS.spatialBlend = 1f; //makes sounds 3d (0.0 is full 2d, 1.0 is full 3d.)

        if (masterMixerGroup != null)
        {
            audioS.outputAudioMixerGroup = masterMixerGroup; //for outputs
        }

        audioS.Play(); //plays sound
        
    }
}
