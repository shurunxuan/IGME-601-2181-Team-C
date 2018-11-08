using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
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

    [Header("Collision Detect")]
    public float ThirdPersonLowestAngle;
    public float ThirdPersonHighestAngle;
    public float RaycastDensity;
    public LayerMask IgnoreLayer;

    private Vector3 firstPersonForward;
    private bool useFirstPerson;
    private CinemachineBrain cinemachineBrain;
    private float cameraDistance;
    private MeshRenderer[] renderers;

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
        cinemachineBrain = gameObject.GetComponent<CinemachineBrain>();
        cameraDistance = ThirdPersonVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.magnitude;
        renderers = FollowingObject.gameObject.GetComponentsInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        bool enableMeshRenderer = !useFirstPerson || cinemachineBrain.ActiveBlend != null;
        foreach (var meshRenderer in renderers)
        {
            meshRenderer.enabled = enableMeshRenderer;
        }
    }

    void FixedUpdate()
    {
        // If the camera is not in transition
        if (cinemachineBrain.ActiveBlend == null)
            // Set the forward vector of drone.
            FollowingObject.Forward = Vector3.Cross(transform.right, Vector3.up);

        // Get User Input
        float horizontalCamera = Input.GetAxis("HorizontalCamera");
        float verticalCamera = Input.GetAxis("VerticalCamera");
        verticalCamera = Mathf.Clamp(verticalCamera, -1.5f, 1.5f);

        // Rotate the LookAtDirection regarding user input
        float horizontalViewSpeed = useFirstPerson ? HorizontalViewSpeedFirstPerson : HorizontalViewSpeedThirdPerson;
        float verticalViewSpeed = useFirstPerson ? VerticalViewSpeedFirstPerson : VerticalViewSpeedThirdPerson;
        firstPersonForward = Quaternion.AngleAxis(horizontalCamera * horizontalViewSpeed, Vector3.up) * firstPersonForward;
        firstPersonForward = Quaternion.AngleAxis(-verticalCamera * verticalViewSpeed, Vector3.Cross(Vector3.up, firstPersonForward)) * firstPersonForward;

        // Pitch angle restriction
        float lowestAngle = ThirdPersonLowestAngle;
        float highestAngle = ThirdPersonHighestAngle;

        if (!useFirstPerson)
        {
            // Calculate the angle restriction according to the surrounding
            Ray upRay = new Ray(FollowingObject.gameObject.transform.position, Vector3.up);
            Ray downRay = new Ray(FollowingObject.gameObject.transform.position, Vector3.down);
            RaycastHit hitInfo;
            if (Physics.Raycast(upRay, out hitInfo, float.PositiveInfinity, ~IgnoreLayer))
            {
                float height = hitInfo.point.y - FirstPersonPosition.position.y - 2f;
                float angle = (Mathf.PI / 2 + Mathf.Asin(height / cameraDistance)) * Mathf.Rad2Deg;
                highestAngle = Mathf.Min(angle, ThirdPersonHighestAngle);
            }

            if (Physics.Raycast(downRay, out hitInfo, float.PositiveInfinity, ~IgnoreLayer))
            {
                float height = hitInfo.point.y - FirstPersonPosition.position.y;
                float angle = (Mathf.PI / 2 + Mathf.Asin(height / cameraDistance)) * Mathf.Rad2Deg;
                lowestAngle = Mathf.Max(angle, ThirdPersonLowestAngle);
            }
        }

        // Restrict the pitch angle of firstPersonForward
        // The firstPersonForward is used by both FirstPersonVCam and ThirdPersonVCam
        // Both Virtual Cameras will look in this direction
        float currentAngle = Mathf.Acos(firstPersonForward.y / firstPersonForward.magnitude);
        if (currentAngle < Mathf.Deg2Rad * lowestAngle)
        {
            Vector3 newFirstPersonForward = new Vector3(firstPersonForward.x, 0, firstPersonForward.z);
            newFirstPersonForward.y = newFirstPersonForward.magnitude / Mathf.Tan(Mathf.Deg2Rad * lowestAngle);
            newFirstPersonForward = newFirstPersonForward.normalized;
            firstPersonForward = Vector3.Slerp(firstPersonForward, newFirstPersonForward, 20f * Time.fixedDeltaTime);
        }
        else if (currentAngle > Mathf.Deg2Rad * highestAngle)
        {
            Vector3 newFirstPersonForward = new Vector3(firstPersonForward.x, 0, firstPersonForward.z);
            newFirstPersonForward.y = newFirstPersonForward.magnitude / Mathf.Tan(Mathf.Deg2Rad * highestAngle);
            newFirstPersonForward = newFirstPersonForward.normalized;
            firstPersonForward = Vector3.Slerp(firstPersonForward, newFirstPersonForward, 20f * Time.fixedDeltaTime);
        }


        //Debug.Log("Curr: " + Mathf.Acos(firstPersonForward.y / firstPersonForward.magnitude) * Mathf.Rad2Deg);

        FirstPersonPosition.LookAt(FirstPersonPosition.position + Vector3.Slerp(FirstPersonPosition.forward, firstPersonForward, 30f * Time.deltaTime));
    }

    public void SetFirstPerson(bool value)
    {
        if (value)
        {
            // Activate First Person Camera
            FirstPersonVirtualCamera.Priority = 10;
            ThirdPersonVirtualCamera.Priority = -1;
        }
        else
        {
            // Activate Third Person Camera
            ThirdPersonVirtualCamera.Priority = 10;
            FirstPersonVirtualCamera.Priority = -1;
        }

        useFirstPerson = value;
        FollowingObject.SkipLerpRotation = value;
    }

    void OnDrawGizmos()
    {

    }
}
