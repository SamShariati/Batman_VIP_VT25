using UnityEngine;

public class Scream : BTNode
{
    private float totalScreamTimer = 2f;
    private float currentTimer = 0f;


    public override NodeState Evaluate(SpiderBTManager agent)
    {
        SetAnimation(agent);
        currentTimer += Time.deltaTime;
        agent.navigation.isStopped = true;

        if (currentTimer > totalScreamTimer)
        {
            agent.screamStateActivated = false;
            agent.currentVisionState = VisionState.ChaseState;
            currentTimer = 0f;
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.RUNNING;
        }
    }

    private void SetAnimation(SpiderBTManager agent)
    {

        agent.animator.SetBool("Agent_Terrify", true);
        agent.animator.SetBool("Agent_Idle", false);
        agent.animator.SetBool("Agent_Walk", false);
        agent.animator.SetBool("Agent_Run", false);
        agent.animator.SetBool("Agent_Sprint", false);
        agent.animator.SetBool("Agent_Scared", false);
        agent.animator.SetBool("Agent_Attack", false);
    }
}
