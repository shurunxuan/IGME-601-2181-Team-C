using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class VirtualCameraController : MonoBehaviour
{
    // The object that the camera is following
    public DroneMovement FollowingObject;

    // The Virtual Cameras
    public CinemachineFreeLook FirstPersonVirtualCamera;
    public CinemachineFreeLook ThirdPersonVirtualCamera;

    public Vector3 LookAtDirection
    {
        get { return transform.forward; }
    }
    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        // Set the forward vector of drone.
        FollowingObject.Forward = Vector3.Cross(transform.right, Vector3.up);
    }

    public void SetFirstPerson(bool value)
    {
        if (value)
        {
            // Activate First Person Camera
            FirstPersonVirtualCamera.Priority = 10;
            ThirdPersonVirtualCamera.Priority = -1;
            // Recenter First Person Camera to Third Person Camera
            FirstPersonVirtualCamera.m_RecenterToTargetHeading.m_enabled = true;
            FirstPersonVirtualCamera.m_RecenterToTargetHeading.DoRecentering(ref FirstPersonVirtualCamera.m_YAxis, 0, 0.5f);
            FirstPersonVirtualCamera.m_RecenterToTargetHeading.DoRecentering(ref FirstPersonVirtualCamera.m_XAxis, 0, ThirdPersonVirtualCamera.m_XAxis.Value);
            FirstPersonVirtualCamera.m_RecenterToTargetHeading.m_enabled = false;
        }
        else
        {
            // Activate Third Person Camera
            ThirdPersonVirtualCamera.Priority = 10;
            FirstPersonVirtualCamera.Priority = -1;
            // Recenter Third Person Camera to First Person Camera
            ThirdPersonVirtualCamera.m_RecenterToTargetHeading.m_enabled = true;
            ThirdPersonVirtualCamera.m_RecenterToTargetHeading.DoRecentering(ref ThirdPersonVirtualCamera.m_YAxis, 0, 0.66f);
            ThirdPersonVirtualCamera.m_RecenterToTargetHeading.DoRecentering(ref ThirdPersonVirtualCamera.m_XAxis, 0, FirstPersonVirtualCamera.m_XAxis.Value);
            ThirdPersonVirtualCamera.m_RecenterToTargetHeading.m_enabled = false;
        }

        FollowingObject.LerpRotation = value;
    }
}
