using UnityEngine;

public class BossInitializer : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false); //boss disabled on start
        EnemyTracker.Instance.RegisterBossInstance(this.gameObject);

        // Trigger boss music
        if (MusicManager.Instance != null)
        {
            Debug.Log("Calling PlayBossMusic from BossInitializer");
            MusicManager.Instance.PlayBossMusic();
        }
    }

    private void OnEnable() //when boss on plays boss music
    {
        // Called when boss is activated in the scene
        MusicManager.Instance?.PlayBossMusic();
    }

}