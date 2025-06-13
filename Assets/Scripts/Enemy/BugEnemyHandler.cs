using UnityEngine;

public class BugEnemyHandler : MonoBehaviour
{
    [Tooltip("Seconds of invulnerability granted on death")]
    public float invulDuration = 3f; //invincible time

    private void OnDestroy() 
    {
        // when this enemy is destroyed, gives the player invulnerability
        var player = GameObject.FindWithTag("Player");
        if (player != null) //checks if player exists
        {
            var hm = player.GetComponent<HealthManager>(); //healthmanager access
            if (hm != null)
                hm.GrantInvulnerability(invulDuration); //invincibility granter
        }
    }
}
