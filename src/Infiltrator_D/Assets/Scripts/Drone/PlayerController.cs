using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // Inspector Properties
    public float energyLostPerSecond;
    public List<ToolComponent> tools;

    // Component links
    private DroneMovement movement;
    private EnergyComponent energy;
    private ChargePointTool chargeTool;
    private CameraTool cameraTool;

    // Tool Selection
    private int selectedTool;
    private bool toolSet;

    // Use this for initialization
    void Start()
    {
        movement = GetComponent<DroneMovement>();
        energy = GetComponent<EnergyComponent>();

        chargeTool = GetComponentInChildren<ChargePointTool>();
        chargeTool.Assign(energy);

        cameraTool = GetComponentInChildren<CameraTool>();
        cameraTool.Assign(energy);

        selectedTool = 0;
        toolSet = false;

        // Link the tools to the energy component
        for (int i = 0; i < tools.Count; i++)
        {
            tools[i].Assign(energy);
            tools[i].SetCurrent(false);
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

        // Constant energy loss for the frame
        if (movement.EngineOn)
        {
            energy.Expend(energyLostPerSecond * Time.deltaTime);
        }

        // Tool logic
        if(Input.GetButton("Cancel"))
        {
            chargeTool.Cancel();
            cameraTool.Cancel();
            if (toolSet)
            {
                if (tools.Count > 0)
                {
                    tools[selectedTool].Cancel();
                }
                tools[selectedTool].SetCurrent(false);
                toolSet = false;
            }
        }

        // Allow movement off of charge points
        if(chargeTool.Connected && !Input.GetButton("ChargeTool") && Input.GetAxisRaw("Up") > 0)
        {
            chargeTool.Cancel();
        }

        // Toggle through equipped non-core tools
        if (Input.GetButtonDown("ToolSelect"))
        {
            if (toolSet)
            {
                tools[selectedTool].SetCurrent(false);
                selectedTool = (selectedTool + 1) % tools.Count;
            }
            else
            {
                // If we don't have a tool equipped, set it to the last tool equipped
                toolSet = true;
            }
            tools[selectedTool].SetCurrent(true);
        }

        // Camera tool is a core tool
        if (Input.GetButtonDown("CameraTool"))
        {
            cameraTool.TryActivate();
            chargeTool.Cancel();
            if (tools.Count > 0)
            {
                tools[selectedTool].Cancel();
            }
        }

        // Charge point tool is a core tool
        if (Input.GetButtonDown("ChargeTool"))
        {
            chargeTool.TryActivate();
            cameraTool.Cancel();
            if (tools.Count > 0)
            {
                tools[selectedTool].Cancel();
            }
        }

        // Use the currently equipped non-core tool
        if (Input.GetButtonDown("UseTool"))
        {
            if (toolSet && tools.Count > 0)
            {
                tools[selectedTool].TryActivate();
                cameraTool.Cancel();
                chargeTool.Cancel();
            }
            else
            {
                // If no tool is set, set the last tool set
                toolSet = true;
                tools[selectedTool].SetCurrent(true);
            }
        }
        
    }
}
