using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // Inspector Properties
    public float EnergyLostPerSecond;
    public float WeaponWheelHoldButtonTime;
    public WeaponWheelController WeaponWheel;
    public List<ToolComponent> Tools;
    public VirtualCameraController CameraController;
    public float WeaponWheelTimeScale;

    // Component links
    private DroneMovement movement;
    private EnergyComponent energy;
    private ChargePointTool chargeTool;
    private CameraTool cameraTool;
    // Instead of the tools list in the inspector,
    // we maintain this list for tool changing logic.
    private List<ToolComponent> allTools;
    private bool isAiming;
    // Weapon Wheel Things
    private float holdButtonTimer;
    private bool weaponWheelShowing;
    private int currentWheelSelection;
    // 4 tool directions
    private List<Vector2> directions;
    private Vector2 directionalInputIntegral;

    public int CurrentTool { get; private set; }

    // Tracks if we died
    private bool live;

    // Use this for initialization
    void Start()
    {
        live = true;

        // De-parent to avoid issues
        transform.parent = null;

        movement = GetComponent<DroneMovement>();
        energy = GetComponent<EnergyComponent>();

        chargeTool = GetComponentInChildren<ChargePointTool>();
        chargeTool.Assign(energy);

        cameraTool = GetComponentInChildren<CameraTool>();
        cameraTool.Assign(energy);

        CurrentTool = 0;

        // Link the tools to the energy component
        foreach (var t in Tools)
        {
            t.Assign(energy);
            t.SetCurrent(false);
        }

        allTools = new List<ToolComponent>(4)
        {
            // Up
            cameraTool,
            // Right
            Tools[0],
            // Down
            chargeTool,
            // Left
            Tools[1]
        };

        UIToolTracker.ActiveInScene.SetCurrentTool(allTools[CurrentTool]);
        holdButtonTimer = WeaponWheelHoldButtonTime;
        weaponWheelShowing = false;
        currentWheelSelection = -1;

        directions = new List<Vector2>(4)
        {
            new Vector2(+0.0f, +1.0f),
            new Vector2(+1.0f, +0.0f),
            new Vector2(+0.0f, -1.0f),
            new Vector2(-1.0f, +0.0f),
        };
        directionalInputIntegral = new Vector2(0.0f, 0.0f);
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
            energy.Expend(EnergyLostPerSecond * Time.deltaTime);
            if (energy.CurrentEnergy <= 0)
            {
                movement.FallToDeath();

                live = false;
                if (Tools.Count > 0)
                {
                    Tools[CurrentTool].Cancel();
                    Tools[CurrentTool].SetCurrent(false);
                    UIDeathTracker.ActiveInScene.Show(UIDeathTracker.DeathTypes.EnergyLoss);
                }
            }
        }

        // Tool logic
        if (live)
        {
            // Allow movement off of charge points
            if (chargeTool.Connected && !Input.GetButton("UseTool") && Input.GetAxisRaw("Up") > 0)
            {
                chargeTool.Cancel();
            }

            // Toggle through equipped non-core tools
            if (Input.GetButton("ToolSelect") && !weaponWheelShowing)
            {
                holdButtonTimer -= Time.deltaTime;
                if (holdButtonTimer <= 0)
                {
                    weaponWheelShowing = true;

                    // Setup Weapon Wheel
                    // Assume we have exactly 4 tools
                    // Subject to change
                    for (int i = 0; i < Math.Min(4, allTools.Count); ++i)
                    {
                        WeaponWheel.Icons[i].sprite = allTools[i].Icon;
                    }

                    // Show Weapon Wheel
                    WeaponWheel.gameObject.SetActive(true);

                    // Change Time
                    Time.timeScale = WeaponWheelTimeScale;

                    // Stop camera movement
                    CameraController.enabled = false;
                }
            }

            if (Input.GetButtonUp("ToolSelect") && weaponWheelShowing)
            {
                holdButtonTimer = WeaponWheelHoldButtonTime;
                weaponWheelShowing = false;
                // Hide Weapon Wheel
                WeaponWheel.gameObject.SetActive(false);
                directionalInputIntegral = new Vector2(0.0f, 0.0f);
                // Select Tool
                if (currentWheelSelection >= 0)
                {
                    CurrentTool = currentWheelSelection;

                    for (int i = 0; i < 4; ++i)
                    {
                        allTools[i].Cancel();
                        allTools[i].SetCurrent(i == CurrentTool);
                    }

                    UIToolTracker.ActiveInScene.SetCurrentTool(allTools[CurrentTool]);
                }
                // Reset
                currentWheelSelection = -1;

                Time.timeScale = 1.0f;

                CameraController.enabled = true;
            }

            // When weapon wheel is showing
            if (weaponWheelShowing)
            {
                // Response to the "Directional Input"
                float wheelX = Input.GetAxis("WeaponWheelX");
                float wheelY = Input.GetAxis("WeaponWheelY");

                // Integral it
                Vector2 delta = new Vector2(wheelX, wheelY);
                directionalInputIntegral += delta;

                // Limit the input
                if (directionalInputIntegral.magnitude > 1.0f)
                    directionalInputIntegral.Normalize();

                // Has directional input
                if (directionalInputIntegral.magnitude > 0.8f)
                {
                    delta.Normalize();
                    float maxDot = -1.0f;

                    // Hard code tool count again
                    for (int i = 0; i < 4; ++i)
                    {
                        float dot = Vector2.Dot(directionalInputIntegral, directions[i]);
                        if (dot > maxDot)
                        {
                            maxDot = dot;
                            currentWheelSelection = i;
                        }
                    }
                }
                else
                {
                    currentWheelSelection = -1;
                }

                // Set weapon wheel response
                WeaponWheel.Selecting(currentWheelSelection);
            }

            // Use the currently equipped tool
            if (Input.GetButtonDown("UseTool"))
            {
                if ((Input.GetButton("ReadyTool") && allTools[CurrentTool].NeedAiming) || !allTools[CurrentTool].NeedAiming)
                {
                    allTools[CurrentTool].TryActivate();
                }
            }

            // Aim the currently equipped tool
            if (Input.GetButtonDown("ReadyTool") && allTools[CurrentTool].NeedAiming)
            {
                allTools[CurrentTool].TryActivate();
            }
            // Cancel aiming
            if (Input.GetButtonUp("ReadyTool") && allTools[CurrentTool].NeedAiming)
            {
                allTools[CurrentTool].Cancel();
            }

            for (int i = 0; i < 4; ++i)
            {
                allTools[i].Cooldown();
            }
        }


    }
}
