using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {

    //enemy state enums defines all the possible states enemy can switch to.
    public enum  enemystate{
        PATROL =1,
        INVESTIGATE=2
    }

    //basic movement parameters
    public Camera cam;
    public NavMeshAgent agent;
    public GameObject enemy_object;
    public GameObject [] patrol_points = new GameObject[6];
    public int startpoint = -1;
    public int speed;
    public float waititme = 3f;
    private float timer = 0f;
    public enemystate enemy_state = enemystate.PATROL;


    //investigation parameters
    private EnemySight sight = null;
    private Transform target;
    private bool targetUpdated = false;
    public LayerMask playermask;
    public LayerMask obstaclemask;

    void Start()
    {
        sight = new EnemySight(transform,playermask,obstaclemask);
        agent.speed = speed;
       
    }

    // Update is called once per frame
    void Update ()
    {
        
        sight.Look();

        //Enemy behaviour can be modified depending upon the current state of enemy.
        switch (enemy_state)
        {
            case enemystate.PATROL:
                Patrol();
                break;
            case enemystate.INVESTIGATE:
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
        
        if(sight.isPlayerVisible(out target))
        {
            Debug.Log("Updating state to Investigate");
            enemy_state = enemystate.INVESTIGATE;
            targetUpdated = true;
        }
    }

    private void Patrol()
    {
        if (timer == 0)
        {
            startpoint = (startpoint < patrol_points.Length - 1) ? startpoint + 1 : 0;
            agent.SetDestination(patrol_points[startpoint].transform.position);
     
            timer = waititme;
        }
        else
        {
            
            if (Vector3.Distance(transform.position, patrol_points[startpoint].transform.position) < 2f)
            {
                timer--;
            }
        }

    }

    private void Investigate()
    {
        agent.SetDestination(target.position);
    }
    



}
