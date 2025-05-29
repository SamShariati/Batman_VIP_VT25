using UnityEngine;

public class ChaseConditions : BTNode
{

    public override NodeState Evaluate(SpiderBTManager agent)
    {
        
        if (CheckConditions(agent))
        {
            agent.chaseStateActivated = true;

            return NodeState.SUCCESS;
        }
        else if (agent.chaseStateActivated)
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
        if (!agent.chaseStateActivated && agent.currentVisionState == VisionState.ChaseState)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
