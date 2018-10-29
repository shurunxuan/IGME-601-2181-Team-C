using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySight {

    //Sight/looking ability related paramerters
    public float view_radius = 5f;
    public Transform transform;
    public float view_angle = 90;
    private LayerMask playermask;
    private LayerMask obstaclemask;
    
    public EnemySight(Transform transform,LayerMask playermask,LayerMask obstaclemask)
    {
        this.transform = transform;
        this.playermask = playermask;
        this.obstaclemask = obstaclemask;
    }

    public Vector3 GetDirFromAngle(float angleindegrees)
    {

        float angle_a = angleindegrees + transform.eulerAngles.y;
        Vector3 dir_a = new Vector3(Mathf.Sin(angle_a * Mathf.Deg2Rad), 0, Mathf.Cos(angle_a * Mathf.Deg2Rad));
        return dir_a;
    }


    public void Look()
    {
        //Get direction from angle

        Vector3 dir_a = GetDirFromAngle(-view_angle / 2);
        Vector3 dir_b = GetDirFromAngle(view_angle / 2);
        Debug.DrawLine(transform.position, transform.position + dir_a * view_radius);
        Debug.DrawLine(transform.position, transform.position + dir_b * view_radius);

    }

    public bool isPlayerVisible(out Transform player)
    {

        //checks if target is in the view radius
        Collider[] playerInViewRadius = Physics.OverlapSphere(transform.position, view_radius, playermask);

        foreach (Collider c in playerInViewRadius)
        {
            
            Transform target = c.transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            Debug.Log(Vector3.Angle(transform.position, dirToTarget));
            //Check if target is visible
            if (Vector3.Angle(transform.position, target.position) < view_angle/2 )
            {
              
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                Debug.DrawLine(transform.position, target.position, Color.red);
                if (!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, obstaclemask))
                {
                    Debug.Log("Found you!!");
                    player = target;

                    Debug.DrawLine(transform.position, target.position,Color.red);
                    return true;
                }
            }

        }
        player = null;
        return false;
    }
    
       
}
