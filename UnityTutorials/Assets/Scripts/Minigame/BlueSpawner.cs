﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueSpawner : MonoBehaviour
{
    //enum Mat { player, gold, cannonball, spaceship, rocket };

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().ChangeMaterial(2);
        }
    }
}