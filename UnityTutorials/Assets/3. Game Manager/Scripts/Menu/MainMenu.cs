using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Track the Animation Component
    // Track the AnimationClips for fade in/out
    // FUnction that can revieve animation evens
    // Function to play fade in/out animations
    // Start is called before the first frame update

    [SerializeField] // [] is a decorator
    private Animation _mainMenuAnimator;
    [SerializeField]
    private AnimationClip _fadeOutAnimation;
    [SerializeField]
    private AnimationClip _fadeInAnimation;

    public Events.EventFadeComplete OnMainMenuFadeComplete;

    //good place to register for an event its when created
    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if(previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING)
        {
            FadeOut();
        }

        if(previousState != GameManager.GameState.PREGAME && currentState == GameManager.GameState.PREGAME)
        {
            Debug.LogWarning("FADING IN");
            FadeIn();
        }
    }

    public void OnFadeOutComplete()
    {
        ///Debug.LogWarning("FadeOUT Complete");
        OnMainMenuFadeComplete.Invoke(true);
    }

    public void OnFadeInComplete()
    {
        //Debug.LogWarning("FadeInComplete");
        OnMainMenuFadeComplete.Invoke(false);
        UIManager.Instance.SetDummyCameraActive(true);
    }

    public void FadeIn()
    {
        _mainMenuAnimator.Stop(); //make sure nothing is playing, stops whatver playing
        _mainMenuAnimator.clip = _fadeInAnimation;
        _mainMenuAnimator.Play();
    }

    public void FadeOut()
    {
        UIManager.Instance.SetDummyCameraActive(false);
        _mainMenuAnimator.Stop(); //make sure nothing is playing, stops whatver playing
        _mainMenuAnimator.clip = _fadeOutAnimation;
        _mainMenuAnimator.Play();
    }
}
