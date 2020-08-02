using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawns 
{
    GameObject itemSpawned { get; set; }
    Renderer itemMaterial { get; set; }
    ItemPickUp itemType { get; set; }

    void CreateSpawn(); //just a template and just lets us reference
}
