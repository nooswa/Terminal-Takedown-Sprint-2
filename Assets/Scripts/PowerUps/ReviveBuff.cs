using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/ReviveBuff")]
public class ReviveBuff : PowerupEffect
{
    public override void Apply(GameObject target)
    {
        HealthManager healthManager = target.GetComponent<HealthManager>();
        if (healthManager != null)
        {
            bool gotRevive = healthManager.TryAddRevive();
            if (!gotRevive)
            {
                Debug.Log("Already had revive");
            }
            else
            {
                Debug.Log("+1 Revive");
            }
        }
    }
}