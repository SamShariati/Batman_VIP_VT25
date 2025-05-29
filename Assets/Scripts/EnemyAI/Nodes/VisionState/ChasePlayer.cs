using UnityEngine;


public class ChasePlayer : BTNode
{
    private float timer = 0;
    private float totalChaseTime = 4f;

    public override NodeState Evaluate(SpiderBTManager agent)
    {
        SetAnimation(agent);

        agent.navigation.speed = agent.chaseSpeed;
        agent.navigation.isStopped = false;
        agent.navigation.SetDestination(agent.player.position);

        timer += Time.deltaTime;

        if (timer > totalChaseTime)
        {
            timer = 0;
            agent.chaseStateActivated = false;
            agent.currentVisionState = VisionState.FleeState;

            //agent.visionSequenceActivated = false; //tillfälligt

            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.RUNNING;
        }
    }

    private void SetAnimation(SpiderBTManager agent)
    {
        agent.animator.SetBool("Agent_Run", true);
        agent.animator.SetBool("Agent_Idle", false);
        agent.animator.SetBool("Agent_Walk", false);
        agent.animator.SetBool("Agent_Terrify", false);
        agent.animator.SetBool("Agent_Sprint", false);
        agent.animator.SetBool("Agent_Scared", false);
        agent.animator.SetBool("Agent_Attack", false);
    }
}
