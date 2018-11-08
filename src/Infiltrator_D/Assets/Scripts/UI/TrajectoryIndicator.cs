using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryIndicator : MonoBehaviour {

    // The speed at which the projectile leaves the muzzle
    public float MuzzleSpeed;

    // Length in seconds of the predicted trajectory
    public float PredictionLength;

    // Time of one step for the prediction of trajectory
    public float PredictionStep;

    // The indicator for the landing position
    public PositionIndicator positionIndicator;

    // The layer mask for what we can collide with
    public LayerMask detectionLayer;

    // Line for drawing projectory
    private LineRenderer predictedTrajectory;

    // Use this for initialization
    void Start () {
        predictedTrajectory = GetComponent<LineRenderer>();
        if(positionIndicator != null)
        {
            positionIndicator.Disappear();
        }
    }

    // Update is called once per frame
    void Update () {
        // Clear pos indicator
        if (positionIndicator != null)
        {
            positionIndicator.Disappear();
        }
        // Predict the trajectory of the shot
        if (predictedTrajectory != null)
        {
            List<Vector3> positions = new List<Vector3>();
            Vector3 grav = Physics.gravity;
            Vector3 vel = transform.forward * MuzzleSpeed;
            Vector3 pos = transform.position;
            positions.Add(pos);
            for (float i = 0; i < PredictionLength; i += PredictionStep)
            {
                vel += grav * PredictionStep;
                RaycastHit hit;
                float dist = vel.magnitude * PredictionStep;
                if (Physics.Raycast(pos, vel, out hit, dist, detectionLayer))
                {
                    positions.Add(hit.point);
                    if (positionIndicator != null)
                    {
                        positionIndicator.Appear(hit.point, hit.normal);
                    }
                    break;
                }
                pos += vel * PredictionStep;

                positions.Add(pos);
            }
            predictedTrajectory.positionCount = positions.Count;
            predictedTrajectory.SetPositions(positions.ToArray());
        }


    }
}
