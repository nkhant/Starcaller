using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * PlayerManager: The player manager will instantiate players and monitor/update their stats.
 *           A players ID will refer to their index in the playerStats array.
 */
public class PlayerManager : Manager<PlayerManager>
{
    #region Player Stats

    [Header("Modifiable Stats")]
    public int playerCount;
    public int playerID;
    [SerializeField] private GameObject[] playerPrefabs;

    [Header("Unmodifiable Stats")]
    public Vector3[] playerWorldPositions;  // Consider changing this to a reporter method to call
    [SerializeField] private GameObject[] playerReferences;
    [SerializeField] private CharacterStats[] playerStats;  // ID/Index == the player number (player 1 = 1, etc)
    [SerializeField] private PlayerSpawn playerSpawn;

    #endregion

    #region Events

    public Events.EventPlayerTakeDamage OnPlayerTakeDamage;

    #endregion

    #region Unity Game Loop (Start/Update/Etc)

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "Boot")   // Used for debugging
        {
            playerSpawn = FindObjectOfType<PlayerSpawn>();  // Retrieves the players starting location in a dungeon/scene
            CreatePlayerReferences(playerCount);
        }
        else
        {
            playerWorldPositions = new Vector3[4];
            playerStats = new CharacterStats[4];    // Up to 4 players in a game
            //playerPrefabs = new GameObject[4];      // If need to have different players; add via inspector
            playerReferences = new GameObject[4];
            playerCount = 1;    // Figure out how many players prior to entering the first level some how to update this variable
            //playerSpawn = FindObjectOfType<PlayerSpawn>();  // Retrieves the players starting location in a dungeon/scene
            playerID = 0;   // Might need to retain throughout the game flow to stop reading player count while session is still active
            GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChange);
            GameManager.Instance.OnLevelChanged.AddListener(SetPlayersInactiveInScene);
        }
    }

    private void Update()
    {
        if(playerReferences[0] != null) // Update to add multiplayer
        {
            for (int i = 0; i < playerCount; i++)    // Updates active player positions per frame
            {
                playerWorldPositions[i] = playerReferences[i].transform.position;
            }
        }
    }

    #endregion

    #region Event Listeners

    /*
     * HandleGameStateChange: Listener for the GameManagers EventOnGameStateChange.
     *                    Is called when even in above event is invoked the the GameManager.
     */
    private void HandleGameStateChange(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if (previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING)
        {
            // Instantiate players with their stats and insert them into player array
            CreatePlayerReferences(playerCount);
        }
        if (previousState == GameManager.GameState.RUNNING && currentState == GameManager.GameState.RUNNING)
        {
            // Instantiate players with their stats and insert them into player array
            SetPlayersActiveInScene();
        }
    }

    #endregion

    #region Control Methods
    /*
     * CreatePlayerReferences: Creates/Instantiates the player on dungeon load.
     *                     To be called when entering a dungeon for the first time to create player persistent data.
     * 
     */
    private void CreatePlayerReferences(int playerCount)
    {
        Debug.LogWarning("[PlayerManager] - CreatePlayerReferences");
        playerSpawn = FindObjectOfType<PlayerSpawn>();  // Retrieves the players starting location in a dungeon/scene
        for (int i = 0; i < playerCount; i++)
        {
            GameObject player = Instantiate(playerPrefabs[0], playerSpawn.transform.position, Quaternion.identity); // Currently only spawns Avelyn
            DontDestroyOnLoad(player);      // Will need to manually destroy when the dungeon is finished
            playerReferences[i] = player;
            playerWorldPositions[i] = player.transform.position;
            playerStats[i] = player.GetComponent<CharacterStats>();
            playerID++;
        }
    }

    /*
     * SetPlayersActiveInScene(): Enables players in the scene upon load of a new scene and sets their respective spawn position
     *                      To be called when loading a new scene in a dungeon that has been started.    
     */
    private void SetPlayersActiveInScene()
    {
        Debug.LogWarning("[PlayerManager] - SetPlayersActiveInScene");
        playerSpawn = FindObjectOfType<PlayerSpawn>();  // Retrieves the players starting location in a dungeon/scene
        // Iterate through player count and instantiate them in the level OnLoadComplete/Start - not sure which one yet
        for (int i = 0; i < playerCount; i++)
        {
            playerReferences[i].transform.position = playerWorldPositions[i] = playerSpawn.transform.position;
            playerReferences[i].SetActive(true);
            //Instantiate(playerReferences[i], playerSpawn.transform.position, Quaternion.identity);  // Might be SetActive later as we have references that are persistent
        }
    }

    /*
     *  SetPlayersInactiveInScene(): Method gets called when transitioning scenes.
     *                          This method will disable player references between scene transitions (Makes it so no player input?)
     */
    private void SetPlayersInactiveInScene(string nextLevel)
    {
        Debug.LogWarning("[PlayerManager] - SetPlayersInactiveInScene");
        // Iterates through player references and disables them inbetween scene transitions
        for (int i = 0; i < playerCount; i++)
        {
            playerReferences[i].SetActive(false);
        }
    }

    /*
     * PlayerTakeDamage(): Method gets called from IAttackable "TakeDamage".
     *                 The method will update player health when taking damage.
     */
    public void PlayerTakeDamage(int playerID, int damage)
    {
        //Debug.LogWarning("[PlayerManager] - PlayerTakeDamage");
        playerStats[playerID - 1].TakeDamage(damage); // - 1 to the player id will result in the reference as I did not want 0 index to be wasted memory
    }

    #endregion

    #region Reporters

    public GameObject GetPlayerReference(int playerID)
    {
        return playerReferences[playerID - 1];
    }

    public CharacterStats GetPlayerStats(int playerID)
    {
        return playerStats[playerID - 1];
    }
    #endregion
}