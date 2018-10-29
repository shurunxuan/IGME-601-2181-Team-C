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

    // The charge point we are currently looking at
    public GameObject closestChargePoint;

    // Use this for initialization
    void Start()
    {

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
                if (closestChargePoint != null)
                {
                    // Remove focus
                    closestChargePoint.GetComponent<SpecialObjectHighlight>().LookedAt = false;
                    closestChargePoint = null;
                }
            }
            else
            {
                // If there is a previously charge point that is focused on
                if (closestChargePoint != null && closestChargePoint != closest)
                {
                    // Remove focus
                    closestChargePoint.GetComponent<SpecialObjectHighlight>().LookedAt = false;
                }
                closestChargePoint = closest;
                closestChargePoint.GetComponent<SpecialObjectHighlight>().LookedAt = true;
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
        if (closestChargePoint != null)
        {
            Connect(closestChargePoint);
        }
    }

    // For the camera tool, cancel just resets the camera to third person and fixes its state
    public override void Cancel()
    {
        Disconnect();
    }

    // Performs logic for connecting to a validated charge point
    private void Connect(GameObject chargePoint)
    {
        _connected = chargePoint;
        // Remove focus
        chargePoint.GetComponent<SpecialObjectHighlight>().LookedAt = false;
        // Stop engine
        gameObject.GetComponent<DroneMovement>().EngineOn = false;
        gameObject.GetComponent<DroneMovement>().UseGravity = false;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    // Performs logic for disconnecting from the current charge point
    // Should disconnect properly even if the connected object no longer exists
    private void Disconnect()
    {
        _connected = null;
        // Start engine
        gameObject.GetComponent<DroneMovement>().EngineOn = true;
        gameObject.GetComponent<DroneMovement>().UseGravity = true;
    }
}
