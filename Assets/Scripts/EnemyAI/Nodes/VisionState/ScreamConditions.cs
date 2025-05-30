using UnityEngine;

public class ScreamConditions : BTNode
{

    private float lastUpdatedTime = 0;
    private float screamCooldown = 0;

    public override NodeState Evaluate(SpiderBTManager agent)
    {
        if (CheckConditions(agent))
        {
            SpiderSFXManager.instance.PlayRandomSFXClip(agent.audioManager.spiderScreamSounds, agent.transform, 1f);
            agent.screamStateActivated = true;
            return NodeState.SUCCESS;

        }
        else if (agent.screamStateActivated)
        {

            return NodeState.SUCCESS;

        }
        else
        {

            return NodeState.FAILURE;

        }
    }




    private bool CheckConditions(SpiderBTManager agent)
    {
        if (!agent.screamStateActivated && agent.currentVisionState == VisionState.ScreamState)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool CheckCooldown()
    {
        if (Time.time > lastUpdatedTime + screamCooldown)
        {
            screamCooldown = 15f;
            lastUpdatedTime = Time.time;
            return true;
        }
        else
        {
            return false;
        }
    }
}
