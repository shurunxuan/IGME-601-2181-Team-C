using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamoTool : ToolComponent {

    // Cost of maintaining the cloak
    public float EnergyCostPerSecond;

    // Material connected to the shader
    public Material CloakMaterial;

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

    // Update is called once per frame
    void FixedUpdate ()
    {
        if (_toggledOn && !_energy.TryExpend(EnergyCostPerSecond * Time.fixedDeltaTime))
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
            SetCloakMaterial(true);
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
        SetCloakMaterial(false);
    }

    // If inactive, cancel the stealth
    public override void SetCurrent(bool state)
    {
        if(!state)
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
    private void SetCloakMaterial(bool state)
    {
        // Set the material and layer
        for (int i = 0; i < meshRenderers.Length; ++i)
        {
            meshRenderers[i].material = state ? CloakMaterial : originalMaterials[i];
            // Find the Mesh Renderer that is layered with Player
            if (meshRenderers[i].gameObject.layer == LayerMask.NameToLayer(state ? "Player" : "InvisiblePlayer"))
            {
                // Set it to InvisiblePlayer
                meshRenderers[i].gameObject.layer = LayerMask.NameToLayer(state ? "InvisiblePlayer" : "Player");
            }
        }
    }

    public override string GetName()
    {
        return "Camo Tool";
    }
}
