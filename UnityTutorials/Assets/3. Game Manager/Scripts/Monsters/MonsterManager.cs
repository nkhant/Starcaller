using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine;

public class MonsterManager : Manager<MonsterManager>
{
    [Header("Monsters to Spawn")]
    public GameObject[] monsterTypes;
    public GameObject[] bossTypes;
    private List<GameObject> monsterList;
    public int monstersLeft = 0;

    private int numberToSpawn;
    private Spawnpoint[] spawnpoints;

    // Monster Manager Events
    [Header("Events")]
    public Events.EventMonsterDeath EventToRaiseLater;

    //Loot Spawns
    public List<LootTable_SO> lootTables;

    //public override void Awake()
    //{
    //    base.Awake();

    //}

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChange);
        InitalizeMonsterManager();
    }

    //private void Update()
    //{
    //    Debug.Log(monstersLeft);
    //    if(!ActiveMonsters())
    //    {
    //        Destroy(this.gameObject);       // Should we even be doing this? Destroy after raising event that no active monsters?
    //    }
    //}

    private void HandleGameStateChange(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if(currentState == GameManager.GameState.RUNNING && previousState == GameManager.GameState.PREGAME)
        {
            InitalizeMonsterManager();
        }
        if (currentState == GameManager.GameState.RUNNING && previousState == GameManager.GameState.RUNNING)
        {
            InitalizeMonsterManager();
        }
        if (currentState == GameManager.GameState.PREGAME && previousState == GameManager.GameState.PAUSED)
        {
            DestroyWave();
        }
    }

    private void InitalizeMonsterManager()
    {
        monstersLeft = 0;
        monsterList = new List<GameObject>();
        spawnpoints = FindObjectsOfType<Spawnpoint>();
        numberToSpawn = spawnpoints.Length;
        SpawnWave();
    }

    private void SpawnWave()
    {
        if(SceneManager.GetActiveScene().name == "Forest_3")    // TO CHANGE LATER HARDCODED BOSS ENDING
        {
            for (int i = 0; i <= numberToSpawn - 1; i++)
            {
                Spawnpoint spawnpoint = spawnpoints[i];
                GameObject monster = Instantiate(bossTypes[0],
                                                spawnpoint.transform.position, Quaternion.LookRotation(Vector3.back, Vector3.up));
                monsterList.Add(monster);
                monstersLeft++;
            }
        }
        else
        {
            for (int i = 0; i <= numberToSpawn - 1; i++)
            {
                Spawnpoint spawnpoint = spawnpoints[i];
                //Spawnpoint spawnpoint = SelectRandomSpawnpoint();
                GameObject monster = Instantiate(SelectRandomMonster(),
                                                spawnpoint.transform.position, Quaternion.LookRotation(Vector3.back, Vector3.up));
                monsterList.Add(monster);
                monstersLeft++;
                //monster.transform.GetChild(0).GetComponent<MonsterController>().OnMonsterDeath.AddListener(OnMonsterDeath);    // Adds the listener to the monster death
            }
        }
    }

    private void DestroyWave()
    {
        foreach (GameObject monster in monsterList)
        {
            if(monster != null)
            {
                Destroy(monster);
            }
        }
    }

    public bool ActiveMonsters()
    {
        return monstersLeft != 0;
    }

    // Monster instantiate helper methods
    private GameObject SelectRandomMonster()
    {
        int monsterIndex = Random.Range(0, monsterTypes.Length);
        return monsterTypes[monsterIndex];
    }

    private Spawnpoint SelectRandomSpawnpoint()
    {
        int pointIndex = Random.Range(0, spawnpoints.Length);
        return spawnpoints[pointIndex];
    }

    public void RemoveMonsterFromManager()
    {
        monstersLeft--;
    }

    // Event Listener Methods
    public void OnMonsterDeath(MonsterList typeOfMonster, Vector3 monsterPosition)
    {
        //OnMonsterDeath.Invoke(monster);
        //Debug.LogWarningFormat("{0} killed at {1}", typeOfMonster, monsterPosition);
        RemoveMonsterFromManager();
        SpawnLoot(typeOfMonster, monsterPosition);
    }

    private void SpawnLoot(MonsterList typeOfMonster, Vector3 monsterPosition)
    {
        ItemPickUp_SO item = GetLoot(typeOfMonster);

        if (item != null)
            Instantiate(item.itemSpawnObject, monsterPosition, Quaternion.identity);
    }

    private ItemPickUp_SO GetLoot(MonsterList typeOfMonster)
    {
        //Lambda - (input-parameters) => expression
        //.Find is list specific function, finds first occurence
        //returns true if the monter in table == the monsterType passed in as argument.
        LootTable_SO monsterLoots = lootTables.Find(monsterType => monsterType.monster == typeOfMonster);

        if (monsterLoots == null)
            return null;

        //sorts from least to most
        monsterLoots.loots.OrderBy(loot => loot.DropChance);

        foreach(LootDefinitions lootItem in monsterLoots.loots)
        {
            bool shouldDrop = Random.value < lootItem.DropChance;

            if (shouldDrop)
                return lootItem.loot;
        }

        return null;
    }
}