using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalFunctions : MonoBehaviour
{
    public int levelIndex;
    public int fadeOutDuration = 2;
    public AudioSource audioSource;

    [SerializeField] private string nextLevel;


    public Events.EventChangeLevel OnChangeLevel;

    private void Start()
    {
        audioSource = transform.parent.gameObject.GetComponent<AudioSource>();
        gameObject.SetActive(false);
        OnChangeLevel.AddListener(GameManager.Instance.OnLevelChange);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && Input.GetButtonDown("Down") && other.gameObject.GetComponent<Player_Physics>().IsGrounded())
        {
            OnChangeLevel.Invoke(nextLevel);    // Place the level name in the portal that it takes you to when using it
            //GameManager.Instance.RestartGame();
            // Loading level with build index
            // SceneManager.LoadScene(levelIndex);
            // Loading level with scene name
            //SceneManager.LoadScene(levelName);
            // Restart level
            //audioSource.Play();
            //Time.timeScale = 0;
            //StartCoroutine(UsePortal());
            //yield return new WaitForSeconds(2);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    //IEnumerator UsePortal()
    //{
    //    //yield return new WaitForSecondsRealtime(2);
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //}
}
