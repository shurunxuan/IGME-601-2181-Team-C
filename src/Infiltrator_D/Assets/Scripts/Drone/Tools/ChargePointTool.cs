using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargePointTool : ToolComponent
{

    // The radius used when attempting to find nearby charge points
    public float DetectionRadius;

    // Rate of energy gained per second
    public float ChargeRate;

    // The camera used for aiming at the charge point
    public CameraController CameraAim;

    // A layermask for disincluding certain layers from detection.
    // We already know the player isn't a charge point
    public LayerMask DetectionMask;

    // The charge point we are currently connected to
    private GameObject _connected;

    // The Highlight Component of the charge point we are currently looking at
    private SpecialObjectHighlight closestChargePointHighlight;

    // DroneMovement Component
    private DroneMovement droneMovement;

    // Rigidbody of the drone
    private Rigidbody droneRigidbody;

    // Use this for initialization
    void Start()
    {
        droneMovement = gameObject.GetComponent<DroneMovement>();
        droneRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Temp code
        // TODO: Final code should be more fluid will need to communicate with other components
        if (_connected != null)
        {
            _energy.Charge(ChargeRate * Time.deltaTime);
            //Transform root = transform.root;
            //root.position = Vector3.Lerp(root.position, _connected.transform.position, 2 * Time.deltaTime);
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

            if (closest == null)
            {
                // No Hit
                // If there is a previously charge point that is focused on
                if (closestChargePointHighlight != null)
                {
                    // Remove focus
                    closestChargePointHighlight.LookedAt = false;
                    closestChargePointHighlight = null;
                }
            }
            else
            {
                // If there is a previously charge point that is focused on
                if (closestChargePointHighlight != null && closestChargePointHighlight.gameObject != closest)
                {
                    // Remove focus
                    closestChargePointHighlight.LookedAt = false;
                }
                closestChargePointHighlight = closest.GetComponent<SpecialObjectHighlight>();
                closestChargePointHighlight.LookedAt = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (_connected != null)
        {
            Transform root = transform.root;
            root.position = Vector3.Lerp(root.position, _connected.transform.position, 2 * Time.deltaTime);
        }
    }

    // Attempt to connect to a nearby charge point
    protected override void Activate()
    {
        // Disconnect if connected
        if (_connected != null)
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
        _connected = chargePoint.gameObject;
        // Remove focus
        chargePoint.LookedAt = false;
        // Stop engine
        droneMovement.EngineOn = false;
        droneMovement.UseGravity = false;
        droneRigidbody.velocity = Vector3.zero;
    }

    // Performs logic for disconnecting from the current charge point
    // Should disconnect properly even if the connected object no longer exists
    private void Disconnect()
    {
        _connected = null;
        // Start engine
        droneMovement.EngineOn = true;
        droneMovement.UseGravity = true;
    }
}
