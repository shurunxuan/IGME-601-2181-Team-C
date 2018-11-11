using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{

    //enemy state enums defines all the possible states enemy can switch to.
    public enum EnemyState
    {
        TRANSITION = 0, //This is the state used when our guards are jumping from one state to another.
        PATROL = 1,
        INVESTIGATE = 2
        
    }

    //basic movement parameters
    public NavMeshAgent Agent;
    public Vector3[] PatrolPoints;
    public int Speed;
    public float WaitTime;
    private float timer = 0f;
    private int nextPoint = -1;
    public EnemyState State = EnemyState.PATROL;
    public float Alertness = 1f;

    //Enemy Abilities
    private EnemySight sight = null;
    private EnemyHearingAbility hearing = null;

    //investigation parameters
    private int sleepTime = 5;
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
        //sight.Look();
        Debug.Log(targetUpdated);
        //Enemy behaviour can be modified depending upon the current state of enemy.
        switch (State)
        {
            case EnemyState.TRANSITION:
                Transition();
                break;
            case EnemyState.PATROL:
                Agent.isStopped = false;
              
                Patrol();
                break;
            case EnemyState.INVESTIGATE:
                Investigate();
                //targetUpdated = false;
                break;
            
        }
        Debug.DrawLine(transform.position, transform.position + transform.forward * 2f, Color.green);
    }

    void FixedUpdate()
    {
        //check if player is in sight and update the beaviour
        if (sight.isPlayerVisible(out target))
        {
            Alertness += 0.05f;
            if (Vector3.Distance(LastPlayerPostion, target.position) > 2f)
            {
                targetUpdated = true;
                LastPlayerPostion = target.position;
            }
            else
            {
                //targetUpdated = false;
            }
            State = EnemyState.INVESTIGATE;

        }
        else if (hearing.Hear(transform.position,out target))
        {
            Debug.Log("I am coming for you.....");
            targetUpdated = true;
            LastPlayerPostion = target.position;
            State = EnemyState.INVESTIGATE;

        }
    }

    private void Transition()
    {

    }

    //This is the method which handles enemy's patrol between different points.
    private void Patrol()
    {
        if (timer == 0)
        {
            nextPoint = (nextPoint < PatrolPoints.Length - 1) ? nextPoint + 1 : 0;
            Agent.SetDestination(PatrolPoints[nextPoint]);

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


    //When player is visible to enemy, it will move towards it and investigate.
    private void Investigate()
    {
        Debug.Log(State);
        //Make our guard look towards the suspicious thing.
        Vector3 delta = LastPlayerPostion - transform.position;
        delta.y = 0;
        transform.forward = delta;

        //Agent.isStopped = true;
        /// TO DO: add coroutine will give player enough time to sneak out
        //Agent.isStopped = false;
        //Debug.Log(Alertness);
        if (targetUpdated)
        {
            Agent.SetDestination(LastPlayerPostion);
        }
        if (timer == 0)
        {
            timer = WaitTime;
        }

        if (Vector3.Distance(transform.position, LastPlayerPostion) <= 2f)
        {
            Alertness += 0.05f ;           
            timer--;
        }

    }
}
