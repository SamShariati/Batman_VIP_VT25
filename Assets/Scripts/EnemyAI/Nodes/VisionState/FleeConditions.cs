using UnityEngine;

public class FleeConditions : BTNode
{
    
    public override NodeState Evaluate(SpiderBTManager agent)
    {
        if (CheckConditions(agent))
        {
            agent.isAttackAllowed = false;
            agent.fleeStateActivated = true;
            return NodeState.SUCCESS;

        }
        else if (agent.fleeStateActivated)
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
        if (!agent.fleeStateActivated && agent.currentVisionState == VisionState.FleeState)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
