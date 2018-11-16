﻿using System;
using System.Collections;
using System.Threading;
using UnityEditor.Animations;
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
    public Vector3[] PatrolPoints;
    public int Speed;
    public float WaitTime = 2f;
    private float waitTimer = 0f;
    private int nextPoint = -1;
    public EnemyState State = EnemyState.PATROL;

    //Enemy Alertness
    public float AlertTimer = 10f;
    public float Alertness = 1f;
    private Renderer myRenderer;

    //Enemy Abilities
    public float DetectionRadius = 7f;
    public float MaxHearingDistance = 10f;
    private EnemySight sight = null;
    private EnemyHearingAbility hearing = null;

    //investigation parameters
    private readonly float sleepTime = 5f;
    private float sleepTimer = 0f;
    private Transform target;

    //Collision detection parameters
    public LayerMask PlayerMask;
    public LayerMask ObstacleMask;
    public LayerMask SoundMask;

    //Static vaiables 
    public static Vector3 LastPlayerPostion = Vector3.zero;
    private static bool targetUpdated = false;

    public Animator GuardAnimator;

    /// <summary>
    /// Sets up all of our basic properties for our enemy.
    /// </summary>
    void Start()
    {
        sight = new EnemySight(DetectionRadius, transform, PlayerMask, ObstacleMask);
        hearing = new EnemyHearingAbility(DetectionRadius, MaxHearingDistance, Agent, SoundMask);
        Agent.speed = Speed;
        myRenderer = GetComponent<Renderer>();

    }

    // Update is called once per frame
    void Update()
    {
        //Enemy behaviour can be modified depending upon the current state of enemy.
        switch (State)
        {
            case EnemyState.PATROL:
                //myRenderer.material.color = Color.green;
                Agent.isStopped = false;
                Patrol();
                break;
            case EnemyState.INVESTIGATE:
                //myRenderer.material.color = Color.yellow;
                Investigate();
                break;
        }
        Debug.DrawLine(transform.position, transform.position + transform.forward * 20f, Color.green);
        //Debug.Log(Agent.velocity.magnitude);
    }

    void FixedUpdate()
    {
        GuardAnimator.SetFloat("Speed", Agent.velocity.magnitude);
        //check if player is in sight and update the behaviour
        if (sight.isPlayerVisible(out target) || hearing.Hear(transform.position, out target))
        {
            Alertness += 0.05f;
            if (Vector3.Distance(LastPlayerPostion, target.position) > 2f)
            {
                targetUpdated = true;
                LastPlayerPostion = target.position;
                Debug.Log(gameObject.name + ": " + "Target " + target.name + " Found");
                sleepTimer = (State == EnemyState.PATROL) ? 0 : sleepTimer;
                AlertTimer = 10f;
            }

            if (State != EnemyState.INVESTIGATE)
            {
                State = EnemyState.INVESTIGATE;
            }
        }
        else // Defines the enemy decisions when player is not visible or in audible range.
        {
            if (State != EnemyState.PATROL)
            {
                if (AlertTimer <= 0)
                {
                    Alertness = 0;
                    Debug.Log(gameObject.name + ": " + "Returning to patrol from state " + State);
                    State = EnemyState.PATROL;
                    nextPoint = -1;
                    waitTimer = 0;
                }
                else
                {
                    AlertTimer -= Time.deltaTime;

                }
            }
        }

    }

    private void Transition()
    {

    }

    /// <summary>
    /// This is the method which handles enemy's patrol between different points.
    /// </summary>
    private void Patrol()
    {
        if (waitTimer <= 0)
        {
            nextPoint = (nextPoint + 1 < PatrolPoints.Length) ? nextPoint + 1 : 0;
            Vector3 currentDirection = transform.forward;
            Vector3 nextDirection = PatrolPoints[nextPoint] - transform.position;
            float angle = Vector3.Angle(currentDirection, nextDirection);
            if (angle > 45)
            {
                GuardAnimator.SetTrigger("TurnRight");
            }
            else if (angle < -45)
            {
                GuardAnimator.SetTrigger("TurnLeft");
            }
            Agent.SetDestination(PatrolPoints[nextPoint]);
            Debug.Log(gameObject.name + ": " + "Heading to waypoint " + (nextPoint + 1));

            waitTimer = WaitTime;
        }
        else
        {

            Vector3 pos = transform.position;
            pos.y = PatrolPoints[nextPoint].y;
            if (Vector3.Distance(pos, PatrolPoints[nextPoint]) < 2f)
            {

                waitTimer -= Time.deltaTime;

            }
        }
    }

    /// <summary>
    /// This method defines the enemy behavior in Investigate state. 
    /// </summary>
    private void Investigate()
    {
        //Debug.Log(State);
        //Make our guard look towards the suspicious thing.
        Vector3 delta = LastPlayerPostion - transform.position;
        delta.y = 0;
        transform.forward = delta;


        if (sleepTimer <= sleepTime)
        {
            Agent.isStopped = true;
            sleepTimer += Time.deltaTime;

        }
        else
        {
            if (targetUpdated)
            {
                waitTimer = 0;
                Agent.SetDestination(LastPlayerPostion);
                Agent.isStopped = false;
                targetUpdated = false;
                Debug.Log(gameObject.name + ": " + "Investigate");
            }
            if (Math.Abs(waitTimer) < 0.001f)
            {
                waitTimer = WaitTime;
            }
            else if (Vector3.Distance(transform.position, LastPlayerPostion) <= 2f)
            {
                Alertness += 0.05f;
                waitTimer -= Time.deltaTime;
            }
        }
    }
}
