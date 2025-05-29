using UnityEngine;
using UnityEngine.InputSystem.Android;

public class TerrifyConditions : BTNode
{

    private float lastUpdatedTime = 0;
    private float terrifyCooldown = 0;

    public override NodeState Evaluate(SpiderBTManager agent)
    {
        if (CheckConditions(agent))
        {
            
            agent.terrifyStateActivated = true;
            agent.calculatedPlayerPos = agent.player.position;
            return NodeState.SUCCESS;
        }
        else if (agent.terrifyStateActivated)
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
        if (!agent.terrifyStateActivated && !agent.currentlyInspectingSound && CheckCooldown())
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
        if (Time.time > lastUpdatedTime + terrifyCooldown)
        {
            terrifyCooldown = 10f;
            lastUpdatedTime = Time.time;
            return true;
        }
        else
        {
            return false;
        }
    }
}
