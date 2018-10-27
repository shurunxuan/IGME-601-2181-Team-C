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

    protected override void Activate()
    {
        switch (State)
        {
            case CameraToolState.Inactive:
                CameraAim.SetFirstPerson(true);
                State = CameraToolState.Aiming;
                break;
            case CameraToolState.Aiming:
                if (TryCaptureInfo())
                {
                    Cancel();
                }
                break;
            default:
                break;
        }
    }

    protected override void Cancel()
    {
        switch (State)
        {
            case CameraToolState.Aiming:
                TryCaptureInfo();
                CameraAim.SetFirstPerson(false);
                State = CameraToolState.Inactive;
                break;
            default:
                break;
        }
    }

    private bool TryCaptureInfo()
    {
        if(_info == null || CameraAim == null)
        {
            Debug.Log("Missing connected components on camera tool.");
            return false;
        }

        RaycastHit hit;
        if (Physics.Raycast(CameraAim.transform.position, CameraAim.LookAtDirection, out hit, 10000000, CameraMask))
        {
            TopSecretInfo inf = hit.collider.GetComponent<TopSecretInfo>();
            if (inf != null)
            {
                return _info.AddInfo(inf.info);
            }
        }
        return false;
    }
}
