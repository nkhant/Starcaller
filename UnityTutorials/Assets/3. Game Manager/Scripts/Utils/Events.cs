using UnityEngine;
using UnityEngine.Events;

public class Events
{
    [System.Serializable] public class EventFadeComplete : UnityEvent<bool> { }
    //if we want event to contain arguments then we can templatize it
    // current and then previous games states in arguments
    //if something wants to register it via inspector, you need system.serializable (only appear if public)
    [System.Serializable] public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState> { }
    [System.Serializable] public class EventMonsterDeath : UnityEvent<MonsterList, Vector3> { }
    //[System.Serializable] public class EventOnMonsterDeath : UnityEvent<MonsterList, Vector3> { }

    // Level Change Event
    [System.Serializable] public class EventChangeLevel : UnityEvent<string> { }

    // Fireball Projectile
    [System.Serializable] public class EventSpecialAttack : UnityEvent<bool> { }
    [System.Serializable] public class EventFireballCollided : UnityEvent<GameObject, GameObject> { }

    // Player Damaged Event
    [System.Serializable] public class EventPlayerTakeDamage : UnityEvent<int, int> { }
}
