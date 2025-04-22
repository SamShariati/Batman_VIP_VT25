using UnityEngine;

public class WalkToRoomConditions : BTNode
{
    public override NodeState Evaluate(SpiderBTManager agent)
    {

        if (agent.walkToNewRoomAllowed)
        {
            agent.walkToNewRoomAllowed = false;
            return NodeState.SUCCESS;
        }

        else if (agent.walkingToNewRoom)
        {
            return NodeState.SUCCESS;
        }

        else
        {
            return NodeState.FAILURE;
        }


    }

    
}
