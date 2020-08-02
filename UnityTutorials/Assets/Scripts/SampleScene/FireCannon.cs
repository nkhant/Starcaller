using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCannon : MonoBehaviour
{
    public Rigidbody projectile;
    public Transform cannonEnd;
    public float fireSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetButton("Fire1"))
        //{
        //    Rigidbody projectileInstance;
        //    projectileInstance = Instantiate(projectile, cannonEnd.position, cannonEnd.rotation) as Rigidbody;
        //    projectileInstance.AddForce(cannonEnd.forward * fireSpeed);
        //}
        if (Input.GetButtonDown("Fire1"))
        {
            Rigidbody projectileInstance;
            projectileInstance = Instantiate(projectile, cannonEnd.position, cannonEnd.rotation) as Rigidbody;
            projectileInstance.AddForce(cannonEnd.forward * fireSpeed);
        }
    }
}
