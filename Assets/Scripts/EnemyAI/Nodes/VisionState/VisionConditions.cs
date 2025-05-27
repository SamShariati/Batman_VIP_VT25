using UnityEngine;

public class VisionConditions : BTNode
{


    public override NodeState Evaluate(SpiderBTManager agent)
    {
        if (agent.vision.playerIsVisible && !agent.playerSpotted)
        {
            agent.playerSpotted = true;
            return NodeState.SUCCESS;
        }
        else if (agent.playerSpotted)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
