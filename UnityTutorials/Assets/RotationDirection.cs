using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationDirection : MonoBehaviour
{
    // Start is called before the first frame update
    //public Vector3 lookDirection = new Vector3(-2.35f, 1.10f, 0.00f); 
    void Update()
    {
        transform.forward = new Vector3(0f, 0f, 4f);

    }
}
