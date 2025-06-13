using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Loot : ScriptableObject
{
    //sprites lootnames, drop%, type of loot dropped
    public Sprite lootSprite;
    public string lootName;
    public int dropChance;
    public LootType lootType;

    public Loot(string lootName, int dropChance) // Constructors
    {
        this.lootName = lootName;
        this.dropChance = dropChance;
    }
}
