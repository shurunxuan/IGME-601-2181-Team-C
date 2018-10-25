﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // The object that the camera is following
    public GameObject FollowingObject;
    // The direction that the camera is currently looking at
    public Vector3 LookAtDirection;

    // Speed Factors
    public float HorizontalViewSpeed;
    public float VerticalViewSpeed;

    // Calculated position that the camera should be at
    private Vector3 targetPosition;
    // Variable for not changing the magnitude of LookAtDirection
    private float distance;

    // Use this for initialization
    void Start()
    {
        // Initialize LookAtDirection
        LookAtDirection = FollowingObject.transform.position + 0.1f * Vector3.up - transform.position;
        distance = LookAtDirection.magnitude;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Get User Input
        float horizontalCamera = Input.GetAxis("HorizontalCamera");
        float verticalCamera = Input.GetAxis("VerticalCamera");

        // Rotate the LookAtDirection regarding user input
        LookAtDirection = Quaternion.AngleAxis(horizontalCamera * HorizontalViewSpeed, Vector3.up) * LookAtDirection;
        LookAtDirection = Quaternion.AngleAxis(-verticalCamera * VerticalViewSpeed, Vector3.Cross(Vector3.up, LookAtDirection)) * LookAtDirection;

        // Restrict the pitch angle of LookAtDirection
        if (Mathf.Acos(LookAtDirection.y / LookAtDirection.magnitude) < Mathf.Deg2Rad * 10)
        {
            LookAtDirection = new Vector3(LookAtDirection.x, 0, LookAtDirection.z);
            LookAtDirection.y = LookAtDirection.magnitude / Mathf.Tan(Mathf.Deg2Rad * 10);
            LookAtDirection = LookAtDirection.normalized * distance;
        }
        else if (Mathf.Acos(LookAtDirection.y / LookAtDirection.magnitude) > Mathf.Deg2Rad * 170)
        {
            LookAtDirection = new Vector3(LookAtDirection.x, 0, LookAtDirection.z);
            LookAtDirection.y = LookAtDirection.magnitude / Mathf.Tan(Mathf.Deg2Rad * 170);
            LookAtDirection = LookAtDirection.normalized * distance;
        }
        // Set the forward vector of drone.
        FollowingObject.GetComponent<DroneMovement>().Forward = Vector3.Cross(Vector3.Cross(Vector3.up, LookAtDirection), Vector3.up);

        // Position and rotation transition
        targetPosition = FollowingObject.transform.position - LookAtDirection + 0.1f * Vector3.up;
        transform.position = Vector3.Lerp(transform.position, targetPosition, 20f * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(LookAtDirection, Vector3.up), 20f * Time.fixedDeltaTime);
    }

}
