using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NUMBER PAD MOVEMENT player movement game click numbers for player to go to the number tile
// Collect moving objects

public class ExperimentalController : MonoBehaviour
{
    public float travelSpeed = 1.0f;
    public Transform[] tiles;

    private Rigidbody rb;
    private Vector3 newPosition;
    private int tileIndex = 0;
    private bool arrived = false;
    private bool dirChange = false;

    // Start is called before the first frame update
    //private void Awake()
    //{
    //    transform.position = new Vector3(tiles[tileIndex].position.x, transform.position.y, tiles[tileIndex].position.z);
    //}

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.LookAt(new Vector3(tiles[tileIndex].position.x, transform.position.y, tiles[tileIndex].position.z));
        Debug.Log("TILE 0: " + tiles[0].position);
        Debug.Log("TILE 1: " + tiles[1].position);
        Debug.Log("TILE 2: " + tiles[2].position);
        Debug.Log("TILE 3: " + tiles[3].position);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("INDEX QUERY: " + tileIndex);
        Debug.Log("Arrived: " + arrived);

        transform.LookAt(new Vector3(tiles[tileIndex].position.x, transform.position.y, tiles[tileIndex].position.z));
        if (Input.GetKeyDown(KeyCode.F1))
        {
            arrived = false;
            dirChange = true;
            tileIndex--;
        }
        if(Input.GetKeyDown(KeyCode.F2))
        {
            tileIndex++;
            dirChange = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("ENTERD");
        if (collision.gameObject.tag == "Tile" && !dirChange)
        {
            arrived = true;
        }
    }

    private void FixedUpdate()
    {
        if(!arrived)
        {
            transform.position += transform.forward * Time.deltaTime;
        }
    }
}
