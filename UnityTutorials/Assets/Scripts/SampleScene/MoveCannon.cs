using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCannon : MonoBehaviour
{
    public float speed = 1.0f;
    public float rotationSpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        float translation = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float rotation = Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;

        gameObject.transform.Translate(new Vector3(translation, 0, 0), Space.World);
        gameObject.transform.Rotate(new Vector3(0, rotation, 0), Space.World);
    }
}
