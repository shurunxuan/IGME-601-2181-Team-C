using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySight {

    //Sight/looking ability related paramerters
    public float ViewRadius = 5f;
    public Transform Transform;
    public float ViewAngle = 90;
    private LayerMask playerMask;
    private LayerMask obstacleMask;
    
    public EnemySight(Transform transform,LayerMask playermask,LayerMask obstaclemask)
    {
        this.Transform = transform;
        this.playerMask = playermask;
        this.obstacleMask = obstaclemask;
    }

    public Vector3 GetDirFromAngle(float angle_in_degrees)
    {
        float angle = angle_in_degrees + Transform.eulerAngles.y;
        Vector3 dir = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
        return dir;
    }


    public void Look()
    {
        //Get direction from angle
        Vector3 dir_a = GetDirFromAngle(-ViewAngle / 2);
        Vector3 dir_b = GetDirFromAngle(ViewAngle / 2);
        Debug.DrawLine(Transform.position, Transform.position + dir_a * ViewRadius);
        Debug.DrawLine(Transform.position, Transform.position + dir_b * ViewRadius);
    }

    public bool isPlayerVisible(out Transform player)
    {
        //checks if target is in the view radius
        Collider[] playerInViewRadius = Physics.OverlapSphere(Transform.position, ViewRadius, playerMask);

        foreach (Collider c in playerInViewRadius)
        {           
            Transform target = c.transform;
            Vector3 dirToTarget = (target.position - Transform.position).normalized;
            //Debug.Log(Vector3.Angle(Transform.position, dirToTarget));
            //Check if target is visible
            if (Vector3.Angle(dirToTarget, Transform.forward) < ViewAngle/2 )
            {              
                float distanceToTarget = Vector3.Distance(Transform.position, target.position);
                Debug.DrawLine(Transform.position, target.position, Color.red);
                if (!Physics.Raycast(Transform.position, dirToTarget, distanceToTarget, obstacleMask))
                {
                    //Debug.Log("Found you!!");
                    player = target;
                    return true;
                }
            }
        }
        player = null;
        return false;
    }
}
