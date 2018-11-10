using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    public float HorizontalSpeedFactor;
    public float VerticalSpeedFactor;
    public float SpeedFactor;
    public bool EngineOn;
    public float TiltFactor;
    public Vector3 Forward
    {
        set
        {
            // The drone will not rotate if the engine is off
            if (EngineOn)
                forward = value.normalized;
        }
        get
        {
            return forward;
        }
    }

    public bool UseGravity;
    public Vector3 Gravity { get; private set; }

    public bool SkipLerpRotation { get; set; }

    public Vector3 TargetForce
    {
        get
        {
            return targetForce;
        }

        set { targetForce = EngineOn ? value : Vector3.zero; }
    }

    private Rigidbody droneRigidbody;

    private PropellerController propellerFR;
    private PropellerController propellerBR;
    private PropellerController propellerFL;
    private PropellerController propellerBL;
    private Vector3 forward;

    private Vector3 targetForce;

    private bool inTransition;

    // Use this for initialization
    void Start()
    {
        droneRigidbody = gameObject.GetComponent<Rigidbody>();

        propellerFR = transform.Find("PropellerFR").gameObject.GetComponent<PropellerController>();
        propellerFL = transform.Find("PropellerFL").gameObject.GetComponent<PropellerController>();
        propellerBR = transform.Find("PropellerBR").gameObject.GetComponent<PropellerController>();
        propellerBL = transform.Find("PropellerBL").gameObject.GetComponent<PropellerController>();

        Gravity = Vector3.down * droneRigidbody.mass;

        inTransition = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        // The actual force that will apply to the drone
        // Making the total force = TargetForce
        Vector3 forceDiff = TargetForce + Gravity;

        if (EngineOn)
        {
            // If the engine is on, then calculate the rotate speed of the propellers.

            // Cancel the effect of gravity
            propellerFR.TargetRotateSpeed = Gravity.magnitude * 25000 / propellerFR.ForceFactor;
            propellerBR.TargetRotateSpeed = Gravity.magnitude * 25000 / propellerBR.ForceFactor;
            propellerFL.TargetRotateSpeed = Gravity.magnitude * 25000 / propellerFL.ForceFactor;
            propellerBL.TargetRotateSpeed = Gravity.magnitude * 25000 / propellerBL.ForceFactor;

            // Apply the forceDiff to the speed
            propellerFR.TargetRotateSpeed += forceDiff.y * 25000 / propellerFR.ForceFactor;
            propellerBR.TargetRotateSpeed += forceDiff.y * 25000 / propellerBR.ForceFactor;
            propellerFL.TargetRotateSpeed += forceDiff.y * 25000 / propellerFL.ForceFactor;
            propellerBL.TargetRotateSpeed += forceDiff.y * 25000 / propellerBL.ForceFactor;

            // Calculate the difference of the positions of the diagonal propellers
            Vector3 bl_fr = propellerFR.gameObject.transform.position - propellerBL.gameObject.transform.position;
            Vector3 br_fl = propellerFL.gameObject.transform.position - propellerBR.gameObject.transform.position;
            // Project the TargetForce
            float bl_fr_diff = Vector3.Dot(TargetForce, bl_fr);
            float br_fl_diff = Vector3.Dot(TargetForce, br_fl);
            // Apply the projected TargetForce
            // This will make the speed different if the drone is tilted
            propellerFR.TargetRotateSpeed -= bl_fr_diff * 25000 / propellerFR.ForceFactor;
            propellerBR.TargetRotateSpeed += br_fl_diff * 25000 / propellerBR.ForceFactor;
            propellerFL.TargetRotateSpeed -= br_fl_diff * 25000 / propellerFL.ForceFactor;
            propellerBL.TargetRotateSpeed += bl_fr_diff * 25000 / propellerBL.ForceFactor;
        }
        else
        {
            // The engine is stopped, so rotate speed is zero
            propellerFR.TargetRotateSpeed = 0;
            propellerBR.TargetRotateSpeed = 0;
            propellerFL.TargetRotateSpeed = 0;
            propellerBL.TargetRotateSpeed = 0;
        }
        Vector3 localUp;
        if (TargetForce.magnitude < 0.01f)
        {
            // If TargetForce is too small, set localUp to global up
            localUp = Vector3.up;
        }
        else
        {
            // Otherwise, tilt the drone
            // The y component is the same, but the x-z components are affected by the TiltFactor
            localUp = TargetForce / Vector3.Dot(TargetForce, Vector3.up);
            localUp.x *= TiltFactor;
            localUp.z *= TiltFactor;
        }
        if (!inTransition)
        {
            // Tilt
            Quaternion tiltRotation = Quaternion.FromToRotation(Vector3.up, localUp);
            // Look Direction
            Quaternion yawRotation = Quaternion.FromToRotation(Vector3.forward, Forward);
            // Combine rotation
            Quaternion rotate = tiltRotation * yawRotation;
            transform.rotation = SkipLerpRotation
                ? rotate
                : Quaternion.Slerp(transform.rotation, rotate, 3.0f * Time.deltaTime);
        }

        if (UseGravity)
            droneRigidbody.AddForce(Gravity * SpeedFactor);
        droneRigidbody.AddForce(TargetForce * SpeedFactor);

        //FirstPersonPosition.LookAt(FirstPersonPosition.position + Forward);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (droneRigidbody != null)
            Gizmos.DrawLine(transform.position, transform.position + droneRigidbody.velocity);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Gravity);
        Gizmos.DrawLine(transform.position, transform.position + TargetForce);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (Gravity + TargetForce));
    }

    public void RotateTo(Transform target)
    {
        forward = Vector3.Cross(Camera.main.transform.right, Camera.main.transform.up);
        inTransition = true;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, 5);
    }

    public void StopTransition()
    {
        //transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up, 0.1f);
        inTransition = false;
    }
}
