using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private GameObject caster;
    private float speed;
    private float range;
    private Vector3 travelDirection;

    private float distanceTraveled;

    public Events.EventFireballCollided OnFireballCollided;

    public void Fire(GameObject caster, float speed, Transform playerTransform, float range)
    {
        this.caster = caster;
        this.speed = speed;
        this.range = range;

        // Calculate travel direction
        travelDirection = playerTransform.forward;
        travelDirection.y = 0.0f;
        travelDirection.Normalize();

        // Initalize distance traveled
        distanceTraveled = 0.0f;
    }

    private void Update()
    {
        // Move this fireball through space
        float distanceToTravel = speed * Time.deltaTime;

        transform.Translate(travelDirection * distanceToTravel);

        // Check to see if we traveled too far, if so destroy this projectile
        distanceTraveled += distanceToTravel;
        //Debug.Log("DTravele: " + distanceTraveled);
        if(distanceTraveled > range)
        {
            //Debug.Log("DESTROYED");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Raise an event
        Debug.Log("MONSTER: " + other.name);
        if(OnFireballCollided != null)
        {
            OnFireballCollided.Invoke(caster, other.gameObject);
        }

        Destroy(gameObject);
    }
}
