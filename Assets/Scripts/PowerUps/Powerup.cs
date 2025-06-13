using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public PowerupEffect HealthBuff;
    public PowerupEffect ReviveBuff;
    public LootType lootType;
    private void OnTriggerEnter2D(Collider2D collision) //collider to make them interactable with player.
    {
        // check here for player collision
        if (collision.CompareTag("Player"))
        {
            PowerupEffect selectedEffect = null;

            switch (lootType) //checker for loot type to assign effects
            {
                case LootType.Health: //if health
                    selectedEffect = HealthBuff;
                    break;
                case LootType.Revive: //if revive
                    selectedEffect = ReviveBuff;
                    break;
            }

            if (selectedEffect != null)
            {
                selectedEffect.Apply(collision.gameObject); //gives collision to the items
            }

            Destroy(gameObject); //for destroying the object
        }
    }
}
