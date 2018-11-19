using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{

    //enemy state enums defines all the possible states enemy can switch to.
    public enum EnemyState
    {
        PATROL = 1,
        INVESTIGATE = 2,
        CHASE=3 // We are merging chase and shoot in same state. If player is visible our guard will shoot him down. If he's running away our guard will chase him.

    }

    //basic movement parameters
    [Header("Movement Parameters")]
    public NavMeshAgent Agent;
    public Vector3[] PatrolPoints;
    public int Speed;
    public float WaitTime = 2f;
    private float waitTimer = 0f;
    private int nextPoint = -1;
    public EnemyState State = EnemyState.PATROL;

    //Enemy Alertness
    [Header("Alertness")]
    public float AlertTimer = 10f;
    public float Alertness = 1f;
    private Renderer myRenderer;

    //Enemy Abilities
    [Header("Abilities")]
    public float DetectionRadius = 7f;
    public float MaxHearingDistance = 10f;
    private EnemySight sight = null;
    private EnemyHearingAbility hearing = null;

    //investigation parameters
    [Header("Investigation Parameters")]
    private readonly float sleepTime = 5f;
    private float sleepTimer = 0f;
    private Transform target;

    //shooting parameters
    [Header("Shooting System")]
    public GameObject Gun;
    public ParticleSystem ImpactEffect;
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
        myRenderer = GetComponent<Renderer>();

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Alertness:" + Alertness);
        //Enemy behaviour can be modified depending upon the current state of enemy.
        switch (State)
        {
            case EnemyState.PATROL:
                myRenderer.material.color = Color.green;
                Agent.isStopped = false;
                Patrol();
                break;
            case EnemyState.INVESTIGATE:
                Agent.stoppingDistance = 0.5f;
                myRenderer.material.color = Color.yellow;
                Investigate();
                break;
            case EnemyState.CHASE:
                Agent.stoppingDistance = 2f;
                myRenderer.material.color = Color.grey;
                Chase();
                break;
        }
        Debug.DrawLine(transform.position, transform.position + transform.forward * 20f, Color.green);
        //Debug.Log(Agent.velocity.magnitude);
    }

    void FixedUpdate()
    {
        //check if player is in sight and update the behaviour
        Debug.Log("Enemy State="+   State);
        if (State != EnemyState.CHASE)
        {
            if (sight.isPlayerVisible(out target) || hearing.Hear(transform.position, out target))
            {
                Alertness += 0.5f;
                if (Alertness >= 50)
                {
                    State = EnemyState.CHASE;
                }
                else
                {
                    if (Vector3.Distance(LastPlayerPostion, target.position) > 2f)
                    {
                        targetUpdated = true;
                        LastPlayerPostion = target.position;
                        sleepTimer = (State == EnemyState.PATROL) ? 0 : sleepTimer;
                        AlertTimer = 10f;
                    }

                    if (State != EnemyState.INVESTIGATE)
                    {
                        State = EnemyState.INVESTIGATE;
                    }
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
                        AlertTimer -= Time.fixedDeltaTime;

                    }
                }
            }
        }

    }

    /// <summary>
    /// This is the method which handles enemy's patrol between different points.
    /// </summary>
    private void Patrol()
    {
        if (waitTimer <= 0)
        {
            nextPoint = (nextPoint + 1 < PatrolPoints.Length) ? nextPoint + 1 : 0;
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
       
        LookAt();

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
                Alertness += 0.5f;
                waitTimer -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Make our enemy to look in the direction of player/sound source.
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
        // It will keep chasing player 
        Agent.SetDestination(target.transform.position);

        if(sight.isPlayerVisible(out target))
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
