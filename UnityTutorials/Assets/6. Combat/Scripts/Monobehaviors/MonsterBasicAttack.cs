using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBasicAttack : MonoBehaviour
{
    public AttackDefinition monsterAttack;
    public CharacterStats monsterStats;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayers = 1 << 11;

    private void Start()
    {
        attackPoint = this.gameObject.transform.GetChild(0).transform;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(enemyStats.animationClips[(int)AnimationsList.]);
        if (collision.gameObject.layer == 11 && collision.collider.GetType() == typeof(CapsuleCollider))    // CapsuleCollider is our player hitbox and 11 is our Player Layer
        {
            Attack();
        }
    }

    private void Attack()
    {
        Collider[] hitPlayers = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayers, QueryTriggerInteraction.Ignore);

        // Damage the enemies in range
        foreach (Collider player in hitPlayers)
        {
            var attack = monsterAttack.CreateAttack(monsterStats, player.gameObject.GetComponent<CharacterStats>());

            var attackables = player.gameObject.GetComponentsInChildren(typeof(IAttackable));

            // Looks for all attack interfaces to read/write from/to
            foreach (IAttackable attackable in attackables)
            {
                attackable.OnAttack(gameObject, attack);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
