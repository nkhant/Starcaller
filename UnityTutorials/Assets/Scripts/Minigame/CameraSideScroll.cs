using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSideScroll : MonoBehaviour
{
    public Transform playerPivot;
    public float lerpSpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(playerPivot);
        Vector3 slidePosition = new Vector3(playerPivot.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, slidePosition, lerpSpeed);
    }
}
