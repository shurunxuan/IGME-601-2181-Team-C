using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class VirtualCameraController : MonoBehaviour
{
    [Header("Objects")]
    // The object that the camera is following
    public DroneMovement FollowingObject;
    // First Person Position
    public Transform FirstPersonPosition;

    // The Virtual Cameras
    [Header("Virtual Cameras")]
    public CinemachineVirtualCamera FirstPersonVirtualCamera;
    public CinemachineVirtualCamera ThirdPersonVirtualCamera;


    // Speed Factors
    [Header("Third Person")]
    public float HorizontalViewSpeedThirdPerson;
    public float VerticalViewSpeedThirdPerson;
    [Header("First Person")]
    public float HorizontalViewSpeedFirstPerson;
    public float VerticalViewSpeedFirstPerson;

    private Vector3 firstPersonForward;
    private bool useFirstPerson;

    public Vector3 LookAtDirection
    {
        get { return transform.forward; }
    }
    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        firstPersonForward = FirstPersonPosition.forward;
        useFirstPerson = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        // Set the forward vector of drone.
        FollowingObject.Forward = Vector3.Cross(transform.right, Vector3.up);

        // Get User Input
        float horizontalCamera = Input.GetAxis("HorizontalCamera");
        float verticalCamera = Input.GetAxis("VerticalCamera");

        // Rotate the LookAtDirection regarding user input
        float horizontalViewSpeed = useFirstPerson ? HorizontalViewSpeedFirstPerson : HorizontalViewSpeedThirdPerson;
        float verticalViewSpeed = useFirstPerson ? VerticalViewSpeedFirstPerson : VerticalViewSpeedThirdPerson;
        firstPersonForward = Quaternion.AngleAxis(horizontalCamera * horizontalViewSpeed, Vector3.up) * firstPersonForward;
        firstPersonForward = Quaternion.AngleAxis(-verticalCamera * verticalViewSpeed, Vector3.Cross(Vector3.up, firstPersonForward)) * firstPersonForward;

        // Restrict the pitch angle of LookAtDirection
        if (Mathf.Acos(firstPersonForward.y / firstPersonForward.magnitude) < Mathf.Deg2Rad * 10)
        {
            firstPersonForward = new Vector3(firstPersonForward.x, 0, firstPersonForward.z);
            firstPersonForward.y = firstPersonForward.magnitude / Mathf.Tan(Mathf.Deg2Rad * 10);
            firstPersonForward = firstPersonForward.normalized;
        }
        else if (Mathf.Acos(firstPersonForward.y / firstPersonForward.magnitude) > Mathf.Deg2Rad * 170)
        {
            firstPersonForward = new Vector3(firstPersonForward.x, 0, firstPersonForward.z);
            firstPersonForward.y = firstPersonForward.magnitude / Mathf.Tan(Mathf.Deg2Rad * 170);
            firstPersonForward = firstPersonForward.normalized;
        }

        FirstPersonPosition.LookAt(FirstPersonPosition.position + Vector3.Slerp(FirstPersonPosition.forward, firstPersonForward, 30f * Time.deltaTime));
    }

    public void SetFirstPerson(bool value)
    {
        if (value)
        {
            // Activate First Person Camera
            FirstPersonVirtualCamera.Priority = 10;
            ThirdPersonVirtualCamera.Priority = -1;
            //// Recenter First Person Camera to Third Person Camera
            //FirstPersonVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.Value = -Mathf.Asin(transform.forward.y) * Mathf.Rad2Deg;
            //FirstPersonVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value = ThirdPersonVirtualCamera.m_XAxis.Value;
            //FirstPersonVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalRecentering.m_enabled = true;
            //FirstPersonVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalRecentering.m_enabled = true;
            //FirstPersonVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalRecentering.DoRecentering(ref FirstPersonVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis, 0, 0.0f);
            //FirstPersonVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalRecentering.DoRecentering(ref FirstPersonVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis, 0, ThirdPersonVirtualCamera.m_XAxis.Value - 90.0f);
            //FirstPersonVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalRecentering.m_enabled = false;
            //FirstPersonVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalRecentering.m_enabled = false;
        }
        else
        {
            // Activate Third Person Camera
            ThirdPersonVirtualCamera.Priority = 10;
            FirstPersonVirtualCamera.Priority = -1;
            //// Recenter Third Person Camera to First Person Camera
            //ThirdPersonVirtualCamera.m_YAxis.Value = 0.66f;
            //ThirdPersonVirtualCamera.m_XAxis.Value = FirstPersonVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value;
            //ThirdPersonVirtualCamera.m_RecenterToTargetHeading.m_enabled = true;
            //ThirdPersonVirtualCamera.m_RecenterToTargetHeading.DoRecentering(ref ThirdPersonVirtualCamera.m_YAxis, 0, 0.66f);
            //ThirdPersonVirtualCamera.m_RecenterToTargetHeading.DoRecentering(ref ThirdPersonVirtualCamera.m_XAxis, 0, FirstPersonVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value);
            //ThirdPersonVirtualCamera.m_RecenterToTargetHeading.m_enabled = false;
        }

        useFirstPerson = value;
        FollowingObject.SkipLerpRotation = value;
    }
}
