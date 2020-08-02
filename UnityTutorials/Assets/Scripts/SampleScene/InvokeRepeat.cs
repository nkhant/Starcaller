using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvokeRepeat : MonoBehaviour
{
    public GameObject spawnItem;

    public float minimum = -1.0f;
    public float maximum = 1.0f;
    public float startingTime = 4.0f;
    public float repeatTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnObject", startingTime, repeatTime);
    }
    
    private void SpawnObject()
    {
        float x = Random.Range(minimum, maximum);
        Instantiate(spawnItem, new Vector3(x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
    }
}
