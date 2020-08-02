using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Manager<GameManager>
{

    // what level the game is currently in (dungeon?)
    //load and unload game levels
    //keep track of the game state
    //generate other persistant system

    // PREGAME, RUNNING, PAUSED

    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED,
    }

    //accessible by inspector in unity. contains that manager might need to keep track off or during boot scene (are ment to be created)
    public GameObject[] SystemPrefabs;
    //actual version of the, because its public, anyone else who has
    //access to game manager can listen to event
    public Events.EventGameState OnGameStateChanged;
    public Events.EventChangeLevel OnLevelChanged;
    //public Events.EventPlayerTakeDamage OnPlayerTakeDamage;

    //things that have been created and want to keep track (have been created, or actually created)
    List<GameObject> _instancedSystemPrefabs;
    List<AsyncOperation> _loadOperations;
    GameState _currentGameState = GameState.PREGAME;

    string _currentLevelName = string.Empty;

    //this allows external systems to request what the current game state is
    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        //so cant randomly set state
        private set { _currentGameState = value; }
    }

    private void Start()
    {
        // make sure game manager statys around and doesnt get destroyed
        //DontDestroyOnLoad(gameObject);

        //every new operation, added, and then completed remove
        _instancedSystemPrefabs = new List<GameObject>();
;       _loadOperations = new List<AsyncOperation>();

        //make sure anything that needs to be created is created on start
        InstantiateSystemPrefabs();
        //LoadLevel("GC_Remake");
        UIManager.Instance.OnMainMenuFadeComplete.AddListener(OnMainMenuFadeComplete);
    }

    private void Update()
    {
        if (_currentGameState == GameState.PREGAME)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //_mainMenu.FadeOut();
            TogglePause();
        }
    }

    public bool ActiveAO()
    {
        Debug.LogWarning("AO COUNT: " + _loadOperations.Count);
        return _loadOperations.Count != 0;
    }

    void OnLoadOperationComplete(AsyncOperation ao)
    {
        //once a scene finishes loading
        if (_loadOperations.Contains(ao)) 
        {
            _loadOperations.Remove(ao);
            // dispatch message
            // tranition between scenes
            if(_loadOperations.Count == 0)
            {
                UpdateState(GameState.RUNNING);
            }
           
        }
        Debug.Log("Load Complete");
    }

    void OnUnloadOperationComplete(AsyncOperation ao)
    {
        Debug.LogWarning("Unload Complete");
    }

    void OnMainMenuFadeComplete(bool fadeOut)
    {
        if (!fadeOut)
        {
            Debug.LogWarning("UNLOADING LEVEL");
            UnloadLevel(_currentLevelName);
        }
    }

    private void UpdateState(GameState state)
    {
        GameState previousGameState = _currentGameState;
        //updates the current state
        _currentGameState = state;

        switch (_currentGameState)
        {
            case GameState.PREGAME:
                Time.timeScale = 1.0f;
                break;
            case GameState.RUNNING:
                Time.timeScale = 1.0f;
                break;
            case GameState.PAUSED:
                Time.timeScale = 0.0f;
                break;
            default:
                break;
        }
        // dispatch message
        // tranition between scenes
        // Invoke lets you distinquish between events and methods in a class
        OnGameStateChanged.Invoke(_currentGameState, previousGameState);
    }

    //creates and keeps track
    void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;
        for(int i = 0; i < SystemPrefabs.Length; ++i)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            _instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    public void LoadLevel(string levelName)
    {
        //asyncOp knows everyhing happening inside
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single); //game manager to be continuous
        if(ao == null)
        {
            Debug.LogError("[GameManager] Unable to load level " + levelName);
            return;
        }
        ao.completed += OnLoadOperationComplete;
        _loadOperations.Add(ao);
        _currentLevelName = levelName;
    }

    // need if additive load scene mode, address space bar spam
    public void UnloadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to load level" + levelName);
            return;
        }
        ao.completed += OnUnloadOperationComplete;
    }

    //garbo collects and cleans stuff
    //protected override void OnDestroy()
    //{
    //    //still want the thing in singleton to actually be called
    //    base.OnDestroy();
    //    for(int i = 0; i < _instancedSystemPrefabs.Count; ++i)
    //    {
    //        Destroy(_instancedSystemPrefabs[i]);
    //    }
    //    _instancedSystemPrefabs.Clear();
    //}

    protected void OnDestroy()
    {
        //still want the thing in singleton to actually be called
        //base.OnDestroy();
        //since gamemanager has a chance to be destoryed on awake
        //check since _instancedsystemprefabs created in awake have this check
        if (_instancedSystemPrefabs == null)
            return;
        for (int i = 0; i < _instancedSystemPrefabs.Count; ++i)
        {
            Destroy(_instancedSystemPrefabs[i]);
        }
        _instancedSystemPrefabs.Clear();
    }

    public void OnLevelChange(string nextLevel)
    {
        OnLevelChanged.Invoke(nextLevel);
        LoadLevel(nextLevel);
    }

    public void StartGame()
    {
        LoadLevel("GC_Remake");
        //UpdateState(GameState.RUNNING);
    }

    public void TogglePause()
    {
        UpdateState(_currentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING); // Turnary operation
    }

    public void RestartGame()
    {
        UpdateState(GameState.PREGAME);
    }

    public void QuitGame()
    {
        // Implement features for quitting

        Application.Quit();
    }
}
