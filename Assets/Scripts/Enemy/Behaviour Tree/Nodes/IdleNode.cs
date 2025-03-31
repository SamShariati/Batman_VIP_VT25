using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;

public class IdleNode : Node
{
    private Transform playerTransform;
    private NavMeshAgent agent;
    private EnemyAI enemyAI;
    private float chaseRange;
    private bool isPlayingIdleAnimation = false;

    public IdleNode(Transform playerTransform, NavMeshAgent agent, EnemyAI enemyAI, float chaseRange)
    {
        this.playerTransform = playerTransform;
        this.agent = agent;
        this.enemyAI = enemyAI;
        this.chaseRange = chaseRange;
    }

    public override NodeState Evaluate()
    {
        float distanceToPlayer = Vector3.Distance(agent.transform.position, playerTransform.position);

        if (distanceToPlayer > chaseRange)
        {
            agent.isStopped = true;
            enemyAI.SetColor(Color.yellow);

            if (!isPlayingIdleAnimation)
            {
                //Debug.Log("IdleNode: Playing idle animation");
                enemyAI.StopAllAnimations();
                enemyAI.PlayIdleAnimation();
                isPlayingIdleAnimation = true;
            }

            enemyAI.PlayIdleAnimation();

            _nodeState = NodeState.SUCCESS;
        }
        else
        {
            if (isPlayingIdleAnimation)
            {
                //Debug.Log("Exiting Idle State, resetting animation flag");
                isPlayingIdleAnimation = false;
                enemyAI.StopAllAnimations();
            }

            _nodeState = NodeState.FAILURE;
        }

        return _nodeState;
    }
}
