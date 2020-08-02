using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Manager<AudioManager>
{
    #region Fields
    //audiosource can only play 1 loop sound, secondary lets us to be able to acrossfade
    [SerializeField]
    private AudioSource primaryMusiceSource;
    //[SerializeField]
    //private AudioSource secondaryMusiceSource;
    //[SerializeField]
    //private AudioSource sfxSource;

    private bool firstMusicSourceIsPlaying = false;
    #endregion

    ////
    //public override void Awake()
    //{
    //    //we still want to run the basic original function for the manager extension so we called base.awake();
    //    base.Awake();
    //    //primaryMusiceSource = this.gameObject.AddComponent<AudioSource>();
    //    //secondaryMusiceSource = this.gameObject.AddComponent<AudioSource>();
    //    //sfxSource = this.gameObject.AddComponent<AudioSource>();

    //    //Loop the music tracks
    //    primaryMusiceSource.loop = true;
    //    secondaryMusiceSource.loop = true;

    //}

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChange);
    }

    private void HandleGameStateChange(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if(previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING)
        {
            primaryMusiceSource.Play();
        }
        if(previousState == GameManager.GameState.PAUSED && currentState == GameManager.GameState.PREGAME)
        {
            primaryMusiceSource.Stop();
        }
        if (previousState == GameManager.GameState.RUNNING && currentState == GameManager.GameState.PREGAME)
        {
            primaryMusiceSource.Stop();
        }
    }

    public void PlayBackGroundMusic(AudioSource activeSource)
    {
        //Determine which source is active
        //AudioSource activeSource = (firstMusicSourceIsPlaying) ? primaryMusiceSource : secondaryMusiceSource;

        //activeSource.clip = musicClip;
        //activeSource.volume = 0.1f;
        activeSource.Play();
    }

    public void PlayMusicWithFade(AudioSource primaryMusiceSource, AudioSource secondaryMusiceSource, float transitionTime = 1.0f)
    {
        //Determine which source is active
        //AudioSource activeSource = (firstMusicSourceIsPlaying) ? primaryMusiceSource : secondaryMusiceSource;
        StartCoroutine(UpdateMusicWIthFade(primaryMusiceSource, secondaryMusiceSource, transitionTime));
    }
    public void PlayMusicWithCrossFade(AudioSource primaryMusiceSource, AudioSource secondaryMusiceSource, float transitionTime = 1.0f)
    {
        //Determine which source is active
        //Find which is active and which one isnt
        //AudioSource activeSource = (firstMusicSourceIsPlaying) ? primaryMusiceSource : secondaryMusiceSource;
        //AudioSource newSource = (firstMusicSourceIsPlaying) ? secondaryMusiceSource : primaryMusiceSource;

        ////Swap the bool, so you always swapping between the two
        //firstMusicSourceIsPlaying = !firstMusicSourceIsPlaying;

        //newSource.clip = musicClip;
        secondaryMusiceSource.Play();

        StartCoroutine(UpdateMusicWIthCrossFade(primaryMusiceSource, secondaryMusiceSource, transitionTime));
    }

    private IEnumerator UpdateMusicWIthFade(AudioSource primaryMusiceSource, AudioSource secondaryMusiceSource, float transitionTime)
    {
        //make sure active and is playing
        if (!primaryMusiceSource.isPlaying)
            primaryMusiceSource.Play();

        float transition = 0.0f;

        //fade out
        for (transition = 0; transition < transitionTime; transition += Time.deltaTime)
        {
            primaryMusiceSource.volume = (1 - (transition / transitionTime));
            yield return null;
        }

        primaryMusiceSource.Stop();
        //activeSource.clip = newClip;
        secondaryMusiceSource.Play();

        //Fade In
        for (transition = 0; transition < transitionTime; transition += Time.deltaTime)
        {
            secondaryMusiceSource.volume = (transition / transitionTime);
            yield return null;
        }
    }
    private IEnumerator UpdateMusicWIthCrossFade(AudioSource primaryMusiceSource, AudioSource secondaryMusiceSource, float transitionTime)
    {
        float transition = 0.0f;

        //fade out
        for (transition = 0; transition <= transitionTime; transition += Time.deltaTime)
        {
            primaryMusiceSource.volume = (1 - (transition / transitionTime));
            secondaryMusiceSource.volume = (transition / transitionTime);
            yield return null;
        }

        primaryMusiceSource.Stop();

    }

    public void PlaySFX(AudioSource source)
    {
        //will mute if its only .play but .PlayOneShot lets you overlap (more cpu intensive)
        source.PlayOneShot(source.clip);
    }
    public void PlaySFX(AudioSource source, float volume)
    {
        source.PlayOneShot(source.clip, volume);
    }

    public void SetMusicVolume(AudioSource source, float volume)
    {
        source.volume = volume;
    }

    public void SetMusicVolume(AudioSource primaryMusiceSource, AudioSource secondaryMusiceSource, float volume)
    {
        primaryMusiceSource.volume = volume;
        secondaryMusiceSource.volume = volume;
    }

    public void SetSFXVolume(AudioSource source, float volume)
    {
        source.volume = volume;
    }
}
