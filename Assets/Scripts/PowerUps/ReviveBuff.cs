using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/ReviveBuff")]
public class ReviveBuff : PowerupEffect
{
    public override void Apply(GameObject target)
    {
        HealthManager healthManager = target.GetComponent<HealthManager>(); //access to healthmanager
        if (healthManager != null)
        {
            bool gotRevive = healthManager.TryAddRevive();
            if (!gotRevive)//debugs to know whether revive is working as intended
            {
                Debug.Log("Already had revive"); //if revive already has
            }
            else
            {
                Debug.Log("+1 Revive"); //adds revive if not have
            }
        }
    }
}