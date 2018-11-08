using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyTool : ToolComponent {

    // Transform used to aim and position the shot
    public Transform Muzzle;

    // The device launched by the tool
    public GameObject DecoyDevice;
    
    // The speed at which the decoy leaves the muzzle
    public float MuzzleSpeed;

    // The TrajectoryIndicatior for this tools trajectory
    private TrajectoryIndicator indicator;

    // Use this for initialization
    void Start()
    {
        indicator = Muzzle.GetComponent<TrajectoryIndicator>();
        if (indicator != null)
        {
            indicator.MuzzleSpeed = MuzzleSpeed;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    // Launch a decoy pellet
    protected override void Activate()
    {
        GameObject obj = Instantiate(DecoyDevice);
        obj.transform.SetPositionAndRotation(Muzzle.position, Muzzle.rotation);
        Rigidbody rigid = obj.GetComponent<Rigidbody>();
        rigid.velocity = Muzzle.forward * MuzzleSpeed;
    }

    // For the camera tool, cancel just resets the camera to third person and fixes its state
    public override void Cancel()
    {
        
    }
}
