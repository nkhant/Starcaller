using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypeDefinitions { HEALTH, WEALTH, MANA, WEAPON, ARMOR, BUFF, EMPTY};    // Pet will be in here later
public enum ItemArmorSubType { NONE, HEAD, CHEST, HANDS, LEGS, BOOTS };

[CreateAssetMenu(fileName = "New Item", menuName = "Spawnable Item/New Pick-up", order = 1)]
public class ItemPickUp_SO : ScriptableObject
{
    public string itemName = "New Item";
    public string itemDescription = "New Description";
    public ItemTypeDefinitions itemType = ItemTypeDefinitions.HEALTH;
    public ItemArmorSubType itemArmorSubType = ItemArmorSubType.NONE;
    public int itemAmount;
    public int SpawnChanceRate = 0;

    public Material itemMaterial = null;
    public Sprite itemIcon = null;
    public GameObject itemSpawnObject = null;
    public Weapon weaponSlotObject = null;
    public Spell spellSlotObject = null;

    public bool isEquipped = false;
    public bool isInteractable = false;
    public bool isStorable = false;
    public bool isUnique = false; //commonly cloned
    public bool isIndestructable = false;
    public bool isQuestItem = false;
    public bool isStackable = false;
    public bool destroyOnUse = false;
    public float itemWeight = 0f; //how heavy item is
}
