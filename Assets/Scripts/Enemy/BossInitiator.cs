using UnityEngine;

public class BossInitializer : MonoBehaviour
{
    private void Start()
    {
        // Hide the boss at start
        gameObject.SetActive(false);

        // Register with EnemyTracker
        EnemyTracker.Instance.RegisterBossInstance(this.gameObject);
    }
}