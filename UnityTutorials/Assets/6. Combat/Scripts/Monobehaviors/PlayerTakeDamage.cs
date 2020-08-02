using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour, IAttackable
{
    [SerializeField] private CharacterStats playerStats;

    //public Events.EventPlayerTakeDamage OnPlayerTakeDamage;

    private void Start()
    {
        //OnPlayerTakeDamage.AddListener(PlayerManager.Instance.PlayerTakeDamage);
        playerStats = this.gameObject.transform.parent.gameObject.GetComponent<CharacterStats>();
    }

    public void OnAttack(GameObject attacker, Attack attack)
    {
        PlayerManager.Instance.PlayerTakeDamage(playerStats.GetPlayerID(), attack.Damage);
        //OnPlayerTakeDamage.Invoke(playerStats.GetPlayerID(), attack.Damage);
        //playerStats.TakeDamage(attack.Damage);
    }
}
