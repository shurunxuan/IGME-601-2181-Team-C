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

    // The rigidbody of the drone, used to create relative velocity
    private Rigidbody rigid;

    // Use this for initialization
    void Awake()
    {
        indicator = Muzzle.GetComponent<TrajectoryIndicator>();
        if (indicator != null)
        {
            indicator.MuzzleSpeed = MuzzleSpeed;
        }
        rigid = GetComponentInParent<Rigidbody>();
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
        Rigidbody objRigid = obj.GetComponent<Rigidbody>();
        objRigid.velocity = rigid.velocity + (Muzzle.forward * MuzzleSpeed);
    }

    // This tool does not require anything done on cancel
    public override void Cancel()
    {
        
    }

    // Indicator should only display when active
    public override void SetCurrent(bool state)
    {
        if(state)
        {
            indicator.Appear();
        }
        else
        {
            indicator.Disappear();
        }
    }

    public override string GetName()
    {
        return "Decoy Tool";
    }
}
