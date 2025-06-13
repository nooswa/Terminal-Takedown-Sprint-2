using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/HealthBuff")]
public class HealthBuff : PowerupEffect
{
    public float amount;
    public override void Apply(GameObject target)
    {
        HealthManager healthManager = target.GetComponent<HealthManager>(); //access to health manager script
        if (healthManager != null)
        {
            healthManager.Heal(amount); //CALLS UPON THE HEAL FUNCTION TO HEAL!
        }
    }
}
