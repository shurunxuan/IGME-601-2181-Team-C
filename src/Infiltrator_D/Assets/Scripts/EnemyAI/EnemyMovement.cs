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
        INVESTIGATE = 2   
    }

    //basic movement parameters
    public NavMeshAgent Agent;
    public Vector3[] PatrolPoints;
    public int Speed;
    public float WaitTime = 250f;
    private float timer = 0f;
    private int nextPoint = -1;
    public EnemyState State = EnemyState.PATROL;

    //Enemy Alertness
    public float AlertTimer = 10f;
    public float Alertness = 1f;

    //Enemy Abilities
    private EnemySight sight = null;
    private EnemyHearingAbility hearing = null;

    //investigation parameters
    private float sleepTime = 5f;
    private Transform target;
   
    //Collision detection parameters
    public LayerMask PlayerMask;
    public LayerMask ObstacleMask;
    public LayerMask SoundMask;

    //Static vaiables 
    public static Vector3 LastPlayerPostion = Vector3.zero;
    private static bool targetUpdated = false;

    void Start()
    {
        sight = new EnemySight(transform, PlayerMask, ObstacleMask);
        hearing = new EnemyHearingAbility(Agent,SoundMask);
        Agent.speed = Speed;

        
    }

    // Update is called once per frame
    void Update()
    {
        //Enemy behaviour can be modified depending upon the current state of enemy.
        switch (State)
        {
            case EnemyState.PATROL:
                Agent.isStopped = false;
                Patrol();
                break;
            case EnemyState.INVESTIGATE:
                Investigate();
                break;      
        }
        Debug.DrawLine(transform.position, transform.position + transform.forward * 2f, Color.green);
    }

    void FixedUpdate()
    {
        //check if player is in sight and update the beaviour
        if (sight.isPlayerVisible(out target) || hearing.Hear(transform.position, out target))
        {
            Alertness += 0.05f;
            if (Vector3.Distance(LastPlayerPostion, target.position) > 2f)
            {
                targetUpdated = true;
                LastPlayerPostion = target.position;
                Debug.Log("Hey..What's that?");
                timer = (State == EnemyState.PATROL)?0:timer;
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
                    Debug.Log("There's Nothing here!! I am going back to position");
                    State = EnemyState.PATROL;
                    nextPoint = -1;
                    timer = 0;
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
        if (timer == 0)
        {
            nextPoint = (nextPoint + 1 < PatrolPoints.Length) ? nextPoint + 1 : 0;
            Agent.SetDestination(PatrolPoints[nextPoint]);
            Debug.Log("Next " + nextPoint);
                
            timer = WaitTime;
        }
        else
        {


            if (Vector3.Distance(transform.position, PatrolPoints[nextPoint]) < 2f)
            {
                timer--;
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

        
        if (timer <= sleepTime)
        {
            Agent.isStopped = true;
            timer += Time.deltaTime;
            //Debug.Log(timer);
        }
        else
        {
            Agent.isStopped = false;
            if (targetUpdated)
            {
                timer = 0;
                Agent.SetDestination(LastPlayerPostion);
            }
            if (timer == 0)
            {
                Debug.Log("I am going to check it out!!");
                timer = WaitTime;
            }
            else if (Vector3.Distance(transform.position, LastPlayerPostion) <= 2f)
            {
                Alertness += 0.05f;
                timer--;
            }
        }
    }
}
