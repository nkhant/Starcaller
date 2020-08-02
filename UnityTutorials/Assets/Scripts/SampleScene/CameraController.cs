using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform startingPosition;
    public Transform inToDistance;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("CameraChange"))
        {
            gameObject.transform.LookAt(inToDistance);
        }
        else
        {
            gameObject.transform.LookAt(startingPosition);
        }
    }
}
