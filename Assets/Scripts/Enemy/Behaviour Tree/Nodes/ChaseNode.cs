using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseNode : Node
{
    private Transform playerTransform;
    private NavMeshAgent agent;
    private EnemyAI enemyAI;
    private PatrolNode patrolNode;
    
    private float chaseSpeed = 2.0f;
    private float chasingRange;
    private float attackRange = 5.0f;
    
    private bool isPlayingRunAnimation = false;


    public ChaseNode(Transform playerTransform, NavMeshAgent agent, EnemyAI enemyAI, float chasingRange)
    {
        this.playerTransform = playerTransform;
        this.agent = agent;
        this.enemyAI = enemyAI;
        this.chasingRange = chasingRange;
    }

    public override NodeState Evaluate()
    {
        float distanceToPlayer = Vector3.Distance(agent.transform.position, playerTransform.position);

        if (distanceToPlayer <= chasingRange && distanceToPlayer > attackRange)
        {
            enemyAI.InterruptInvestigation();

            agent.isStopped = false;
            agent.speed = chaseSpeed;
            agent.SetDestination(playerTransform.position);
            enemyAI.SetColor(Color.green);

            if (!isPlayingRunAnimation)
            {
                //Debug.Log("ChaseNode: Playing run animation");
                enemyAI.StopAllAnimations();
                enemyAI.PlayRunAnimation();
                isPlayingRunAnimation = true;
            }

            if (distanceToPlayer > chasingRange)
            {
                if (isPlayingRunAnimation)
                {
                    isPlayingRunAnimation = false;
                    enemyAI.StopAllAnimations();
                }
                patrolNode.ResetPatrol(); // Reset patrol when exiting chase
                _nodeState = NodeState.FAILURE;
            }

            _nodeState = NodeState.RUNNING;
        }
        else
        {
            if (isPlayingRunAnimation)
            {
                //Debug.Log("ChaseNode: Exiting chase state, stopping run animation");
                isPlayingRunAnimation = false;
                enemyAI.StopAllAnimations();
                enemyAI.PlayPatrolAnimation();
            }

            _nodeState = NodeState.FAILURE;
        }

        return _nodeState;
    }
}
