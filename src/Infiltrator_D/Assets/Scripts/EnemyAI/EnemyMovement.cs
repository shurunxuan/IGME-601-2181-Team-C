﻿using System;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{

    //enemy state enums defines all the possible states enemy can switch to.
    public enum EnemyState
    {
        PATROL = 0,
        STAND = 1,
        WALKTOTHESOURCE=3,
        INVESTIGATE = 4,
        CHASE = 5 // This is the most alert state.
    }

    private List<Action> stateHandlers = new List<Action>();


    //basic movement parameters
    [Header("Movement Parameters")]
    public NavMeshAgent Agent;
    public Vector3[] PatrolPoints;
    public int Speed;
    public float PatrolWaitTimer = 2f;
    private float Timer = 0f;
    private int nextPoint = -1;
    public EnemyState State = EnemyState.PATROL;

    //Enemy Alertness
    [Header("Alertness")]
    public float Alertness = 0f;
    public float AlertnessTimer = 0f;

    //Enemy Abilities
    [Header("Abilities")]
    public float DetectionRadius = 7f;
    public float MaxHearingDistance = 10f;
    private EnemySight sight = null;
    private EnemyHearingAbility hearing = null;

    //investigation parameters
    [Header("Investigation Parameters")]
    private Transform target;

    //shooting parameters
    [Header("Shooting System")]
    //public GameObject Gun;
    //public ParticleSystem ImpactEffect;
    public float ImpactForce = 10f;
    private Time nextShootTime = null;

    //Collision detection parameters
    [Header("Collision Detection Parameters")]
    public LayerMask PlayerMask;
    public LayerMask ObstacleMask;
    public LayerMask SoundMask;

    //Static vaiables 
    public static Vector3 LastPlayerPostion = Vector3.zero;
    private static bool targetUpdated = false;

   
    /// <summary>
    /// Sets up all of our basic properties for our enemy.
    /// </summary>
    void Start()
    {
        
        sight = new EnemySight(DetectionRadius, transform, PlayerMask, ObstacleMask);
        hearing = new EnemyHearingAbility(DetectionRadius, MaxHearingDistance, Agent);
        Agent.speed = Speed;
        //Uncomment following method when we need to manage animations in our script.
        //Agent.updatePosition = false;
     
        //Updating state handlers list
        stateHandlers.Add(Patrol);
        stateHandlers.Add(Stand);
        stateHandlers.Add(Investigate);
        stateHandlers.Add(WalkToSource);
        stateHandlers.Add(Chase);        
    }

    // Update is called once per frame
    void Update()
    {
        //Enemy behaviour can be modified depending upon the current state of enemy.
        stateHandlers[(int)State].DynamicInvoke();
        
    }

    
    /// <summary>
    /// Detects player using Enemies sight and hearing ability.
    /// </summary>
    /// <returns>Tru if player detected and False if otherwise.</returns>
    public bool IsPlayerDetected()
    {
        if (sight.See(out target) || hearing.Hear(transform.position, out target))
        {
          
            if (Vector3.Distance(LastPlayerPostion, target.position) > 2f )
            {
                targetUpdated = true;
                LastPlayerPostion = target.position;
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Method that handles alertness of player. We have a timer as we don't want the timer to increase to max quickly
    /// so after each increment timer will be set to whater value we have passed.
    /// </summary>
    /// <param name="incr">It is the factor which decides if the alertness increses or decreases. 1 for increment and - for decrement.</param>
    /// <param timer="timer">It is the value to which countdown timer should be set to.</param>
    private void UpdateAlertness(float incr, float timer)
    {
        if (AlertnessTimer <= 0)
        {
            Alertness += 5 * incr;
            AlertnessTimer = timer;
        }
        else
        {
            AlertnessTimer -= Time.deltaTime;
        }
        
    }

    /// <summary>
    /// This is the method which handles enemy's patrol between different points.
    /// </summary>
    private void Patrol()
    {
        //Detection Cycle
        if (IsPlayerDetected())
        {
            State = EnemyState.STAND;
            Timer = 3;
            Agent.isStopped = true;
        }
        else if (Alertness > 0)
        {
            UpdateAlertness(-1, 1f);
        }

        //Patrol Movement block
        if (Timer <= 0)
        {
            nextPoint = (nextPoint + 1 < PatrolPoints.Length) ? nextPoint + 1 : 0;
            Agent.SetDestination(PatrolPoints[nextPoint]);
            Timer = PatrolWaitTimer;
        }
        else
        {
            Vector3 pos = transform.position;
            pos.y = PatrolPoints[nextPoint].y;
            if (Vector3.Distance(pos, PatrolPoints[nextPoint]) <= 2f)
            {
                Timer -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Method which handles Stand behavior.
    /// </summary>
    private void Stand()
    {
        if (Timer <= 0)
            State = EnemyState.WALKTOTHESOURCE;
        else
            Timer -= Time.deltaTime;
    }

 

    /// <summary>
    /// Method will make the enemy walk to the source of sound/visible detection of suspicious thing.
    /// </summary>
    private void WalkToSource()
    {
        if (targetUpdated)
        {
            Timer = 0;
            Agent.SetDestination(LastPlayerPostion);
            Agent.isStopped = false;
            //shouldMove = true;
            targetUpdated = false;
        }
        else if (Vector3.Distance(transform.position, LastPlayerPostion) <= 5f || Alertness>=100)
        {
            State = EnemyState.INVESTIGATE;
        }

        //Check to see for target position and alertness.
        //We want to increase the Alertness only if the AI has been to investigate state and came back here so we are checking if alertness in > 0
        if (IsPlayerDetected() && Alertness > 0)
        {    
            UpdateAlertness(1,1f); 
        }
        else if (Alertness > 0)
        {
            UpdateAlertness(-1, 1f);
        }

    }

    /// <summary>
    /// This method defines the enemy behavior in Investigate state. 
    /// </summary>
    private void Investigate()
    {
        Debug.Log("Investigatin");
        if (Alertness >= 100)
            State = EnemyState.CHASE;
        if (IsPlayerDetected())
        {
            UpdateAlertness(1, 1f); 
            if (targetUpdated)
                State = EnemyState.WALKTOTHESOURCE;
        }
        else
        {
            Debug.Log("Decresing Alertness");
            Timer = 0;
            UpdateAlertness(-1,1f);
            if (Alertness <= 0)
            {
                Alertness = 0;
                State = EnemyState.PATROL;
            }
        }


    }

    /// <summary>
    /// Make our enemy to look in the direction of player/sound source.Also Can be used to point gun at the player.
    /// </summary>
    private void LookAt()
    {
        Vector3 delta = LastPlayerPostion - transform.position;
        delta.y = 0;
        transform.forward = delta;
    }

    /// <summary>
    /// This method will handle chase and shooting behavior of the guard.
    /// </summary>
    private void Chase()
    {
        LookAt();
        //shouldMove = true;
        // It will keep chasing player 
        if (target != null)
        {
            Agent.SetDestination(target.position);
        }
        //Debug.LogWarning(shouldMove);
        if (sight.See(out target))
        {
            Shoot();
        }

    }

    private void Shoot()
    {
        //Creating a stub method for now.
        Debug.Log("Dead drone!!!!!");

    }
}
