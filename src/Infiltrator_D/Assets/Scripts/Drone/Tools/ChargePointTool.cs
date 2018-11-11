using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ChargePointTool : ToolComponent
{
    // The radius used when attempting to find nearby charge points
    public float DetectionRadius;

    // Rate of energy gained per second
    public float ChargeRate;

    // The position on the drone that connects to the charge point
    public Transform DroneConnectionPoint;

    // A layermask for disincluding certain layers from detection.
    // We already know the player isn't a charge point
    public LayerMask DetectionMask;

    // The camera controller
    public VirtualCameraController CameraController;

    // The charge point and it's virtual camera we are currently connected to
    private GameObject connected;
    public bool Connected { get { return connected != null; } }

    private CinemachineVirtualCamera vCam;

    // A private bool that denotes whether we have finished plugging in to the connected charge point
    private bool finishedConnecting;

    // The Highlight Component of the charge point we are currently looking at
    private SpecialObjectHighlight closestChargePointHighlight;

    // DroneMovement Component
    private DroneMovement droneMovement;

    // Rigidbody of the drone
    private Rigidbody droneRigidbody;

    // Info Gatherer of the Drone
    private InfoGatherer droneInfoGatherer;

    // Use this for initialization
    void Start()
    {
        droneMovement = gameObject.GetComponent<DroneMovement>();
        droneRigidbody = gameObject.GetComponent<Rigidbody>();
        droneInfoGatherer = gameObject.GetComponent<InfoGatherer>();
        if(DroneConnectionPoint == null)
        {
            DroneConnectionPoint = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (connected != null)
        {
            if (finishedConnecting)
            {
                _energy.Charge(ChargeRate * Time.deltaTime);
            }
        }
        else
        {
            // If not connected, continuously looking for a closest charge point
            Collider[] overlap = Physics.OverlapSphere(transform.position, DetectionRadius, DetectionMask);
            GameObject closest = null;
            float sqrDist = 0;
            Vector3 pos = transform.root.position;
            // Check for tagged charge points
            foreach (Collider item in overlap)
            {
                if (item.CompareTag("ChargePoint"))
                {
                    // Determine if this is the closest tagged object we've found
                    if (closest == null)
                    {
                        closest = item.gameObject;
                        sqrDist = (pos - closest.transform.position).sqrMagnitude;
                    }
                    else
                    {
                        float itemDist = (pos - item.transform.position).sqrMagnitude;
                        if (itemDist < sqrDist)
                        {
                            closest = item.gameObject;
                            sqrDist = itemDist;
                        }
                    }
                }
            }

            // If there is a previously charge point that is focused on
            if (closestChargePointHighlight != null)
            {
                if (closest == null)
                {
                    // No Hit
                    // Remove current focus
                    closestChargePointHighlight.LookedAt = false;
                    closestChargePointHighlight = null;

                }
                else
                {
                    // Hit a thing
                    // We want the new one
                    if (closestChargePointHighlight.gameObject != closest)
                    {
                        // Remove old focus
                        closestChargePointHighlight.LookedAt = false;
                        // Get the new component
                        closestChargePointHighlight = closest.GetComponent<SpecialObjectHighlight>();
                    }
                    // Animate that charge point anyway
                    closestChargePointHighlight.LookedAt = true;
                }
            }
            else if (closest != null)
            {
                // There is not a previously charge point that is focused on
                // Get the new component if it exists
                closestChargePointHighlight = closest.GetComponent<SpecialObjectHighlight>();
            }
        }
    }

    void FixedUpdate()
    {
        if (connected != null && !finishedConnecting)
        {
            Transform root = transform.root;
            Vector3 targetPosition = connected.transform.position - (DroneConnectionPoint.transform.position - root.transform.position);
            root.position = Vector3.Lerp(root.position, targetPosition, 3.75f * Time.fixedDeltaTime);
            // Ensure minimum movement
            root.position = Vector3.MoveTowards(root.position, targetPosition, .0001f);
            //root.rotation = Quaternion.RotateTowards(root.rotation, connected.transform.rotation, 15);

            // Check if our animation is finished
            if (root.position == targetPosition)// && root.rotation == connected.transform.rotation)
            {
                // If we are close enough to the charge point, jump the last bit and attempt to hack
                root.position = targetPosition;
                finishedConnecting = true;
                TryHack();
            }
        }
    }

    // Attempt to connect to a nearby charge point
    protected override void Activate()
    {
        // Disconnect if connected
        if (connected != null)
        {
            Disconnect();
            return;
        }
        // Connect if unconnected
        // Check if we have found a closest charge point
        if (closestChargePointHighlight != null)
        {
            Connect(closestChargePointHighlight);
        }
    }

    // For the camera tool, cancel just resets the camera to third person and fixes its state
    public override void Cancel()
    {
        Disconnect();
    }

    // Performs logic for connecting to a validated charge point
    private void Connect(SpecialObjectHighlight chargePoint)
    {
        connected = chargePoint.gameObject;
        // Remove focus
        chargePoint.LookedAt = false;
        // Stop engine
        droneMovement.EngineOn = false;
        droneMovement.UseGravity = false;
        droneRigidbody.velocity = Vector3.zero;
        // Find the Virtual Camera
        vCam = chargePoint.gameObject.transform.Find("ChargePointVCam").gameObject.GetComponent<CinemachineVirtualCamera>();
        // Activate it
        vCam.Priority = 11;
        // Disable the Virtual Camera Controller of the drone
        CameraController.enabled = false;

        droneRigidbody.isKinematic = true;
        finishedConnecting = false;
    }

    // Performs logic for disconnecting from the current charge point
    // Should disconnect properly even if the connected object no longer exists
    private void Disconnect()
    {
        connected = null;
        // Start engine
        droneMovement.EngineOn = true;
        droneMovement.UseGravity = true;
        // Deactivate the Virtual Camera
        if (vCam != null)
        {
            vCam.Priority = -1;
        }
        // Enable the Virtual Camera Controller of the drone
        CameraController.enabled = true;

        droneRigidbody.isKinematic = false;
        finishedConnecting = false;
    }

    // Attempts to hack into connected
    private void TryHack()
    {
        // Sanity check for being connected
        if(connected == null)
        {
            return;
        }

        // Attempt to hack
        TopSecretInfo hackInfo = connected.GetComponent<TopSecretInfo>();
        if (hackInfo != null && hackInfo.type == TopSecretInfo.InfoType.Digital)
        {
            droneInfoGatherer.AddInfo(hackInfo.info);
        }

        HackableDetector hackDetector = connected.GetComponent<HackableDetector>();
        if (hackDetector != null)
        {
            hackDetector.Hack();
        }
    }
}
