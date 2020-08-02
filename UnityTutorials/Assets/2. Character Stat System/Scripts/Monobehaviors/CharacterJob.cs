using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJob : MonoBehaviour
{
    #region Player Container and Overrides
    [Header("Player Job Container")]
    public CharacterJob_SO characterJob;

    [Header("Player Job Automatic Overrides")]
    [SerializeField] private AttackDefinition basicAttack;
    [SerializeField] private AttackDefinition skill_1;
    [SerializeField] private AttackDefinition skill_2;
    [SerializeField] private AttackDefinition skill_3;
    [SerializeField] private int attackCount = 4;
    #endregion

    #region Initalizations
    private void Start()
    {
        if(!characterJob.setManually)
        {
            characterJob.jobSkillAttributes = new CharacterJob_SO.JobSkillAttributes[attackCount];
            if(basicAttack != null)
            {
                //Debug.Log(basicAttack);
                characterJob.SetBasicAttack(basicAttack);
                //characterJob.jobSkillAttributes[0].skillName = basicAttack.name;
                //characterJob.jobSkillAttributes[0].skillDescription = "Add description here.";
                //characterJob.jobSkillAttributes[0].skillSlotNumber = "0";
                //characterJob.jobSkillAttributes[0].hpCost = 0;
                //characterJob.jobSkillAttributes[0].manaCost = 0;
                //characterJob.jobSkillAttributes[0].energyCost = 0;
                //characterJob.jobSkillAttributes[0].cooldown = basicAttack.Cooldown;
                //characterJob.jobSkillAttributes[0].range = basicAttack.Range;
                //characterJob.jobSkillAttributes[0].minDamage = basicAttack.minDamage;
                //characterJob.jobSkillAttributes[0].maxDamage = basicAttack.maxDamage;
                //characterJob.jobSkillAttributes[0].criticalMultipier = basicAttack.criticalMultiplier;
                //characterJob.jobSkillAttributes[0].criticalChance = basicAttack.criticalChance;
            }

            if(skill_1 != null)
            {
                characterJob.SetSkill_1(skill_1);
                //characterJob.jobSkillAttributes[1].skillName = skill_1.name;
                //characterJob.jobSkillAttributes[1].skillDescription = "Add description here.";
                //characterJob.jobSkillAttributes[1].skillSlotNumber = "1";
                //characterJob.jobSkillAttributes[1].hpCost = 0;
                //characterJob.jobSkillAttributes[1].manaCost = 0;
                //characterJob.jobSkillAttributes[1].energyCost = 0;
                //characterJob.jobSkillAttributes[1].cooldown = skill_1.Cooldown;
                //characterJob.jobSkillAttributes[1].range = skill_1.Range;
                //characterJob.jobSkillAttributes[1].minDamage = skill_1.minDamage;
                //characterJob.jobSkillAttributes[1].maxDamage = skill_1.maxDamage;
                //characterJob.jobSkillAttributes[1].criticalMultipier = skill_1.criticalMultiplier;
                //characterJob.jobSkillAttributes[1].criticalChance = skill_1.criticalChance;
            }

            if(skill_2 != null)
            {
                characterJob.SetSkill_2(skill_2);
                //characterJob.jobSkillAttributes[2].skillName = skill_2.name;
                //characterJob.jobSkillAttributes[2].skillDescription = "Add description here.";
                //characterJob.jobSkillAttributes[2].skillSlotNumber = "2";
                //characterJob.jobSkillAttributes[2].hpCost = 0;
                //characterJob.jobSkillAttributes[2].manaCost = 0;
                //characterJob.jobSkillAttributes[2].energyCost = 0;
                //characterJob.jobSkillAttributes[2].cooldown = skill_2.Cooldown;
                //characterJob.jobSkillAttributes[2].range = skill_2.Range;
                //characterJob.jobSkillAttributes[2].minDamage = skill_2.minDamage;
                //characterJob.jobSkillAttributes[2].maxDamage = skill_2.maxDamage;
                //characterJob.jobSkillAttributes[2].criticalMultipier = skill_2.criticalMultiplier;
                //characterJob.jobSkillAttributes[2].criticalChance = skill_2.criticalChance;
            }

            if(skill_3 != null)
            {
                characterJob.SetSkill_3(skill_3);
                //characterJob.jobSkillAttributes[3].skillName = skill_3.name;
                //characterJob.jobSkillAttributes[3].skillDescription = "Add description here.";
                //characterJob.jobSkillAttributes[3].skillSlotNumber = "3";
                //characterJob.jobSkillAttributes[3].hpCost = 0;
                //characterJob.jobSkillAttributes[3].manaCost = 0;
                //characterJob.jobSkillAttributes[3].energyCost = 0;
                //characterJob.jobSkillAttributes[3].cooldown = skill_3.Cooldown;
                //characterJob.jobSkillAttributes[3].range = skill_3.Range;
                //characterJob.jobSkillAttributes[3].minDamage = skill_3.minDamage;
                //characterJob.jobSkillAttributes[3].maxDamage = skill_3.maxDamage;
                //characterJob.jobSkillAttributes[3].criticalMultipier = skill_3.criticalMultiplier;
                //characterJob.jobSkillAttributes[3].criticalChance = skill_3.criticalChance;
            }
        }
    }
    #endregion

    #region [Wrappers] Get & Set Attacks/Skills
    public AttackDefinition GetBasicAttack()
    {
        return characterJob.GetBasicAttack();
    }

    public void SetBasicAttack(AttackDefinition basicAttack)
    {
        characterJob.SetBasicAttack(basicAttack);
    }

    public AttackDefinition GetSkill_1()
    {
        return characterJob.GetSkill_1();
    }

    public void SetSkill_1(AttackDefinition skill)
    {
        characterJob.SetSkill_1(skill);
    }

    public AttackDefinition GetSkill_2()
    {
        return characterJob.GetSkill_2();
    }

    public void SetSkill_2(AttackDefinition skill)
    {
        characterJob.SetSkill_2(skill);
    }

    public AttackDefinition GetSkill_3()
    {
        return characterJob.GetSkill_3();
    }

    public void SetSkill_3(AttackDefinition skill)
    {
        characterJob.SetSkill_3(skill);
    }
    #endregion

    #region [Wrappers] Get & Set JobSkillAttributes

    // TO WRITE LATER IF ACTUALLY REQUIRED

    #endregion

    #region [Wrappers] Reporters

    public CharacterJob_SO.JobSkillAttributes GetJobSkillAttributes(int index)
    {
        return characterJob.jobSkillAttributes[index];
    }

    #endregion
}
