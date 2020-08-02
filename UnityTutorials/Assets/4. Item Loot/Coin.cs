using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Eventually
public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    public float jumpForce = 100.0f;

    [SerializeField]
    private float timeOfStop = 4.0f;
    private float timeAfterSpawn = 0.0f;

    public Rigidbody rb;

    [SerializeField]
    private float rotateSpeed = 5.0f;
    [SerializeField]
    private float horizontalSpeed = 1.0f;
    private float randomDirection = 0.0f;
    [SerializeField]
    private float jumpVariationAmount = 2.0f;
    [SerializeField]
    private float spinVariationAmount = 2.0f;
    private float finalRotationSpeed = 0.0f;

    void Awake()
    {
        
        rb = GetComponent<Rigidbody>();
        //rotates to be face up
        transform.parent.transform.Rotate(new Vector3(0, 90, 90));
        randomDirection = Random.Range(-2, 2);
        finalRotationSpeed = Random.Range(rotateSpeed - spinVariationAmount, rotateSpeed + spinVariationAmount);
        timeAfterSpawn = Time.time;

        initialBurst();
    }

    private void Update()
    {

        transform.parent.transform.Rotate(finalRotationSpeed, 0, 0);
    }

    private void FixedUpdate()
    {
        //stops loot moving after time set by timeOfStop
        if(Time.time - timeAfterSpawn < timeOfStop)
        {
            transform.parent.transform.Translate(new Vector3(randomDirection, 0, 0) * horizontalSpeed * Time.fixedDeltaTime, Space.World); // Standard horizontal movement
        }

        // ignore collision of loot --> Player/Loot/Enemy
        //Physics.IgnoreLayerCollision(13, 11); //- If used, cant use onTriggerEnter() since based on colliders
        Physics.IgnoreLayerCollision(13, 13);
        Physics.IgnoreLayerCollision(13, 12);

        //ignores platform going up
        if(rb.velocity.y > 0.0f)
        {
            Physics.IgnoreLayerCollision(13,9, true);
        }
        else
        {
            Physics.IgnoreLayerCollision(13, 9, false);
        }
    }

    //Calculations of the velcity of loot on spawn
    private void initialBurst()
    {
        float jumpForceVariation = Random.Range(jumpForce - jumpVariationAmount, jumpForce + jumpVariationAmount);
        //how high loot goes on spawn
        this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForceVariation * Time.fixedDeltaTime, ForceMode.Impulse);
    }

    ////Use this to add to inventory 
    //private void OnTriggerEnter(Collider other)
    //{
    //    //Debug.Log("Going To Pick Up");
    //    if (other.gameObject.layer == 11) //fix latter to no be hard coded
    //    {
    //        //Debug.Log("Picked Up Loot");
    //        Destroy(this.gameObject.transform.parent.gameObject);
    //    }
    //}
}
