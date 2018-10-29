using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{

    //enemy state enums defines all the possible states enemy can switch to.
    public enum EnemyState
    {
        PATROL = 1,
        INVESTIGATE = 2
    }

    //basic movement parameters
    public NavMeshAgent Agent;
    public GameObject[] PatrolPoints;
    public int Speed;
    public float WaitTime;
    private float timer = 0f;
    private int nextPoint = -1;
    public EnemyState State = EnemyState.PATROL;


    //investigation parameters
    private EnemySight sight = null;
    private Transform target;
    private bool targetUpdated = false;
    public LayerMask PlayerMask;
    public LayerMask ObstacleMask;

    void Start()
    {
        sight = new EnemySight(transform, PlayerMask, ObstacleMask);
        Agent.speed = Speed;
    }

    // Update is called once per frame
    void Update()
    {
        sight.Look();

        //Enemy behaviour can be modified depending upon the current state of enemy.
        switch (State)
        {
            case EnemyState.PATROL:
                Patrol();
                break;
            case EnemyState.INVESTIGATE:
                if (targetUpdated)
                {
                    Investigate();
                    targetUpdated = false;
                }
                break;
        }
        Debug.DrawLine(transform.position, transform.position + transform.forward * 2f, Color.green);
    }

    void FixedUpdate()
    {
        //check if player is in sight and update the beaviour
        if (sight.isPlayerVisible(out target))
        {
            Debug.Log("Updating state to Investigate");
            State = EnemyState.INVESTIGATE;
            targetUpdated = true;
        }
    }

    //This is the method which handles enem't patrol between different points.
    private void Patrol()
    {
        if (timer == 0)
        {
            nextPoint = (nextPoint < PatrolPoints.Length - 1) ? nextPoint + 1 : 0;
            Agent.SetDestination(PatrolPoints[nextPoint].transform.position);

            timer = WaitTime;
        }
        else
        {

            if (Vector3.Distance(transform.position, PatrolPoints[nextPoint].transform.position) < 2f)
            {
                timer--;
            }
        }
    }


    //When player is visible to enemy, it will move towards it and investigate.
    private void Investigate()
    {
        Agent.SetDestination(target.position);
    }
}
