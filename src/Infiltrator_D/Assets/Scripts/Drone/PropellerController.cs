using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerController : MonoBehaviour
{
    // The target speed of rotation in Degree/Sec
    public float TargetRotateSpeed;

    // The factor that represents the relationship between 
    // TargetRotateSpeed and the force that will be applied 
    // to the drone.
    public float ForceFactor;

    // The force that the propeller provides
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
