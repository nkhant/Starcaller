
using UnityEngine;

public class DestructedRaiseEvent : MonoBehaviour, IDestructible
{
    private MonsterController monster;

    public void Awake()
    {
        monster = GetComponent<MonsterController>();
    }

    public void OnDestruction()
    {
        monster.OnMonsterDeath.Invoke(monster.monsterType, transform.position);
    }
}
