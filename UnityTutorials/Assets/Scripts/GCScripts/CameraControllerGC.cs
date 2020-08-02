using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerGC : MonoBehaviour
{
    public GameObject target;
    public float smoothTime = 0.5f;
    public Vector3 transformPoint;

    private CameraBounds bound;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        bound = gameObject.GetComponent<CameraBounds>();
    }

    private void Start()
    {
        target = PlayerManager.Instance.GetPlayerReference(1).transform.GetChild(4).gameObject;  // Hard coded to take player 1
    }

    private void Update()
    {
        // Define target position above and behind the target transform
        //Vector3 targetPosition = target.TransformPoint(transformPoint);
        Vector3 targetPosition = target.transform.position;

        //Debug.Log(targetPosition);

        // Smoothly move the camera towards that target position
        //transform.position = Vector3.SmoothDamp(new Vector3(transform.position.x, 0, 0), targetPosition, ref velocity, smoothTime);
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(
            Mathf.Clamp(targetPosition.x, bound.minCameraPos.x, bound.maxCameraPos.x),
            Mathf.Clamp(targetPosition.y, bound.minCameraPos.y, bound.maxCameraPos.y),
            Mathf.Clamp(targetPosition.z, bound.minCameraPos.z, bound.maxCameraPos.z)), ref velocity, smoothTime);
    }
}
