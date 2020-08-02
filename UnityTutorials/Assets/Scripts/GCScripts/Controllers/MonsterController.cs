using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour, IDestructible
{
    [Header("Monster Stats")]
    public AudioSource audioSoruce;
    public CharacterStats_SO SO_Reference;
    public CharacterStats enemyStats;
    public bool isDead = false;
    public MonsterList monsterType; // need to be added to character stats eventually
    public LayerMask enemyLayers = 1 << 11;

    //AI--------------------------
    [Header("AI Variables")]
    public float lookRadius = 10.0f;
    //private Vector3 playerTarget;
    NavMeshAgent agent;

    [Header("Events")]
    public Events.EventMonsterDeath OnMonsterDeath;

    private Animator animator;
    private AnimatorStateInfo stateInfo;
    //private MonsterManager monsterManager;

    //Loot Drops
    [SerializeField]
    private int amountOfLootDropped = 3;
    public LootDrop lootDrop;

    // ---------------------- Unity Game Loop Primitive Calls ----------------------
    private void Awake()
    {
        //monsterManager = FindObjectOfType<MonsterManager>(); //same as gameobject.find
        //if monster manager exists we want see if there is  monster death 
        // if there is a monster death then we want to call the function in the listener (in this case 
        // it is OnMonsterDeath in the monster Manager i.e. run the function
        if (MonsterManager.Instance != null)
            OnMonsterDeath.AddListener(MonsterManager.Instance.OnMonsterDeath);
        enemyStats = GetComponent<CharacterStats>();
        enemyStats.characterStats = Object.Instantiate(SO_Reference);
        //OnMonsterDeath.AddListener()
    }

    void Start()
    {
        //monsterManager = GameObject.Find("MonsterManager").GetComponent<MonsterManager>(); // Might be extensive call to change later
        audioSoruce = transform.parent.gameObject.GetComponent<AudioSource>();
        //playerTarget = PlayerManager.Instance.playerWorldPositions[0];  // Harded coded for player 1 atm since no multiplayer implementation yet.
        animator = gameObject.transform.parent.GetComponent<Animator>();
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        agent = gameObject.transform.parent.GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if(PlayerManager.Instance.playerWorldPositions[0] == null)
        {
            return;
        }

        float distance = Vector3.Distance(PlayerManager.Instance.playerWorldPositions[0], transform.position);
        //Debug.Log("MONSTER - distance " + distance);
        animator.SetFloat("playerDistance", distance);
        animator.SetInteger("monsterHP", enemyStats.GetHealth());

        //checks if player is near enemy, if so, enemy will go to player
        if (distance <= lookRadius)
        {
            //Debug.Log(distance);
            if (enemyStats.GetHealth() > 0.0f)
            {
                agent.SetDestination(PlayerManager.Instance.playerWorldPositions[0]);
            }
        }
        else
        {
            //walk back and forth?
        }
        if (enemyStats.GetHealth() <= 0 && isDead == false)    // Mechanim already controls the death animation to be played heres
        {
            isDead = true;
            // Destroy the object
            var destructibles = GetComponents(typeof(IDestructible));
            foreach (IDestructible d in destructibles)
            {
                d.OnDestruction();  // Can customize to stick particles on every part that gets destroyed
            }
        }
    }

    // CHANGE THIS OUT TO BE IN A SEPARATE SCRIPT LATER PLEASE!!! FUNCITONS SIMILAR TO IATTACKABLE.
    // ---------------------- IDestructible interface implementation ----------------------
    void IDestructible.OnDestruction()
    {
        //OnMonsterDeath.Invoke(this.gameObject);                 // Raise OnMonsterDeath Event for the Monster Manager
        //we invoke the event here because we want to state that the monster has died
        //we invoke when we wanna relay that something has happened. (Raise an event)
        //listeners will listen to the event and send the information to the function in the listener
        //in this case since we stated we wanted to run monster manager's function onmonsterdeath on invoke
        //we send the information to it (the manager's function contains the parameters to recieving this function)
        OnMonsterDeath.Invoke(monsterType, transform.position);
        //Debug.Log("[Enemy Controller] Raised ON MONSTER DEATH");
        // Do other cool stuff here on enemy death
        gameObject.GetComponent<Collider>().enabled = false;    // Disable collider component
        //MonsterManager.Instance.RemoveMonsterFromManager();   // Moved to MManager as monster should not dictate what mmanager calls
        Destroy(this.gameObject.transform.parent.gameObject, enemyStats.GetDeathTimer());   // Destory GameObject
    }

    // ---------------------- Debug Methods ----------------------
    private void DropLootOnDeath()
    {
        Debug.Log("Going To Drop Loot");
        while (amountOfLootDropped > 0)
        {
            Debug.Log("Dropping Loot Number: " + amountOfLootDropped);
            lootDrop.LootDropCheck(this.gameObject);
            this.amountOfLootDropped--;
        }
    }
}
