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

    // Length in seconds of the predicted trajectory
    public float PredictionLength;

    // Time of one step for the prediction of trajectory
    public float PredictionStep;

    // Line for drawing projectory
    private LineRenderer predictedTrajectory;

    // Bounciness of the decoy
    private float bounciness = 0;

    // Use this for initialization
    void Start () {
        predictedTrajectory = GetComponent<LineRenderer>();
        bounciness = DecoyDevice.GetComponent<Collider>().material.bounciness;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Predict the trajectory of the shot
        if (predictedTrajectory != null)
        {
            List<Vector3> positions = new List<Vector3>();
            Vector3 grav = Physics.gravity;
            Vector3 vel = Muzzle.forward * MuzzleSpeed;
            Vector3 pos = Muzzle.position;
            positions.Add(pos);
            for (float i = 0; i < PredictionLength; i += PredictionStep)
            {
                vel += grav * PredictionStep;
                RaycastHit hit;
                float dist = vel.magnitude * PredictionStep;
                if (Physics.Raycast(pos, vel, out hit, dist))
                {
                    positions.Add(hit.point);
                    break;
                }
                pos += vel * PredictionStep;

                positions.Add(pos);
            }
            predictedTrajectory.positionCount = positions.Count;
            predictedTrajectory.SetPositions(positions.ToArray());
        }
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
