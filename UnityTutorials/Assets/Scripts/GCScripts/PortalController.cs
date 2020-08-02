using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public AudioSource audioSource;
    [SerializeField] private List<Transform> portals;
    private bool finished;
    private bool soundPlayed;

    //private MonsterManager monsterManager;

    private void Awake()
    {
        int children = transform.childCount;
        for (int i = 0; i < children; i++)
        {
            portals.Add(transform.GetChild(i));
        }
    }

    void Start()
    {
        // DEACTIVATE WHEN SWAPPING TO PREGAME
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChange);
        //monsterManager = GameObject.Find("MonsterManager").GetComponent<MonsterManager>(); // Might be extensive call to change later
        audioSource = GetComponent<AudioSource>();
        finished = false;
        soundPlayed = false;
    }

    void Update()
    {
        if (!finished)
        {
            if (MonsterManager.Instance.ActiveMonsters() == false) // Call checking if any monsters are alive
            {
                if (!soundPlayed)
                {
                    audioSource.Play();
                }
                SetPortals(true);
                finished = true;
            }
        }
    }

    private void HandleGameStateChange(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        //if (currentState == GameManager.GameState.RUNNING && previousState == GameManager.GameState.PREGAME)
        //{

        //}
        if (currentState == GameManager.GameState.PREGAME && previousState == GameManager.GameState.PAUSED)
        {
            SetPortals(false);
        }
    }

    private void SetPortals(bool active)
    {
        foreach (Transform portal in portals)
        {
            portal.gameObject.SetActive(active);
        }
    }
}
