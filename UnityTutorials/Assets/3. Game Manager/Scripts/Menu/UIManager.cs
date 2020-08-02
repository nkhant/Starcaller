using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Manager<UIManager>
{
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private PlayerUI _playerUI;
    [SerializeField] private InventoryMenu _inventorySystem;
    [SerializeField] private Camera _dummyCamera;

    public Events.EventFadeComplete OnMainMenuFadeComplete;
    public Events.EventPlayerTakeDamage OnPlayerTakeDamage;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
        _mainMenu.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);
        //OnPlayerTakeDamage.AddListener(_playerUI.OnPlayerTakeDamage);
    }

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        _pauseMenu.gameObject.SetActive(currentState == GameManager.GameState.PAUSED);
        _playerUI.gameObject.SetActive(currentState == GameManager.GameState.RUNNING);
        _inventorySystem.gameObject.SetActive(currentState == GameManager.GameState.RUNNING);
    }

    //helps us bubble up the meunu system
    void HandleMainMenuFadeComplete(bool fadeOut)
    {
        OnMainMenuFadeComplete.Invoke(fadeOut);
    }

    private void Update()
    {
        if(GameManager.Instance.CurrentGameState != GameManager.GameState.PREGAME)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.ActiveAO() == false)
        {
            //_mainMenu.FadeOut();
            GameManager.Instance.StartGame();
            //SetDummyCameraActive(false);
            SetInventoryActive(true);
        }
    }

    public void SetDummyCameraActive(bool active)
    {
        _dummyCamera.gameObject.SetActive(active);
    }

    public void SetInventoryActive(bool active)
    {
        _inventorySystem.gameObject.SetActive(active);
    }
}
