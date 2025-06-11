using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootHover : MonoBehaviour //Animation for loot
{
    public float height = 0.1f; //height of hover
    public float speed = 5f; //speed of hover

    private Rigidbody2D rb;
    private float startY;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startY = rb.position.y;
    }

    void FixedUpdate()
    {
        float newY = startY + Mathf.Sin(Time.time * speed) * height;
        Vector2 newPos = new Vector2(rb.position.x, newY);

        rb.MovePosition(newPos);
    }
}

public class LootBag : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<Loot> lootList = new List<Loot>();

    Loot GetDroppedItem()
    {
        int randomNumber = Random.Range(1, 101); // Random number range set to 1-100
        List<Loot> possibleItems = new List<Loot>();
        foreach (Loot item in lootList)
        {
            if(randomNumber <= item.dropChance) // If random number is less than or equal the dropChance, then you will get the item.
            {
                possibleItems.Add(item);
            }
        }
        if(possibleItems.Count > 0)
        {
            Loot droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            return droppedItem;
        }
        Debug.Log("No loot dropped"); //in case u get no loot.
        return null;
    }

    public void InstantiateLoot(Vector3 spawnPosition)
    {
        Loot droppedItem = GetDroppedItem();
        if(droppedItem != null )
        {
            GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
            lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.lootSprite;

            Powerup powerupScript = lootGameObject.GetComponent<Powerup>();
            if (powerupScript != null)
            {
                powerupScript.lootType = droppedItem.lootType;
            }

            Rigidbody2D rb = lootGameObject.GetComponent<Rigidbody2D>();

            lootGameObject.AddComponent<LootHover>();//inserts loothover to the lootobject
        }
    }
}
