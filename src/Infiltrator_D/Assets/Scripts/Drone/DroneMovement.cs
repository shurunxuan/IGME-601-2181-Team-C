using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    public float HorizontalSpeedFactor;
    public float VerticalSpeedFactor;
    public bool EngineOn;
    public float TiltFactor;
    public Vector3 Forward
    {
        set
        {
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

    // Use this for initialization
    void Start()
    {
        droneRigidbody = gameObject.GetComponent<Rigidbody>();

        propellerFR = transform.Find("PropellerFR").gameObject.GetComponent<PropellerController>();
        propellerFL = transform.Find("PropellerFL").gameObject.GetComponent<PropellerController>();
        propellerBR = transform.Find("PropellerBR").gameObject.GetComponent<PropellerController>();
        propellerBL = transform.Find("PropellerBL").gameObject.GetComponent<PropellerController>();

        Gravity = Vector3.down * droneRigidbody.mass;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Vector3 forceDiff = TargetForce + Gravity;

        if (EngineOn)
        {
            propellerFR.TargetRotateSpeed = Gravity.magnitude * 25000 / propellerFR.ForceFactor;
            propellerBR.TargetRotateSpeed = Gravity.magnitude * 25000 / propellerBR.ForceFactor;
            propellerFL.TargetRotateSpeed = Gravity.magnitude * 25000 / propellerFL.ForceFactor;
            propellerBL.TargetRotateSpeed = Gravity.magnitude * 25000 / propellerBL.ForceFactor;

            propellerFR.TargetRotateSpeed += forceDiff.y * 25000 / propellerFR.ForceFactor;
            propellerBR.TargetRotateSpeed += forceDiff.y * 25000 / propellerBR.ForceFactor;
            propellerFL.TargetRotateSpeed += forceDiff.y * 25000 / propellerFL.ForceFactor;
            propellerBL.TargetRotateSpeed += forceDiff.y * 25000 / propellerBL.ForceFactor;

            Vector3 bl_fr = propellerFR.gameObject.transform.position - propellerBL.gameObject.transform.position;
            Vector3 br_fl = propellerFL.gameObject.transform.position - propellerBR.gameObject.transform.position;

            float bl_fr_diff = Vector3.Dot(TargetForce, bl_fr);
            float br_fl_diff = Vector3.Dot(TargetForce, br_fl);

            propellerFR.TargetRotateSpeed -= bl_fr_diff * 25000 / propellerFR.ForceFactor;
            propellerBR.TargetRotateSpeed += br_fl_diff * 25000 / propellerBR.ForceFactor;
            propellerFL.TargetRotateSpeed -= br_fl_diff * 25000 / propellerFL.ForceFactor;
            propellerBL.TargetRotateSpeed += bl_fr_diff * 25000 / propellerBL.ForceFactor;
        }
        else
        {
            propellerFR.TargetRotateSpeed = 0;
            propellerBR.TargetRotateSpeed = 0;
            propellerFL.TargetRotateSpeed = 0;
            propellerBL.TargetRotateSpeed = 0;
        }
        Vector3 localUp;
        if (TargetForce.magnitude < 0.01f)
            localUp = Vector3.up;
        else
        {
            localUp = TargetForce / Vector3.Dot(TargetForce, Vector3.up);
            localUp.x *= TiltFactor;
            localUp.z *= TiltFactor;
        }

        Quaternion tiltRotation = Quaternion.FromToRotation(Vector3.up, localUp);
        Quaternion yawRotation = Quaternion.FromToRotation(Vector3.forward, Forward);
        Quaternion rotate = tiltRotation * yawRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, 3.0f * Time.deltaTime);

        if (UseGravity)
            droneRigidbody.AddForce(Gravity);
        droneRigidbody.AddForce(TargetForce);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (droneRigidbody != null)
            Gizmos.DrawLine(transform.position, transform.position + droneRigidbody.velocity / 10);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Gravity / 10);
        Gizmos.DrawLine(transform.position, transform.position + TargetForce / 10);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (Gravity + TargetForce) / 10);
    }
}
