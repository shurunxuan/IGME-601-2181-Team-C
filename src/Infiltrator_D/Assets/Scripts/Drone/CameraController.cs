using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject FollowingObject;
    public Vector3 LookAtDirection;

    public float HorizontalViewSpeed;
    public float VerticalViewSpeed;

    private bool firstPerson = false;
    public Transform FirstPersonPosition;

    private Vector3 targetPosition;
    private float distance;
    // Use this for initialization
    void Start()
    {
        LookAtDirection = FollowingObject.transform.position + 0.1f * Vector3.up - transform.position;
        distance = LookAtDirection.magnitude;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontalCamera = Input.GetAxis("HorizontalCamera");
        float verticalCamera = Input.GetAxis("VerticalCamera");

        LookAtDirection = Quaternion.AngleAxis(horizontalCamera * HorizontalViewSpeed, Vector3.up) * LookAtDirection;
        LookAtDirection = Quaternion.AngleAxis(-verticalCamera * VerticalViewSpeed, Vector3.Cross(Vector3.up, LookAtDirection)) * LookAtDirection;
        if (Mathf.Acos(LookAtDirection.y / LookAtDirection.magnitude) < Mathf.Deg2Rad * 10)
        {
            LookAtDirection = new Vector3(LookAtDirection.x, 0, LookAtDirection.z);
            LookAtDirection.y = LookAtDirection.magnitude / Mathf.Tan(Mathf.Deg2Rad * 10);
            LookAtDirection = LookAtDirection.normalized * distance;
        }
        else if (Mathf.Acos(LookAtDirection.y / LookAtDirection.magnitude) > Mathf.Deg2Rad * 170)
        {
            LookAtDirection = new Vector3(LookAtDirection.x, 0, LookAtDirection.z);
            LookAtDirection.y = LookAtDirection.magnitude / Mathf.Tan(Mathf.Deg2Rad * 170);
            LookAtDirection = LookAtDirection.normalized * distance;
        }
        FollowingObject.GetComponent<DroneMovement>().Forward = Vector3.Cross(Vector3.Cross(Vector3.up, LookAtDirection), Vector3.up);

        if(firstPerson)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, 20f * Time.fixedDeltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(FirstPersonPosition.transform.forward, Vector3.up), 20f * Time.fixedDeltaTime);
        }
        else
        {
            targetPosition = FollowingObject.transform.position - LookAtDirection + 0.1f * Vector3.up;
            transform.position = Vector3.Lerp(transform.position, targetPosition, 20f * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(LookAtDirection, Vector3.up), 20f * Time.fixedDeltaTime);
        }
    }

    // Modifies the view
    public void SetFirstPerson(bool useFirst)
    {
        // Quick out
        if(useFirst == firstPerson || FirstPersonPosition == null)
        {
            return;
        }

        Vector3 pos = transform.position;
        if(useFirst)
        {
            transform.parent = FirstPersonPosition;
            firstPerson = useFirst;
        }
        else
        {
            transform.parent = null;
        }
        transform.position = pos;
        firstPerson = useFirst;
    }

}
