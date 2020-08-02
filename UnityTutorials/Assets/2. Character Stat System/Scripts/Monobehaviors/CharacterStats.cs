using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    #region Character Variables
    [Header("Character Customizable")]
    public CharacterStats_SO characterStats;
    public CharacterInventory characterInventory;
    public GameObject characterWeaponSlot;
    public ItemPickUp startWeapon;  // Set Manually ATM regardless of checkbox (Place in inspector)
    public bool isPlayer;
    [Header("Enemy Dependency")]
    public Animator animator;
    public AudioSource audioSoruce;
    public string[] animationClips;

    public HealthBar healthBar;

    #endregion

    #region Events
    public Events.EventPlayerTakeDamage OnPlayerTakeDamage;
    #endregion

    #region Constructors
    public CharacterStats()
    {
        characterInventory = CharacterInventory.instance;
    }
    #endregion

    #region Initializations
    //private void Awake()
    //{
    //    //UIManager.Instance.OnPlayerTakeDamage.AddListener(OnPlayerTakeDamage);
    //}
    private void Start()
    {
        if(!characterStats.setManually)
        {
            characterStats.playerID = PlayerManager.Instance.playerCount; // Is this really where we should put this initalization?

            characterStats.maxHealth = 100;
            characterStats.currentHealth = 50;

            characterStats.maxMana = 25;
            characterStats.currentMana = 10;

            characterStats.maxWealth = 500;
            characterStats.currentWealth = 0;

            //characterStats.currentDamage = characterStats.baseDamage;   // To remove later?

            characterStats.baseResistance = 0;
            characterStats.currentResistance = 0;

            characterStats.maxEncumbrance = 50f;
            characterStats.currentEncumbrance = 0f;

            characterStats.characterExperience = 0;
            characterStats.characterLevel = 0;
        }
        if (gameObject.tag == "Enemy")
        {
            characterStats.currentHealth = characterStats.maxHealth;
            characterInventory = null;
            characterWeaponSlot = null;
            animator = transform.parent.GetComponent<Animator>();   // Move to outside of enemy conditional once restructured
            audioSoruce = transform.parent.gameObject.GetComponent<AudioSource>();
        }
        else if (gameObject.tag == "Player")
        {
            //Debug.Log("[Character Stats] SETTING INITIAL WEAPON"); To be traded out when we can swap out character equipment. (GC PRE LOBBY SCREEN)
            EquipWeapon(startWeapon);
            healthBar = GameObject.Find("PlayerUI").transform.GetChild(0).GetComponent<HealthBar>();
        }

        healthBar.SetMaxHealth(characterStats.maxHealth);
    }
    #endregion

    #region Updates
    //private void Update()
    //{
    //    if(Input.GetButtonDown("Save"))
    //    {
    //        characterStats.SaveCharacterData();
    //    }
    //}
    #endregion

    #region Stat Increasers
    // Wrappers
    public void ApplyHealth(int healthAmount)
    {
        Debug.LogWarningFormat("Health Healed: +" + healthAmount);
        characterStats.ApplyHealth(healthAmount);
        healthBar.HealDamage(healthAmount);
    }

    public void ApplyMana(int manaAmount)
    {
        characterStats.ApplyMana(manaAmount);
    }

    public void ApplyWealth(int wealthAmount)
    {
        characterStats.ApplyWealth(wealthAmount);
    }

    public void EquipWeapon(ItemPickUp weapon)
    {
        characterStats.EquipWeapon(weapon, characterInventory, characterWeaponSlot);
    }
    #endregion

    #region Stat Reducers
    public void TakeDamage(int amount)
    {
        if(gameObject.tag == "Enemy")       // REMOVE THESE CLAUSES IN THE FUTURE TESTING ONLY
        {
            characterStats.TakeDamage(amount);
            animator.Play(animationClips[(int)AnimationsList.TAKE_DAMAGE]);
            audioSoruce.Play();
            healthBar.TakeDamage(amount);
        }
        else
        {
            //characterStats.TakeDamage(amount);  // Player damage testing
            //OnPlayerTakeDamage.Invoke(amount); -------------------------------------
            if (isPlayer)
                healthBar.TakeDamage(amount);
        }
    }

    public void TakeMana(int amount)
    {
        characterStats.TakeMana(amount);
    }

    #endregion

    #region Weapon and Armor Change
    public void ChangeWeapon(ItemPickUp weaponPickUp)
    {
        if(!characterStats.UnEquipWeapon(weaponPickUp, characterInventory, characterWeaponSlot))
        {
            characterStats.EquipWeapon(weaponPickUp, characterInventory, characterWeaponSlot);
        }
    }

    public void ChangeArmor(ItemPickUp armorPickUp)
    {
        if(!characterStats.UnEquipArmor(armorPickUp, characterInventory))
        {
            characterStats.EquipArmor(armorPickUp, characterInventory);
        }
    }
    #endregion

    #region Level Up and Death
    public void LevelUp()
    {
        characterStats.LevelUp();
    }

    public float GetDeathTimer()
    {
        return characterStats.GetDeathTimer();
    }

    public void SetDeathTimer(float characterDeathTimer)
    {
        characterStats.SetDeathTimer(characterDeathTimer);
    }
    #endregion

    #region Reporters
    public int GetPlayerID()
    {
        return characterStats.playerID;
    }

    public int GetHealth()
    {
        return characterStats.currentHealth;
    }

    public ItemPickUp GetCurrentWeapon()
    {
        if(characterStats.weapon != null)
        {
            return characterStats.weapon; // Returns the prefab currently holding
            //return characterStats.weapon
        }
        else
        {
            return null;
        }
    }

    public int GetDamage()
    {
        return characterStats.currentDamage;
    }

    public float GetResistance()
    {
        return characterStats.currentResistance;
    }
    #endregion
}
