using UnityEngine;
using UnityEngine.InputSystem.Android;

public class TerrifyConditions : BTNode
{

    private float lastUpdatedTime = 0;
    private float terrifyCooldown = 0;

    public override NodeState Evaluate(SpiderBTManager agent)
    {
        if (CheckCooldown() && !agent.terrifyStateActivated)
        {
            agent.terrifyReady = false;
            agent.terrifyStateActivated = true;
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

    private bool CheckCooldown()
    {
        if (Time.time > lastUpdatedTime + terrifyCooldown)
        {
            terrifyCooldown = 30f;
            lastUpdatedTime = Time.time;
            return true;
        }
        else
        {
            return false;
        }
    }
}
