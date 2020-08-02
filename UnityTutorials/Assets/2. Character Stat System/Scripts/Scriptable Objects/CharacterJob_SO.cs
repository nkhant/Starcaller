using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerJob", menuName = "Character/Job", order = 1)]
public class CharacterJob_SO : ScriptableObject
{
    [System.Serializable]
    public class JobSkillAttributes // Read only for text/UI displaying
    {
        public string skillName;
        public string skillDescription;
        public string skillSlotNumber;
        public int hpCost;
        public int manaCost;
        public int energyCost;
        public float cooldown;
        public float range;
        public float minDamage;
        public float maxDamage;
        public float criticalMultipier;
        public float criticalChance;
    }

    #region Fields

    public bool setManually = false;
    public bool saveSataOnClose = false;

    // Attack/Skill Movesets 
    //public AttackDefinition basicAttack { get; private set; }
    //public AttackDefinition Skill_1 { get; private set; }
    //public AttackDefinition Skill_2 { get; private set; }
    //public AttackDefinition Skill_3 { get; private set; }
    [Header("Attack Definitions")]
    [SerializeField] private AttackDefinition basicAttack;
    [SerializeField] private AttackDefinition skill_1;
    [SerializeField] private AttackDefinition skill_2;
    [SerializeField] private AttackDefinition skill_3;

    // JobSkillAttribute Definitions
    public JobSkillAttributes[] jobSkillAttributes;  // Change to reflect how many attacks & skills exist for player
    #endregion

    #region Getting & Setting Attack/Skill Definitions
    public AttackDefinition GetBasicAttack()
    {
        return basicAttack;
    }

    public void SetBasicAttack(AttackDefinition basicAttack)
    {
        if(this.basicAttack == basicAttack)
        {
            return;
        }
        if(basicAttack == null)
        {
            Debug.LogError("[PlayerJob_SO] basicAttack is null and has not been set");
        }
        this.basicAttack = basicAttack;
        this.jobSkillAttributes[0].skillSlotNumber = "aa";
    }

    public AttackDefinition GetSkill_1()
    {
        return skill_1;
    }

    public void SetSkill_1(AttackDefinition skill)
    {
        if (this.skill_1 == skill)
        {
            return;
        }
        if (skill == null)
        {
            Debug.LogError("[PlayerJob_SO] Skill_1 is null and has not been set");
        }
        this.skill_1 = skill;
        this.jobSkillAttributes[1].skillSlotNumber = "1";
    }

    public AttackDefinition GetSkill_2()
    {
        return skill_2;
    }

    public void SetSkill_2(AttackDefinition skill)
    {
        if (this.skill_2 == skill)
        {
            return;
        }
        if (skill == null)
        {
            Debug.LogError("[PlayerJob_SO] Skill_2 is null and has not been set");
        }
        this.skill_2 = skill;
        this.jobSkillAttributes[2].skillSlotNumber = "2";
    }

    public AttackDefinition GetSkill_3()
    {
        return skill_3;
    }

    public void SetSkill_3(AttackDefinition skill)
    {
        if (this.skill_3 == skill)
        {
            return;
        }
        if (skill == null)
        {
            Debug.LogError("[PlayerJob_SO] Skill_3 is null and has not been set");
        }
        this.skill_3 = skill;
        this.jobSkillAttributes[3].skillSlotNumber = "3";
    }
    #endregion
}
