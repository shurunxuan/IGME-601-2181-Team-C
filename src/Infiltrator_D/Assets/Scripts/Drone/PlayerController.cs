using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Inspector Properties
    public List<ToolComponent> tools;
    public List<string> buttonBinds;

    // Component links
    private DroneMovement movement;
    private EnergyComponent energy;
    
    // Use this for initialization
	void Start () {
		movement = GetComponent<DroneMovement>();
        energy = GetComponent<EnergyComponent>();

        // Link the tools to the energy component
        for (int i = 0; i < tools.Count; i++)
        {
            tools[i].Assign(energy);
        }
    }
	
	// Update is called once per frame
	void Update () {
        float right = Input.GetAxis("Right");
        float forward = Input.GetAxis("Forward");
		float up = Input.GetAxis("Up");

        movement.TargetForce = (forward * 2 * movement.Forward + up * Vector3.up + right * 2 * Vector3.Cross(Vector3.up, movement.Forward) - movement.Gravity) * movement.SpeedFactor;

        // Tool logic
        for (int i = 0; i < tools.Count && i < buttonBinds.Count; i++)
        {
            if(Input.GetButtonDown(buttonBinds[i]))
            {
                tools[i].TryActivate();
            }
        }
    }
}
