using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass : MonoBehaviour
{
    // Player Fields
    public int health;
    public int mana;

    // Constructor
    public PlayerClass()
    {
        this.health = 3;
        this.mana = 3;
    }
}
