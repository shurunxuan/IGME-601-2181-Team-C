using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime.DynamicDispatching;
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
    public float ThirdPersonLowestAngle;
    public float ThirdPersonHighestAngle;
    [Header("First Person")]
    public float HorizontalViewSpeedFirstPerson;
    public float VerticalViewSpeedFirstPerson;

    [Header("Collision Detect")]
    public float CollisionRadius;
    public LayerMask IgnoreLayer;
    public float RaycastDensity;

    private Vector3 firstPersonForward;
    private bool useFirstPerson;
    private CinemachineBrain cinemachineBrain;
    private MeshRenderer[] renderers;

    private bool firstPersonCameraRequested;
    private bool thirdPersonCameraCollided;
    private bool lineOfSightIsBroken;

    public Vector3 LookAtDirection
    {
        get { return transform.forward; }
    }
    // Use this for initialization
    void Start()
    {
        firstPersonForward = FirstPersonPosition.forward;
        useFirstPerson = false;
        cinemachineBrain = gameObject.GetComponent<CinemachineBrain>();
        renderers = FollowingObject.gameObject.GetComponentsInChildren<MeshRenderer>();
        lineOfSightIsBroken = false;
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

        // See if something breaks the line of sight
        // As long as we can see one part of the drone, we don't need to toggle between cameras.
        // We need to do raycast in two directions to prevent the camera being stuck in something like the walls.
        foreach (var meshRenderer in renderers)
        {
            // Raycast from camera to player
            Ray lineOfSight = new Ray(ThirdPersonVirtualCamera.transform.position, meshRenderer.gameObject.transform.position - ThirdPersonVirtualCamera.transform.position);
            RaycastHit hitInfo;

            lineOfSightIsBroken = Physics.Raycast(lineOfSight, out hitInfo,
                Vector3.Distance(ThirdPersonVirtualCamera.transform.position,
                    meshRenderer.gameObject.transform.position), ~IgnoreLayer);

            // Raycast from player to camera
            lineOfSight = new Ray(meshRenderer.gameObject.transform.position, ThirdPersonVirtualCamera.transform.position - meshRenderer.gameObject.transform.position);

            lineOfSightIsBroken = lineOfSightIsBroken || Physics.Raycast(lineOfSight, out hitInfo,
                                      Vector3.Distance(ThirdPersonVirtualCamera.transform.position,
                                          meshRenderer.gameObject.transform.position), ~IgnoreLayer);

            // One part of the player can be seen
            if (!lineOfSightIsBroken)
                break;
        }

        // Find obstacles near third person camera
        thirdPersonCameraCollided = false;
        for (float i = 0; i < Mathf.PI * 2; i += Mathf.PI * 2 / RaycastDensity)
        {
            Ray ray = new Ray(ThirdPersonVirtualCamera.transform.position, new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i)));
            thirdPersonCameraCollided = Physics.Raycast(ray, CollisionRadius, ~IgnoreLayer);

            if (thirdPersonCameraCollided) break;

            ray = new Ray(ThirdPersonVirtualCamera.transform.position, new Vector3(0, Mathf.Sin(i), Mathf.Cos(i)));
            thirdPersonCameraCollided = Physics.Raycast(ray, CollisionRadius, ~IgnoreLayer);

            if (thirdPersonCameraCollided) break;
        }

        useFirstPerson = firstPersonCameraRequested || lineOfSightIsBroken || thirdPersonCameraCollided;

        if (useFirstPerson)
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
        
        firstPersonForward = Quaternion.AngleAxis(horizontalCamera * horizontalViewSpeed, Vector3.up) * firstPersonForward;
        firstPersonForward = Quaternion.AngleAxis(-verticalCamera * verticalViewSpeed, Vector3.Cross(Vector3.up, firstPersonForward)) * firstPersonForward;

        // Restrict the pitch angle of firstPersonForward
        // The firstPersonForward is used by both FirstPersonVCam and ThirdPersonVCam
        // Both Virtual Cameras will look in this direction
        float currentAngle = Mathf.Acos(firstPersonForward.y / firstPersonForward.magnitude);
        if (currentAngle < Mathf.Deg2Rad * ThirdPersonLowestAngle)
        {
            Vector3 newFirstPersonForward = new Vector3(firstPersonForward.x, 0, firstPersonForward.z);
            newFirstPersonForward.y = newFirstPersonForward.magnitude / Mathf.Tan(Mathf.Deg2Rad * ThirdPersonLowestAngle);
            newFirstPersonForward = newFirstPersonForward.normalized;
            firstPersonForward = Vector3.Slerp(firstPersonForward, newFirstPersonForward, 20f * Time.fixedDeltaTime);
        }
        else if (currentAngle > Mathf.Deg2Rad * ThirdPersonHighestAngle)
        {
            Vector3 newFirstPersonForward = new Vector3(firstPersonForward.x, 0, firstPersonForward.z);
            newFirstPersonForward.y = newFirstPersonForward.magnitude / Mathf.Tan(Mathf.Deg2Rad * ThirdPersonHighestAngle);
            newFirstPersonForward = newFirstPersonForward.normalized;
            firstPersonForward = Vector3.Slerp(firstPersonForward, newFirstPersonForward, 20f * Time.fixedDeltaTime);
        }


        //Debug.Log("Curr: " + Mathf.Acos(firstPersonForward.y / firstPersonForward.magnitude) * Mathf.Rad2Deg);
        FirstPersonPosition.LookAt(FirstPersonPosition.position + Vector3.Slerp(FirstPersonPosition.forward, firstPersonForward, 30f * Time.deltaTime));
    }

    public void SetFirstPerson(bool value)
    {
        firstPersonCameraRequested = value;
    }

    public void SetThirdPersonCameraCollision(bool value)
    {
        thirdPersonCameraCollided = value;
    }

    void OnDrawGizmos()
    {

    }
}
