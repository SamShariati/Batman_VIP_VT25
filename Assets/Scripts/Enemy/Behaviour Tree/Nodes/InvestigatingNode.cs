using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InvestigatingNode : Node
{
    private NavMeshAgent agent;
    private EnemyAI enemyAI;
    private PatrolNode patrolNode;

    private Vector3 soundPosition;

    private bool reachedPosition = false;
    private bool isInvestigating = false;
    private bool isPlayingWalkAnimation = false;

    public InvestigatingNode(NavMeshAgent agent, EnemyAI enemyAI, Vector3 soundPosition)
    {
        this.agent = agent;
        this.enemyAI = enemyAI;
        this.soundPosition = soundPosition;
    }

    public override NodeState Evaluate()
    {
        if (!isInvestigating)
        {
            if (isPlayingWalkAnimation)
            {
                //enemyAI.StopAllAnimations();
                isPlayingWalkAnimation = false;
            }
            return NodeState.FAILURE;
        }

        if (isInvestigating && reachedPosition)
        {
            isInvestigating = false;
            patrolNode.ResetPatrol();
            return NodeState.SUCCESS;
        }

        if (!reachedPosition)
        {
            agent.SetDestination(soundPosition);
            agent.isStopped = false;
            enemyAI.SetColor(Color.blue);

            if (!isPlayingWalkAnimation)
            {
                enemyAI.StopAllAnimations();
                enemyAI.PlayWalkAnimation();
                isPlayingWalkAnimation = true;
            }

            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                reachedPosition = true;
                isInvestigating = false;
                enemyAI.PlayPatrolAnimation();
            }

            _nodeState = NodeState.RUNNING;
            return _nodeState;
        }
        else
        {
            if (isPlayingWalkAnimation)
            {
                //enemyAI.StopAllAnimations();
                isPlayingWalkAnimation = false;
                enemyAI.PlayPatrolAnimation();
            }
            _nodeState = NodeState.SUCCESS;
            return _nodeState;
        }
    }

    public void SetSoundPosition(Vector3 position)
    {
        soundPosition = position;
        reachedPosition = false;
        isInvestigating = true;
        isPlayingWalkAnimation = false;
    }
    public void InterruptInvestigation()
    {
        isInvestigating = false;
        reachedPosition = true;
    }
}
