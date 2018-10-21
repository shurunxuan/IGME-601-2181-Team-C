using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerController : MonoBehaviour
{

    public float TargetRotateSpeed; // In deg/s
    public float ForceFactor;
    public Vector3 Force
    {
        get
        {
            return force;
        }
    }

    private float currentRotateSpeed;
    private float yRotation;
    private Vector3 force;
    // Use this for initialization
    void Start()
    {
        currentRotateSpeed = 0;
        yRotation = transform.localEulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        yRotation += currentRotateSpeed * Time.deltaTime;
        transform.localEulerAngles = new Vector3(0, yRotation, 0);
    }

    void FixedUpdate()
    {
        currentRotateSpeed = Mathf.Lerp(currentRotateSpeed, TargetRotateSpeed, 0.8f * Time.fixedDeltaTime);
        force = currentRotateSpeed * ForceFactor / 100000 * transform.up;
    }

    void OnDrawGizmos()
    {
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position + force / 10);
    }
}
