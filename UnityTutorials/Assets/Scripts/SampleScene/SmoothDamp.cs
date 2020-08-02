using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothDamp : MonoBehaviour
{
    public Transform target;
    private float smoothTime = 0.5f;
    private float yVelocity = 0.0f;
    public float maxSpeed = 2.0f;

    // Update is called once per frame
    void Update()
    {
        float newPosition = Mathf.SmoothDamp(transform.position.y, target.position.y, ref yVelocity, smoothTime, maxSpeed);
        transform.position = new Vector3(transform.position.x, newPosition, transform.position.z);
    }
}
