using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerTool : ToolComponent {

    [SerializeField]
    private Material scanMaterial;

    [SerializeField]
    private float maxScanRange;

    [SerializeField]
    private float scanSpeed;

    private Transform origin;
    private ScannerCameraEffect cameraEffect;

    private bool scanning = false;
    private float currentScanDistance = 0;

    // Use this for initialization
    void Start ()
    {
        // Create transform for managing point of origin
        origin = new GameObject("Scanner Origin").GetComponent<Transform>();

        // Add shader controller to main camera
        cameraEffect = Camera.main.gameObject.AddComponent<ScannerCameraEffect>();
        cameraEffect.SetMaterial(scanMaterial);
        cameraEffect.SetOrigin(origin);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (scanning)
        {
            // Update scan distance
            if (currentScanDistance > maxScanRange)
            {
                scanning = false;
                currentScanDistance = 0;
            } else
            {
                currentScanDistance += scanSpeed * Time.deltaTime;
            }
            // Set shader value
            cameraEffect.SetScanDistance(currentScanDistance);
        }
    }

    protected override void Activate()
    {
        origin.position = this.transform.position;
        currentScanDistance = 0;
        scanning = true;
    }

    public override void Cancel()
    {
        
    }

}
