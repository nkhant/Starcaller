using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newLootTable", menuName ="New Loot Table", order = 1)]
public class LootTable_SO : ScriptableObject
{
    // Start is called before the first frame update
    public MonsterList monster;
    public LootDefinitions[] loots;
}
