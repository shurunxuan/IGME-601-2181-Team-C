using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{

    public Vector3 TargetForce;
    public Vector3 Forward
    {
        set
        {
            forward = value.normalized;
        }
        get
        {
            return forward;
        }
    }
    public Vector3 Gravity
    {
        get
        {
            return gravity;
        }
    }

    private Rigidbody droneRigidbody;

    private PropellerController propellerFR;
    private PropellerController propellerBR;
    private PropellerController propellerFL;
    private PropellerController propellerBL;
    private Vector3 forward;

    private Vector3 gravity;
    // Use this for initialization
    void Start()
    {
        droneRigidbody = gameObject.GetComponent<Rigidbody>();

        propellerFR = transform.Find("PropellerFR").gameObject.GetComponent<PropellerController>();
        propellerFL = transform.Find("PropellerFL").gameObject.GetComponent<PropellerController>();
        propellerBR = transform.Find("PropellerBR").gameObject.GetComponent<PropellerController>();
        propellerBL = transform.Find("PropellerBL").gameObject.GetComponent<PropellerController>();

        gravity = Vector3.down * droneRigidbody.mass;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Vector3 forceDiff = TargetForce + gravity;

        propellerFR.TargetRotateSpeed = gravity.magnitude * 25000 / propellerFR.ForceFactor;
        propellerBR.TargetRotateSpeed = gravity.magnitude * 25000 / propellerBR.ForceFactor;
        propellerFL.TargetRotateSpeed = gravity.magnitude * 25000 / propellerFL.ForceFactor;
        propellerBL.TargetRotateSpeed = gravity.magnitude * 25000 / propellerBL.ForceFactor;

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

        Vector3 localUp;
        if (TargetForce.magnitude < 0.01f)
            localUp = Vector3.up;
        else
            localUp = TargetForce / Vector3.Dot(TargetForce, Vector3.up);
        Quaternion rotate = Quaternion.FromToRotation(Vector3.up, localUp) * Quaternion.FromToRotation(Vector3.forward, Forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotate, 3.0f * Time.deltaTime);

        droneRigidbody.AddForce(gravity);
        droneRigidbody.AddForce(TargetForce);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (droneRigidbody != null)
            Gizmos.DrawLine(transform.position, transform.position + droneRigidbody.velocity / 100);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + gravity / 10);
        Gizmos.DrawLine(transform.position, transform.position + TargetForce / 10);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (gravity + TargetForce) / 10);
    }
}
