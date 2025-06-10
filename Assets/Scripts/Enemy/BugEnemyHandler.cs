using UnityEngine;

public class BugEnemyHandler : MonoBehaviour
{
    [Tooltip("Seconds of invulnerability granted on death")]
    public float invulDuration = 3f;

    private void OnDestroy()
    {
        // when this enemy is destroyed, gives the player invulnerability
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            var hm = player.GetComponent<HealthManager>();
            if (hm != null)
                hm.GrantInvulnerability(invulDuration);
        }
    }
}
