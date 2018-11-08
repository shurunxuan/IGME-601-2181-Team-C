using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SpecialObjectHighlight : MonoBehaviour
{
    public Material HighlightMaterial;
    public float OutlineWidth;
    public Color HighlightColor;
    public CinemachineVirtualCamera VirtualCamera;

    public bool LookedAt
    {
        get { return lookedAt; }
        set
        {
            if (!value)
            {
                currentWidth = 0.0f;
                propertyBlock.SetFloat("_Outline", 0.0f);
                meshRenderer.SetPropertyBlock(propertyBlock);
            }
            lookedAt = value;
        }
    }

    private bool lookedAt;

    private Renderer meshRenderer;
    private MaterialPropertyBlock propertyBlock;

    private float diff = 0.1f;
    private float currentWidth;

    // Use this for initialization
    void Start()
    {
        meshRenderer = gameObject.GetComponentInParent<Renderer>();
        Material mat = meshRenderer.material;
        propertyBlock = new MaterialPropertyBlock();
        meshRenderer.materials = new[] { mat, HighlightMaterial };
        meshRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor("_OutlineColor", HighlightColor);
        meshRenderer.SetPropertyBlock(propertyBlock);

        currentWidth = 0.0f;
        LookedAt = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (lookedAt)
        {
            currentWidth += diff * Time.deltaTime;
            if (currentWidth > OutlineWidth)
            {
                currentWidth = OutlineWidth;
                diff = -diff;
            }
            else if (currentWidth < 0.0f)
            {
                currentWidth = 0.0f;
                diff = -diff;
            }
            
            propertyBlock.SetFloat("_Outline", currentWidth);
            meshRenderer.SetPropertyBlock(propertyBlock);
        }
    }
}
