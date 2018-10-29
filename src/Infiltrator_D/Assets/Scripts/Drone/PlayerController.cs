using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // Inspector Properties
    public List<ToolComponent> tools;
    public List<string> buttonBinds;

    // Component links
    private DroneMovement movement;
    private EnergyComponent energy;

    // Use this for initialization
    void Start()
    {
        movement = GetComponent<DroneMovement>();
        energy = GetComponent<EnergyComponent>();

        // Link the tools to the energy component
        for (int i = 0; i < tools.Count; i++)
        {
            tools[i].Assign(energy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float right = Input.GetAxis("Right");
        float forward = Input.GetAxis("Forward");
        float up = Input.GetAxis("Up");

        // The right vector in local x-z plane
        Vector3 localRight = Vector3.Cross(Vector3.up, movement.Forward);
        // Apply the horizontal movement, vertical movement and customized gravity
        movement.TargetForce = (forward * movement.Forward + right * localRight) * movement.HorizontalSpeedFactor
                               + up * Vector3.up * movement.VerticalSpeedFactor
                               - (movement.UseGravity ? movement.Gravity : Vector3.zero);

        // Tool logic
        bool cancel = Input.GetButton("Cancel");
        for (int i = 0; i < tools.Count && i < buttonBinds.Count; i++)
        {
            if (cancel)
            {
                tools[i].Cancel();
            }
            else if (Input.GetButtonDown(buttonBinds[i]))
            {
                tools[i].TryActivate();
            }
        }
    }
}
