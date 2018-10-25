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


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override void Activate()
    {
        switch (State)
        {
            case CameraToolState.Inactive:
                CameraAim.UseFirstPerson = true;
                State = CameraToolState.Aiming;
                break;
            case CameraToolState.Aiming:
                TryCaptureInfo();
                CameraAim.UseFirstPerson = false;
                State = CameraToolState.Inactive;
                break;
            default:
                break;
        }
    }

    private void TryCaptureInfo()
    {

    }
}
