using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidewaysMovement : MonoBehaviour
{
    public float speed = 1.0f;
    public float procTimer = 1.0f;
    private float resetTimer;
    public bool dirRight = false;
    public float moveSpeed = 1.0f;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        resetTimer = procTimer;
    }

    // Update is called once per frame
    void Update()
    {
        //procTimer -= Time.deltaTime;
        //if(procTimer <= 0.0f)
        //{
        //    procTimer = resetTimer;
        //    dirRight = !dirRight;
        //}
        //if (dirRight)
        //{
        //    rb.AddForce(Vector3.right * speed, ForceMode.Acceleration);
        //}
        //else
        //{
        //    rb.AddForce(Vector3.left * speed, ForceMode.Acceleration);
        //}
    }
}
