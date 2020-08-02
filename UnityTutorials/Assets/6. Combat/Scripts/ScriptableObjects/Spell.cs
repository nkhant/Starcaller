using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell.asset", menuName = "Attack/Spell")]
public class Spell : AttackDefinition
{
    public Fireball fireballToFire;
    public float fireballSpeed;

    public void Cast(GameObject caster, Vector3 hotSpot, Transform playerTransform, int layer)
    {
        // Fire Projectile in forward direction
        Fireball fireball = Instantiate(fireballToFire, hotSpot, Quaternion.identity);  // Fireball pool instead of instantiate/destroy use enable/disable?
        fireball.Fire(caster, fireballSpeed, playerTransform, Range);

        // Set Projectile's collision layer
        fireball.gameObject.layer = layer;

        // Listen to Projectile Collided Event
        fireball.OnFireballCollided.AddListener(OnFireballCollided);
    }

    private void OnFireballCollided(GameObject caster, GameObject monster)
    {
        // Method to call after event raised; Attack landed on target, create attack and attack target
        Debug.Log("DAMAGE CALCULATING");
        // Make sure both the caster and target are still alive
        if (caster == null || monster == null)
            return;
        Debug.Log("PLAYER: " + caster.name);
        Debug.Log("MONSTER: " + monster.name);
        // Create the attack
        var casterStats = caster.GetComponent<CharacterStats>();
        var monsterStats = monster.GetComponent<CharacterStats>();

        var attack = CreateAttack(casterStats, monsterStats);

        // Send attack to all attackable behviors of the monster
        var attackables = monster.GetComponentsInChildren(typeof(IAttackable));
        foreach (IAttackable a in attackables)
        {
            a.OnAttack(caster, attack);
        }
    }
}
