using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LootDrop : MonoBehaviour
{
    [SerializeField]
    private int total = 0;
    [SerializeField]
    private int randomNumber = 0;
    [SerializeField]
    private float spawnHeightAdjustment = 4.0f;
    Vector3 deadMonsterPosition;
    //public Rigidbody loot;
    // Start is called before the first frame update
    public void LootDropCheck(GameObject deadMonster)
    {
        foreach(KeyValuePair<int, string> item in MonsterLootDropTable.starterContinent)
        {
            total += item.Key;
        }

        Debug.Log("Loot Drop Total: " + total);

        randomNumber = Random.Range(0, total);

        Debug.Log("Dice Roll: " + randomNumber);

        //check for which item is dropped
        foreach (KeyValuePair<int, string> item in MonsterLootDropTable.starterContinent)
        {
            Debug.Log("Checking if item " + item.Value + " dropped");
            if (randomNumber <= item.Key)
            {
                //Item has been dropped
                // Called via:
                if(item.Value != "Nothing")
                {
                    var loadedPrefabResource = LoadPrefabFromFile(item.Value);
                    //Instantiate(loadedPrefabResource, Vector3.zero, Quaternion.identity);
                    if(loadedPrefabResource == null)
                    {
                        return;
                    }
                    deadMonsterPosition = deadMonster.transform.position;
                    deadMonsterPosition.y += spawnHeightAdjustment;
                    GameObject.Instantiate(loadedPrefabResource, deadMonsterPosition, Quaternion.identity);
                }

                Debug.Log(item.Value + " dropped");
                return;
            } else
            {
                randomNumber -= item.Key;
                Debug.Log("Dice Roll: " + randomNumber);
            }
            Debug.Log("Checked if item " + item.Value + " dropped");
        }
    }

    private UnityEngine.Object LoadPrefabFromFile(string filename)
    {
        Debug.Log("Trying to load LevelPrefab from file (" + filename + ")...");
        var loadedObject = Resources.Load(filename);
        if (loadedObject == null)
        {
           Debug.Log("...no file found - please check the configuration");
        }
        return loadedObject;
    }


}
