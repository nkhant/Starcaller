using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatAttacks : MonoBehaviour
{
    #region Player Dependencies
    [Header("Automatic Population")]
    [SerializeField] private Animator anim;                           // Animator to control which animation to play when
    [SerializeField] private AnimatorStateInfo stateInfo;             // Used to read which animation is currently playing
    [SerializeField] private float animationNormalizedTime = 0.75f;   // Animation normailzed time to have input buffers
    [SerializeField] private CharacterStats characterStats;  // Player's current stats
    [SerializeField] private CharacterJob characterJob; // Player Job Skill Tree
    [SerializeField] private GameObject weaponSlot;     // Location of weapon (will have different weapons so got generic spot)
    [SerializeField] private Transform spellHotSpot;    // Located to throw spells out from
    [SerializeField] private Transform attackPoint;     // Where attacks register from on the weapon
    [SerializeField] private float attackRange = 0.8f;  // Can edit through inspector
    [SerializeField] private ItemPickUp weapon;   // Could also be AttackDefinition, but Weapons will have their own unique attack definition
    #endregion

    #region Attack Animation Clips
    // Attack Animation Clips
    public const string IDLE = "Sword_F_BattleIdle_01";
    public const string RUN = "Sword_F_Run_01";
    public const string ATT_0 = "Sword_F_Attack_01";
    public const string ATT_1 = "Sword_F_Attack_02";
    public const string COMBO_1 = "Sword_F_Skill_02";
    public const string JUMP = "Sword_F_Jump_01";
    public const string SPECIAL_1 = "Sword_F_Skill_03";
    #endregion

    #region Hit Detection
    [Header("Hit Detection")]
    public LayerMask enemyLayers = 1 << 12;
    #endregion

    #region Animation Hashes
    private int basicAttackHash = Animator.StringToHash("Basic_Attack");
    private int chain_0_hash = Animator.StringToHash("Chain_0");
    private int chain_1_hash = Animator.StringToHash("Chain_1");
    private int special_1_hash = Animator.StringToHash("Special_1");
    #endregion

    #region Events
    [Header("Combat Events")]
    public Events.EventSpecialAttack EventSpecialAttack;
    #endregion

    #region Unity Game Loop (Start/Update/Etc)
    private void Start()
    {
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        characterJob = GetComponent<CharacterJob>();
        weaponSlot = transform.Find("ch_bone/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/Bip001 R Hand/W_R/WeaponSlot").gameObject;
        attackPoint = weaponSlot.transform.GetChild(0).transform.GetChild(0);   // Weapon Slot --> Weapon --> Attack Point
        spellHotSpot = transform.Find("ch_bone/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm/Bip001 L Forearm/Bip001 L Hand/W_L/SpellHotSpot");
        weapon = characterStats.GetCurrentWeapon();
    }

    // Deals wih state transitions and input buffers
    void Update()
    {
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (Input.GetButtonDown("Attack"))  // The standard "Z Z Z" combo input for up to 3 basic attacks
        {
            // Register if attack button is pressed within the frame window
            if (stateInfo.IsName(IDLE) || stateInfo.IsName(RUN) || stateInfo.IsName(JUMP))  // First attack normalized time is 0.36 - 0.54
            {
                anim.Play(ATT_0);
                animationNormalizedTime = 0.54f;
            }
            else if(stateInfo.IsName(SPECIAL_1))
            {
                anim.SetTrigger(basicAttackHash);
                animationNormalizedTime = 0.54f;
            }
            else if (stateInfo.IsName(ATT_0) && (stateInfo.normalizedTime < animationNormalizedTime))   // Second attack normalized time is 0.34 - 0.50
            {
                anim.SetTrigger(chain_0_hash);
                animationNormalizedTime = 0.60f;
            }
            else if (stateInfo.IsName(ATT_1) && (stateInfo.normalizedTime < animationNormalizedTime))
            {
                anim.SetTrigger(chain_1_hash);
                animationNormalizedTime = 0.60f; // Will add if additional move set
            }
        }
        if(Input.GetButtonDown("Special_1"))
        {
            //Debug.Log("SPECIAL");
            if (stateInfo.IsName(IDLE) || stateInfo.IsName(RUN) || stateInfo.IsName(JUMP))  // First attack normalized time is 0.36 - 0.54
            {
                anim.SetTrigger(special_1_hash);
                anim.Play(SPECIAL_1);
                //animationNormalizedTime = 0.70f;
            }
            else if(stateInfo.IsName(COMBO_1) && (stateInfo.normalizedTime < animationNormalizedTime))
            {
                anim.SetTrigger(special_1_hash);
                //anim.Play(SPECIAL_1);
            }
        }
        if (stateInfo.IsName(IDLE) || stateInfo.IsName(RUN))
        {
            anim.ResetTrigger("Basic_Attack");
            anim.ResetTrigger("Chain_0");
            anim.ResetTrigger("Chain_1");
            anim.ResetTrigger("Special_1");
        }
        //Debug.Log(stateInfo.normalizedTime);
    }

    // Used for hit detection and physics calculations Avelyn is a compound collider (ONLY RIGIDBODY IS ON PLAYER/ONLY TRIGGER IS ON THE WEAPON)
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("THIS: " + this.gameObject.name);
        if (other.gameObject.layer == 12) // Layer 12 is our enemy; we can make a layer mask if need additional enemy layers
        {
            //Debug.Log("Name: " + other.gameObject.name);
            BasicAttack();   // Will have an issue with repeat calculations
        }
    }
    #endregion

    #region Attack Methods

    public void BasicAttack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers, QueryTriggerInteraction.Ignore);
        // Damage all enemies in range
        foreach (Collider enemy in hitEnemies)
        {
            var attack = characterJob.GetBasicAttack().CreateAttack(characterStats, enemy.gameObject.GetComponent<CharacterStats>());
            var attackables = enemy.gameObject.GetComponentsInChildren(typeof(IAttackable));
            foreach (IAttackable a in attackables)
            {
                a.OnAttack(this.gameObject, attack);
            }
        }
    }

    public void Skill_1()
    {
        var skillType = characterJob.GetSkill_1();

        if(skillType is Spell)
        {
            var spell = (Spell)characterJob.GetSkill_1();
            spell.Cast(this.gameObject, spellHotSpot.position, this.transform, LayerMask.NameToLayer("PlayerSpells"));
        }
        else if(skillType is Weapon)
        {
            Debug.LogError("[PlayerCombatAttack] No attack definition for weapon type implemented.");
        }
        else if (skillType is AttackDefinition)
        {
            BasicAttack();
        }
    }

    public void Skill_2()
    {
        var skillType = characterJob.GetSkill_2();
        if (skillType is Spell)
        {
            var spell = (Spell)characterJob.GetSkill_1();
            spell.Cast(this.gameObject, spellHotSpot.position, this.transform, LayerMask.NameToLayer("Player"));
        }
        else if (skillType is Weapon)
        {
            Debug.LogError("[PlayerCombatAttack] No attack definition for weapon type implemented.");
        }
        else if (skillType is AttackDefinition)
        {
            BasicAttack();
        }
    }

    public void Skill_3()
    {
        var skillType = characterJob.GetSkill_3();
        if (skillType is Spell)
        {
            var spell = (Spell)characterJob.GetSkill_1();
            spell.Cast(this.gameObject, spellHotSpot.position, this.transform, LayerMask.NameToLayer("Player"));
        }
        else if (skillType is Weapon)
        {
            Debug.LogError("[PlayerCombatAttack] No attack definition for weapon type implemented.");
        }
        else if (skillType is AttackDefinition)
        {
            BasicAttack();
        }
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    #endregion
}