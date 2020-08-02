using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    // Start is called before the first frame update
    //private UIManager uiManager;
    //public Events.EventPlayerTakeDamage OnPlayerTakeDamage;
    //[SerializeField]
    //private HealthBar healthBar;

    private void Awake()
    {
       //OnPlayerTakeDamage.AddListener(UIManager.OnPlayerTakeDamage)
      // healthBar = FindObjectOfType<MonsterManager>();

    }
    // Update is called once per frame

    public void OnPlayerTakeDamage(int damage)
    {
        Debug.Log("Passed");
    }

}
