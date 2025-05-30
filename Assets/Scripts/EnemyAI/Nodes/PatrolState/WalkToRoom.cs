using UnityEngine;

public class WalkToRoom : BTNode
{
    private float distance;

    private float lastUpdatedTime = 0;
    private float soundCooldown = 0;

    public override NodeState Evaluate(SpiderBTManager agent)
    {
        SetAnimation(agent);
        agent.navigation.speed = agent.runSpeed;
        agent.navigation.isStopped = false;
        agent.navigation.SetDestination(agent.chosenRoom.position);
        agent.currentlyWalkingToRoom = true;

        distance = Vector3.Distance(agent.transform.position, agent.chosenRoom.position);

        if (SoundCooldown())
        {
            SpiderSFXManager.instance.PlaySFXClip(agent.audioManager.spiderRunningSound, agent.transform, 1f);
        }


        if (distance < 6f)
        {
            agent.navigation.isStopped = true;
            agent.currentlyWalkingToRoom = false;
            agent.startSearchingRoom = true;

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
