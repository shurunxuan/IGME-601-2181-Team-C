using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargePointTool : ToolComponent {

    // The radius used when attempting to find nearby charge points
    public float DetectionRadius;

    // Rate of energy gained per second
    public float ChargeRate;

    // A layermask for disincluding certain layers from detection.
    // We already know the player isn't a charge point
    public LayerMask DetectionMask;

    // The charge point we are currently connected to
    private GameObject _connected;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Temp code
        // Final code should be more fluid will need to communicate with other components
        if(_connected != null)
        {
            _energy.Charge(ChargeRate * Time.deltaTime);
            Transform root = transform.root;
            root.position = Vector3.MoveTowards(root.position, _connected.transform.position, 20 * Time.deltaTime);
        }
	}

    // Attempt to connect to a nearby charge point
    protected override void Activate()
    {
        Collider[] overlap = Physics.OverlapSphere(transform.position, DetectionRadius, DetectionMask);
        foreach (Collider item in overlap)
        {
            if (item.CompareTag("ChargePoint"))
            {
                Connect(item.gameObject);
            }
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
    }

    // Performs logic for disconnecting from the current charge point
    // Should disconnect properly even if the connected object no longer exists
    private void Disconnect()
    {
        _connected = null;
    }
}
