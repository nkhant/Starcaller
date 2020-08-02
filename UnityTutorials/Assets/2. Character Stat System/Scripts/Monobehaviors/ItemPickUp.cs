using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    //use to hold information about the item
    public ItemPickUp_SO itemDefinition;
    //use to hold the reference character stats for the item, do not need to drag in the inspector
    public CharacterStats characterStats;

    private CharacterInventory characterInventory;

    private GameObject foundStats;

    #region Constructors
    //public ItemPickUp()
    //{
    //    characterInventory = CharacterInventory.instance;
    //}
    #endregion

    private void Start()
    {
        /* Used to make sure the item has an updated reference
         * If monster drops sword, it will have monster starts.
         * When player picks it up, this code will update to player stats.
         */
       if(characterStats == null)
        {
            characterStats = PlayerManager.Instance.GetPlayerStats(1); // Hard coded for single player atm
            //foundStats = GameObject.FindGameObjectWithTag("Player");
            //characterStats = foundStats.GetComponent<CharacterStats>();
        }
        //itemDefinition.weaponSlotObject = FindObjectOfType<Weapon>;
        //Debug.Log("START ITEM PICK UP: " + characterInventory);
        characterInventory = characterStats.characterInventory;
        //characterInventory = CharacterInventory.instance;
    }

    //stores item to specific inventory(to that specific inventory instance
    private void StoreItemInInventory()
    {
        //Debug.Log(this);
        //Debug.Log(characterInventory);
        characterInventory.StoreItem(this);
    }

    public void UseItem()
    {
        switch (itemDefinition.itemType)
        {
            case ItemTypeDefinitions.HEALTH:
                characterStats.ApplyHealth(itemDefinition.itemAmount);
                    break;
            case ItemTypeDefinitions.MANA:
                characterStats.ApplyMana(itemDefinition.itemAmount);
                    break;
            case ItemTypeDefinitions.WEALTH:
                characterStats.ApplyWealth(itemDefinition.itemAmount);
                    break;
            case ItemTypeDefinitions.WEAPON:        // This should never run because of the below clause in OnTriggerEnter. This should be implemented when we can swapp out character equipment
                characterStats.ChangeWeapon(this);
                    break;
            case ItemTypeDefinitions.ARMOR:
                characterStats.ChangeArmor(this);
                    break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && itemDefinition.itemType != ItemTypeDefinitions.WEAPON)
        {
            if(itemDefinition.isStorable)
            {
                StoreItemInInventory();
            }
            else
            {
                UseItem();
            }
        }
    }
}
