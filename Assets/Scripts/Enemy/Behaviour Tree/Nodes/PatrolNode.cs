using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolNode : Node
{
    private NavMeshAgent agent;
    private EnemyAI enemyAI;
    private Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private bool isPlayingPatrolAnimation = false;
    private bool initialized = false;

    public PatrolNode(NavMeshAgent agent, EnemyAI enemyAI, Transform[] waypoints)
    {
        this.agent = agent;
        this.enemyAI = enemyAI;
        this.waypoints = waypoints;
    }

    //private void Update()
    //{
    //    //enemyAI.PlayWalkAnimation();
    //}

    public override NodeState Evaluate()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogWarning("PatrolNode: No waypoints available for patrol.");
            return NodeState.FAILURE;
        }

        // Initialize to closest waypoint if not yet initialized
        if (!initialized)
        {
            currentWaypointIndex = FindClosestWaypointIndex();
            agent.SetDestination(waypoints[currentWaypointIndex].position);
            //Debug.Log($"PatrolNode: Starting patrol at closest waypoint {currentWaypointIndex}");
            initialized = true;
        }

        // Move to the next waypoint if the current one is reached
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
            //Debug.Log($"PatrolNode: Moving to waypoint {currentWaypointIndex}");
        }

        // Ensure the spider is walking
        if (!isPlayingPatrolAnimation)
        {
            enemyAI.StopAllAnimations();
            enemyAI.PlayPatrolAnimation();
            isPlayingPatrolAnimation = true;
        }

        return NodeState.RUNNING;
    }

    private int FindClosestWaypointIndex()
    {
        float closestDistance = Mathf.Infinity;
        int closestIndex = 0;

        for (int i = 0; i < waypoints.Length; i++)
        {
            float distance = Vector3.Distance(agent.transform.position, waypoints[i].position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }
        return closestIndex;
    }

    public void ResetPatrol()
    {
        initialized = false;
        isPlayingPatrolAnimation = false;
    }
}
