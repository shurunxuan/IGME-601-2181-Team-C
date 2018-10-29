using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTool : ToolComponent {

    public enum CameraToolState
    {
        Inactive, Aiming
    }

    // The camera used for aiming the tool
    public CameraController CameraAim;
    // The layer mask for capturing info
    public LayerMask CameraMask;

    // Tool internal
    public CameraToolState State { get; private set; }

    // Linked Components
    private InfoGatherer _info;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Assign(EnergyComponent energy)
    {
        base.Assign(energy);
        _info = energy.GetComponent<InfoGatherer>();
    }

    // Enter first person mode. If we are already in First person mode, try to capture info.
    protected override void Activate()
    {
        switch (State)
        {
            case CameraToolState.Inactive:
                CameraAim.SetFirstPerson(true);
                State = CameraToolState.Aiming;
                break;
            case CameraToolState.Aiming:
                TryCaptureInfo();
                Cancel();
                break;
            default:
                break;
        }
    }

    // For the camera tool, cancel just resets the camera to third person and fixes its state
    public override void Cancel()
    {
        switch (State)
        {
            case CameraToolState.Aiming:
                CameraAim.SetFirstPerson(false);
                State = CameraToolState.Inactive;
                break;
            default:
                break;
        }
    }

    // Attempts to take a picture
    private bool TryCaptureInfo()
    {
        // Ensure neccesary components are connected
        if(_info == null || CameraAim == null)
        {
            Debug.Log("Missing connected components on camera tool.");
            return false;
        }

        // Raycast from the camera in a straight line
        RaycastHit hit;
        if (Physics.Raycast(CameraAim.transform.position, CameraAim.LookAtDirection, out hit, 10000000, CameraMask))
        {
            // Check for visual info
            TopSecretInfo inf = hit.collider.GetComponent<TopSecretInfo>();
            if (inf != null && inf.type == TopSecretInfo.InfoType.Visual)
            {
                // Visual info was found, feed it to the InfoGatherer
                return _info.AddInfo(inf.info);
            }
        }
        return false;
    }
}
