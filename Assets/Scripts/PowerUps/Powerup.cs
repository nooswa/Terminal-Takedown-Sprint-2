using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public PowerupEffect HealthBuff;
    public PowerupEffect ReviveBuff;
    public LootType lootType;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // check here for player collision
        if (collision.CompareTag("Player"))
        {
            PowerupEffect selectedEffect = null;

            switch (lootType) //checker for loot type
            {
                case LootType.Health:
                    selectedEffect = HealthBuff;
                    break;
                case LootType.Revive:
                    selectedEffect = ReviveBuff;
                    break;
            }

            if (selectedEffect != null)
            {
                selectedEffect.Apply(collision.gameObject);
            }

            Destroy(gameObject);
        }
    }
}
