﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHearingAbility{

    public float HearingRadius = 7f;
    public float MaxHearDistance = 7f;

    //private LayerMask soundMask;
    private NavMeshAgent agent;

    /// <summary>
    /// Constrcutor to set up hearing component.
    /// </summary>
    /// <param name="HearingRadius">Defines the radius for sphear in which our enemy can hear</param>
    /// <param name="MaxHearDistance">Max distance our agent can hear from</param>
    /// <param name="agent">Navmesh component of enemy</param>
    /// <param name="soundMask">Layer mask for collision</param>
    public EnemyHearingAbility(float HearingRadius, float MaxHearDistance, NavMeshAgent agent)
    {
        this.HearingRadius = HearingRadius;
        this.MaxHearDistance = MaxHearDistance;
        this.agent = agent;
    }

    /// <summary>
    /// Checks if there is any object on sound layer inside hearing radius.
    /// </summary>
    /// <param name="agentPosition">Agent's Current position</param>
    /// <param name="target">This is an out parameter which holds target's posiotion.</param>
    /// <returns>Returns True if we are able to hear something else false</returns>
    public bool Hear(Vector3 agentPosition, out Transform target)
    {
        //Get objects on the sound layer.
        GameObject[] soundsources = GameObject.FindGameObjectsWithTag("SoundSource");
       
        if (soundsources.Length > 0)
        {
            int i = 0;

            for (i = 0; i < soundsources.Length; i++)
            {
                if (soundsources[i].activeInHierarchy && Getdistance(agentPosition, soundsources[i].transform.position) <= MaxHearDistance)
                {
                    break;
                }
                
            }
            if (i < soundsources.Length)
            {
                target = soundsources[i].transform;
                Debug.Log("Heard " + target.name);
            }
            else
            {
                target = null;
            }

            return target != null;
        }

        target = null;
        return false;
       
    }

    /// <summary>
    /// Get distance between our enemy agent and sound target. After collision occurs.
    /// </summary>
    /// <param name="agentPosition">Current position of the agent</param>
    /// <param name="targetPosition">Position of the target</param>
    /// <returns>The distance between agnet and source of sound.</returns>
    private float Getdistance (Vector3 agentPosition, Vector3 targetPosition)
    {
        //calculate path between the target and our enemy agent using NavMesh APIs.
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(targetPosition, path);

        //Create an array of all the waypoints between target and agent and use it to calculate total distance.

        Vector3[] wayPoints = new Vector3[path.corners.Length + 2]; //+2 is to include the positions of the agent and target
        wayPoints[0] = agentPosition;
        wayPoints[wayPoints.Length - 1] = targetPosition;

        for(int i = 0; i < path.corners.Length; i++)
        {
            wayPoints[i + 1] = path.corners[i];
        }

        float totalDistance = 0f;

        for(int i =0;i<wayPoints.Length - 1; i++)
        {
            totalDistance += Vector3.Distance(wayPoints[i], wayPoints[i + 1]);
        }

        return totalDistance;
    }
}
