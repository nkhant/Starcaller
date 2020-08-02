using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour, ISpawns
{

    public ItemPickUp_SO[] items;

    private int whichToSpawn = 0;
    private int totalSpawnWeight = 0;
    private int chosenItem = 0;

    public GameObject itemSpawned { get; set; }
    public Renderer itemMaterial { get; set; }
    public ItemPickUp itemType { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        // adds any special items the mobs have specific for them
        foreach (ItemPickUp_SO ip in items)
        {
            totalSpawnWeight += ip.SpawnChanceRate;
        }
    }

    public void CreateSpawn()
    {
        foreach(ItemPickUp_SO ip in items)
        {
            whichToSpawn += ip.SpawnChanceRate;
            if(whichToSpawn >= chosenItem)
            {
                itemSpawned = Instantiate(ip.itemSpawnObject, transform.position, Quaternion.identity);
                itemMaterial = itemSpawned.GetComponent<Renderer>();
                if(itemMaterial != null)
                    itemMaterial.material = ip.itemMaterial;

                itemType = itemSpawned.GetComponent<ItemPickUp>();
                itemType.itemDefinition = ip;
                break;
            }
        }

    }

}
