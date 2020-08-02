using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

[CreateAssetMenu(fileName = "NewStats", menuName = "Character/Stats", order = 1)]
public class CharacterStats_SO : ScriptableObject
{
    [System.Serializable]
    public class CharacterLevelUps
    {
        public int maxHeatlh;
        public int maxMana;
        public int maxWealth;
        public int baseDamage;
        public float baseResistence;
        public float maxEncumberance;
    }

    #region Fields

    public bool setManually = false;
    public bool saveDataOnClose = false;

    public ItemPickUp weapon { get; private set; }
    public ItemPickUp headArmor { get; private set; }
    public ItemPickUp chestArmor { get; private set; }
    public ItemPickUp handArmor { get; private set; }
    public ItemPickUp legArmor { get; private set; }
    public ItemPickUp footArmor { get; private set; }
    public ItemPickUp misc1 { get; private set; }
    public ItemPickUp misc2 { get; private set; }

    public int playerID = 0;    // IDs will start at 1; 0 means the player was not assigned a ID

    public int maxHealth = 0;
    public int currentHealth = 0;

    public int maxMana = 0;
    public int currentMana = 0;

    public int maxWealth = 0;
    public int currentWealth = 0;

    public int baseDamage = 0;
    public int currentDamage = 0;

    public float baseResistance = 0f;
    public float currentResistance = 0f;

    public float maxEncumbrance = 0f;
    public float currentEncumbrance = 0f;

    public int characterExperience = 0;
    public int characterLevel = 0;

    public float characterDeathTimer = 0.0f;

    public CharacterLevelUps[] characterLevelUps;

    #endregion

    #region Stat Increasers
    public void ApplyHealth(int healthAmount)
    {
        if((currentHealth + healthAmount) > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += healthAmount;
        }
    }

    public void ApplyMana(int manaAmount)
    {
        if((currentMana + manaAmount) > maxMana)
        {
            currentMana = maxMana;
        }
        else
        {
            currentMana += manaAmount;
        }
    }

    public void ApplyWealth(int wealthAmount)
    {
        if((currentWealth + wealthAmount) > maxWealth)
        {
            currentWealth = maxWealth;
        }
        else
        {
            currentWealth += wealthAmount;
        }
    }

    public void EquipWeapon(ItemPickUp weaponPickUp, CharacterInventory characterInventory, GameObject weaponSlot)
    {
        //Debug.Log("[CharacterStats_SO] SETTING CURRENT DAMAGE");
        //Rigidbody newWeapon;
        weapon = weaponPickUp;
        //newWeapon = Instantiate(weaponPickUp.itemDefinition.weaponSlotObject, weaponSlot.transform);
        currentDamage = baseDamage + weapon.itemDefinition.itemAmount;
    }

    public void EquipArmor(ItemPickUp armorPickUp, CharacterInventory characterInventory)
    {
        switch(armorPickUp.itemDefinition.itemArmorSubType)
        {
            case ItemArmorSubType.HEAD:
                //characterInventory.inventoryDisplaySlots[3].sprite = armorPickUp.itemDefinition.itemIcon;
                headArmor = armorPickUp;
                currentResistance += armorPickUp.itemDefinition.itemAmount;
                break;
            case ItemArmorSubType.CHEST:
                chestArmor = armorPickUp;
                currentResistance += armorPickUp.itemDefinition.itemAmount;
                break;
            case ItemArmorSubType.HANDS:
                handArmor = armorPickUp;
                currentResistance += armorPickUp.itemDefinition.itemAmount;
                break;
            case ItemArmorSubType.LEGS:
                legArmor = armorPickUp;
                currentResistance += armorPickUp.itemDefinition.itemAmount;
                break;
            case ItemArmorSubType.BOOTS:
                footArmor = armorPickUp;
                currentResistance += armorPickUp.itemDefinition.itemAmount;
                break;
        }
    }
    #endregion

    #region Stat Reducers
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if(currentHealth <= 0)
        {
            Death();
        }
    }

    public void TakeMana(int amount)
    {
        currentMana -= amount;

        if(currentMana < 0)
        {
            currentMana = 0;
        }
    }

    public bool UnEquipWeapon(ItemPickUp weaponPickUp, CharacterInventory characterInventory, GameObject weaponSlot)
    {
        bool previousWeaponSame = false;

        if (weapon != null)
        {
            if(weapon == weaponPickUp)
            {
                previousWeaponSame = true;
            }

            Destroy(weaponSlot.transform.GetChild(0).gameObject);
            weapon = null;
            currentDamage = baseDamage;
        }

        return previousWeaponSame;
    }

    public bool UnEquipArmor(ItemPickUp armorPickUp, CharacterInventory characterInventory)
    {
        bool previousArmorSame = false;

        switch(armorPickUp.itemDefinition.itemArmorSubType)
        {
            case ItemArmorSubType.HEAD:
                if(headArmor != null)
                {
                    if(headArmor == armorPickUp)
                    {
                        previousArmorSame = true;
                    }

                    currentResistance -= armorPickUp.itemDefinition.itemAmount;
                    headArmor = null;
                }
                break;
            case ItemArmorSubType.CHEST:
                if (chestArmor != null)
                {
                    if (chestArmor == armorPickUp)
                    {
                        previousArmorSame = true;
                    }
                    currentResistance -= armorPickUp.itemDefinition.itemAmount;
                    chestArmor = null;
                }
                break;
            case ItemArmorSubType.HANDS:
                if (handArmor != null)
                {
                    if (handArmor == armorPickUp)
                    {
                        previousArmorSame = true;
                    }

                    currentResistance -= armorPickUp.itemDefinition.itemAmount;
                    handArmor = null;
                }
                break;
            case ItemArmorSubType.LEGS:
                if (legArmor != null)
                {
                    if (legArmor == armorPickUp)
                    {
                        previousArmorSame = true;
                    }

                    currentResistance -= armorPickUp.itemDefinition.itemAmount;
                    legArmor = null;
                }
                break;
            case ItemArmorSubType.BOOTS:
                if (footArmor != null)
                {
                    if (footArmor == armorPickUp)
                    {
                        previousArmorSame = true;
                    }

                    currentResistance -= armorPickUp.itemDefinition.itemAmount;
                    footArmor = null;
                }
                break;
        }

        return previousArmorSame;
    }

    #endregion

    #region Character Level Up and Death
    public void LevelUp()  // Change this back to private when done debugging
    {
        characterLevel += 1;
        // Display level up visulaization and sounds

        maxHealth = characterLevelUps[characterLevel].maxHeatlh;
        maxMana = characterLevelUps[characterLevel].maxMana;
        maxWealth = characterLevelUps[characterLevel].maxWealth;
        baseDamage = characterLevelUps[characterLevel].baseDamage;
        baseResistance = characterLevelUps[characterLevel].baseResistence;
        maxEncumbrance = characterLevelUps[characterLevel].maxEncumberance;
    }

    public float GetDeathTimer()
    {
        return this.characterDeathTimer;
    }

    public void SetDeathTimer(float characterDeathTimer)
    {
        this.characterDeathTimer = characterDeathTimer;
    }

    private void Death()
    {
        Debug.LogFormat("{0} has been slain.", name);
        //if (name == "Avelyn")
        //{
        //    // Call to Game Manager for Death State to Trigger Respawn
        //    // Display the Death Visualizations
        //    // Destroy Game Object
        //}
    }

    #endregion

    #region Save Character Data
    //public void SaveCharacterData()
    //{
    //    saveDataOnClose = true;
    //    EditorUtility.SetDirty(this);
    //}
    #endregion
}
