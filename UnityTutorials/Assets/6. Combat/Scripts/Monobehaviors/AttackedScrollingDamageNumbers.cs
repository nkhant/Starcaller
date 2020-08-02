using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class AttackedScrollingDamageNumbers : MonoBehaviour, IAttackable
{
    public DamageNumbers damageNumbers;
    public Color32 basicDamageNumbersColor = new Color32(241, 217, 15, 255);
    public VertexGradient basicDamageNumbersGradient = new VertexGradient(new Color32(255, 255, 255, 255), new Color32(255, 255, 255, 255), 
        new Color32(200, 0, 255, 255), new Color32(200, 0, 255, 255));
    public Color32 criticalDamageNumbersColor = new Color32(214, 159, 245, 255);
    public VertexGradient criticalDamageNumbersGradient = new VertexGradient(new Color32(255, 255, 255, 255), new Color32(255, 255, 255, 255),
        new Color32(200, 0, 255, 255), new Color32(200, 0, 255, 255));

    private float fontSize = 60.0f;

    private Vector3 placeInFrontOfMonster;
    public void OnAttack(GameObject attacker, Attack attack)
    {
        var attackDamageNumber = attack.Damage.ToString();

        placeInFrontOfMonster = transform.position;
        placeInFrontOfMonster.z += -5.0f;

        var scrollingDamageNumber = Instantiate(damageNumbers, placeInFrontOfMonster, Quaternion.identity);
        scrollingDamageNumber.SetDamageNumber(attackDamageNumber);

        //color changes for type of attack type hit (normal, crit, magic(?), healing)
        if (attack.IsCritical)
        {
            //color changes to purple
            scrollingDamageNumber.SetColor(criticalDamageNumbersColor);
            scrollingDamageNumber.SetGradient(criticalDamageNumbersGradient);
            scrollingDamageNumber.IsCritical(true);
            scrollingDamageNumber.SetFontSize(fontSize);
        }
        else
        {
            scrollingDamageNumber.SetColor(basicDamageNumbersColor);
            scrollingDamageNumber.SetGradient(basicDamageNumbersGradient);
        }


    }
}
