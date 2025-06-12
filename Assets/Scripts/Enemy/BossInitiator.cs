using UnityEngine;

public class BossInitializer : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
        EnemyTracker.Instance.RegisterBossInstance(this.gameObject);

        // Trigger boss music
        if (MusicManager.Instance != null)
        {
            Debug.Log("Calling PlayBossMusic from BossInitializer");
            MusicManager.Instance.PlayBossMusic();
        }
    }

    private void OnEnable()
    {
        // Called when boss is activated in the scene
        MusicManager.Instance?.PlayBossMusic();
    }

}