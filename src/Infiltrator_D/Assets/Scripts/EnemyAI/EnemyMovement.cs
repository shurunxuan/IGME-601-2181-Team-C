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
        CHASE = 3 // We are merging chase and shoot in same state. If player is visible our guard will shoot him down. If he's running away our guard will chase him.

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

    public Animator GuardAnimator;

    private Vector2 smoothDeltaPosition = Vector2.zero;
    private Vector2 velocity = Vector2.zero;
    private bool shouldMove;
    /// <summary>
    /// Sets up all of our basic properties for our enemy.
    /// </summary>
    void Start()
    {
        sight = new EnemySight(DetectionRadius, transform, PlayerMask, ObstacleMask);
        hearing = new EnemyHearingAbility(DetectionRadius, MaxHearingDistance, Agent);
        Agent.speed = Speed;
        Agent.updatePosition = false;

        myRenderer = GetComponent<Renderer>();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Alertness:" + Alertness);
        //Enemy behaviour can be modified depending upon the current state of enemy.
        switch (State)
        {
            case EnemyState.PATROL:
                Agent.isStopped = false;
                Patrol();
                break;
            case EnemyState.INVESTIGATE:
                Agent.stoppingDistance = 0.5f;
                Investigate();
                break;
            case EnemyState.CHASE:
                Agent.stoppingDistance = 0f;
                Chase();
                break;
        }
        Debug.DrawLine(transform.position, transform.position + transform.forward * 20f, Color.green);

        Vector3 worldDeltaPosition = Agent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        //// Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;


        //if (shouldMove) Debug.Log(Agent.desiredVelocity.magnitude);
        Agent.updateRotation = shouldMove;
        GuardAnimator.SetBool("ShouldMove", shouldMove);
        //Debug.Log(Agent.desiredVelocity.magnitude);
        if (shouldMove)
        {
            GuardAnimator.SetFloat("Speed", Mathf.Lerp(GuardAnimator.GetFloat("Speed"), Agent.desiredVelocity.magnitude, 4 * Time.deltaTime));
            GuardAnimator.SetFloat("XSpeed",
                Mathf.Lerp(GuardAnimator.GetFloat("XSpeed"), Vector3.Dot(Agent.desiredVelocity, transform.right),
                    Time.deltaTime));
            GuardAnimator.SetFloat("ZSpeed",
                Mathf.Lerp(GuardAnimator.GetFloat("ZSpeed"), Vector3.Dot(Agent.desiredVelocity, transform.forward),
                    Time.deltaTime));
        }
        else
        {
            GuardAnimator.SetFloat("Speed", 0);
            GuardAnimator.SetFloat("XSpeed", Mathf.Lerp(GuardAnimator.GetFloat("XSpeed"), 0, Time.deltaTime));
            GuardAnimator.SetFloat("ZSpeed", Mathf.Lerp(GuardAnimator.GetFloat("ZSpeed"), 0, Time.deltaTime));
        }

        // Pull agent towards character
        if (worldDeltaPosition.magnitude > Agent.radius)
            Agent.nextPosition = transform.position + 0.9f * worldDeltaPosition;
    }

    void OnAnimatorMove()
    {
        // Update position based on animation movement using navigation surface height
        Vector3 position = GuardAnimator.rootPosition;
        //position.y = Agent.nextPosition.y;
        transform.position = position;
    }

    void FixedUpdate()
    {
        //check if player is in sight and update the behaviour
        //Debug.Log("Enemy State=" + State);
        if (State != EnemyState.CHASE)
        {
            if (sight.isPlayerVisible(out target) || hearing.Hear(transform.position, out target))
            {
                
                Alertness += 0.5f;
                if (Alertness >= 50 && (target != null && target.gameObject.tag == "Player"))
                {
                    UIDeathTracker.ActiveInScene.Show(UIDeathTracker.DeathTypes.Shooting);
                    shouldMove = false;
                    Agent.isStopped = true;
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
            shouldMove = true;
        }
        else
        {

            Vector3 pos = transform.position;
            pos.y = PatrolPoints[nextPoint].y;
            //Debug.Log(Vector3.Distance(pos, PatrolPoints[nextPoint]));
            if (Vector3.Distance(pos, PatrolPoints[nextPoint]) < 20 * Agent.radius)
            {
                shouldMove = false;
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
            shouldMove = false;
            sleepTimer += Time.deltaTime;

        }
        else
        {
            if (targetUpdated)
            {
                waitTimer = 0;
                Agent.SetDestination(LastPlayerPostion);
                Agent.isStopped = false;
                shouldMove = true;
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
        shouldMove = true;
        // It will keep chasing player 
        if (target != null)
        {
            Agent.SetDestination(target.position);
        }
        Debug.LogWarning(shouldMove);
        if (sight.isPlayerVisible(out target))
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
