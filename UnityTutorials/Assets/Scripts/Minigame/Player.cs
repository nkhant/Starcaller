using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerClass))]
public class Player : MonoBehaviour
{
    // Classes
    private PlayerClass player;

    // Data Structures

    // Variables

    // References
    private Material[] materialStorage;
    private Material playerMaterial;

    private void Awake()
    {
        player = gameObject.AddComponent(typeof(PlayerClass)) as PlayerClass;
        playerMaterial = gameObject.GetComponent<Renderer>().material;
        materialStorage = GameObject.Find("Material Storage").GetComponent<MaterialStorage>().material;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Health: " + player.health);
        Debug.Log("Mana: " + player.mana);
    }

    // Helper Methods
    public void ChangeMaterial(int color)
    {
        playerMaterial.color = materialStorage[color].color;
    }
}
