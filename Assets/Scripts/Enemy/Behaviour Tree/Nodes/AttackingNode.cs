using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;

public class AttackingNode : Node
{
    private Transform playerTransform;
    private NavMeshAgent agent;
    private EnemyAI enemyAI;

    private float attackRange;
    private float attackInterval = 2.0f;
    private float lastAttackTime = 0.0f;

    private bool isPlayingAttackAnimation = false;

    public AttackingNode(Transform playerTransform, NavMeshAgent agent, EnemyAI enemyAI, float attackRange)
    {
        this.playerTransform = playerTransform;
        this.agent = agent;
        this.enemyAI = enemyAI;
        this.attackRange = attackRange;
    }

    public override NodeState Evaluate()
    {
        float distanceToPlayer = Vector3.Distance(agent.transform.position, playerTransform.position);

        // Attack when within attack range
        if (distanceToPlayer <= attackRange)
        {
            agent.isStopped = true;
            agent.speed = 0.0f;
            enemyAI.SetColor(Color.red);

            RotateTowardsPlayer();

            if (Time.time >= lastAttackTime + attackInterval)
            {
                lastAttackTime = Time.time;
                PlayerHealth.Instance.TakeDamage(1);
            }

            if (!isPlayingAttackAnimation)
            {
                //Debug.Log("AttackingNode: Playing attack animation");
                enemyAI.StopAllAnimations(); 
                enemyAI.PlayAttackAnimation();
                isPlayingAttackAnimation = true;
            }

            _nodeState = NodeState.RUNNING;
        }
        else
        {
            // Reset attack animation and prepare to switch to chase mode
            if (isPlayingAttackAnimation)
            {
                isPlayingAttackAnimation = false;
                enemyAI.StopAllAnimations();
                enemyAI.PlayRunAnimation();
            }

            _nodeState = NodeState.FAILURE;
        }

        return _nodeState;
    }
    private void RotateTowardsPlayer()
    {
        Vector3 direction = (playerTransform.position - agent.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
