using UnityEngine;


public class ChasePlayer : BTNode
{
    private float timer = 0;
    private float totalChaseTime = 5f;

    private float lastUpdatedTime = 0;
    private float soundCooldown = 0;

    public override NodeState Evaluate(SpiderBTManager agent)
    {
        SetAnimation(agent);

        agent.navigation.speed = agent.chaseSpeed;
        agent.navigation.isStopped = false;
        agent.navigation.SetDestination(agent.player.position);

        if (SoundCooldown())
        {
            SFXManager.instance.PlaySFXClip(agent.audioManager.spiderRunningSound, agent.transform, 1f);
        }

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

    private bool SoundCooldown()
    {
        if (Time.time > lastUpdatedTime + soundCooldown)
        {
            soundCooldown = 1.2f;
            lastUpdatedTime = Time.time;
            return true;
        }
        else
        {
            return false;
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
