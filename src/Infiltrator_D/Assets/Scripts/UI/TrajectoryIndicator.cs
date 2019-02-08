using System.Collections.Generic;
using UnityEngine;

public class TrajectoryIndicator : MonoBehaviour
{

    // The speed at which the projectile leaves the muzzle
    public float MuzzleSpeed;

    // Length in seconds of the predicted trajectory
    public float PredictionLength;

    // Time of one step for the prediction of trajectory
    public float PredictionStep;

    // The indicator for the landing position
    public PositionIndicator PositionIndicator;

    // The layer mask for what we can collide with
    public LayerMask DetectionLayer;

    // Keep tracking the current camera direction
    public Transform CameraDirection;

    // Line for drawing projectory
    private LineRenderer predictedTrajectory;

    // Use this for initialization
    void Awake()
    {
        predictedTrajectory = GetComponent<LineRenderer>();
        if (PositionIndicator != null)
        {
            PositionIndicator.Disappear();
        }
        Disappear();
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = CameraDirection.eulerAngles;
        // Clear pos indicator
        if (PositionIndicator != null)
        {
            PositionIndicator.Disappear();
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
                if (Physics.Raycast(pos, vel, out hit, dist, DetectionLayer))
                {
                    positions.Add(hit.point);
                    if (PositionIndicator != null)
                    {
                        PositionIndicator.Appear(hit.point, hit.normal);
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

    // Disappear from game
    public void Disappear()
    {
        if (PositionIndicator != null)
        {
            PositionIndicator.Disappear();
            PositionIndicator.enabled = false;
        }
        if (predictedTrajectory != null)
        {
            predictedTrajectory.enabled = false;
        }
        enabled = false;
    }

    // Appear 
    public void Appear()
    {
        if (PositionIndicator != null)
        {
            PositionIndicator.enabled = true;
        }
        if (predictedTrajectory != null)
        {
            predictedTrajectory.enabled = true;
            predictedTrajectory.positionCount = 0;
        }
        enabled = true;
    }

}
