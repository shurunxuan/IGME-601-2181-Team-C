using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamoTool : ToolComponent
{

    // Cost of maintaining the cloak
    [SerializeField]
    public float energyCostPerSecond;

    // Time to transition from fully cloaked to fully uncloaked or vice-versa
    [SerializeField]
    private float transitionSpeed;

    // State of transition to cloaked material (0-1)
    [SerializeField]
    private float transitionState = 0;

    // All MeshRenderers of the drone
    private MeshRenderer[] meshRenderers;

    // The original materials
    private Material[] originalMaterials;

    // Use this for initialization
    void Awake()
    {
        meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        // Backup the original materials
        originalMaterials = new Material[meshRenderers.Length];
        for (int i = 0; i < meshRenderers.Length; ++i)
        {
            originalMaterials[i] = meshRenderers[i].material;
        }
    }

    private void Update()
    {
        float oldState = transitionState;
        float tDir = _toggledOn ? 1 : -1;
        transitionState += tDir * transitionSpeed * Time.deltaTime;
        transitionState = Mathf.Clamp01(transitionState);
        if (oldState != transitionState)
        {
            SetCloakMaterial(transitionState);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_toggledOn && !_energy.TryExpend(energyCostPerSecond * Time.fixedDeltaTime))
        {
            Cancel();
        }
    }

    // Go into cloaked mode
    protected override void Activate()
    {
        if (!_toggledOn)
        {
            _toggledOn = true;
            SetLayers(transform.root, 15);
        }
        else
        {
            Cancel();
        }
    }

    // This tool does not require anything done on cancel
    public override void Cancel()
    {
        _toggledOn = false;
        SetLayers(transform.root, 9);
    }

    // If inactive, cancel the stealth
    public override void SetCurrent(bool state)
    {
        if (!state)
        {
            Cancel();
        }
    }

    // Sets collision layers recursively to handle enemy detection
    // If we change how AI detection works, we will likely need to change this
    private static void SetLayers(Transform obj, int layer)
    {
        for (int i = 0; i < obj.childCount; i++)
        {
            SetLayers(obj.GetChild(i), layer);
        }
        obj.gameObject.layer = layer;
    }

    // Communicates the current state of the object to the material
    private void SetCloakMaterial(float amt)
    {
        // Set the state of the transition
        for (int i = 0; i < meshRenderers.Length; ++i)
        {
            meshRenderers[i].material.SetFloat("_Transition", amt);
        }
    }

    public override string GetName()
    {
        return "Camo Tool";
    }
}
