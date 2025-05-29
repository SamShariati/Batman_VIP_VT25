using UnityEngine;

public class PatrolRoomConditions : BTNode
{
    


    public override NodeState Evaluate( SpiderBTManager agent)
    {


        if (agent.startSearchingRoom)
        {
            agent.startSearchingRoom = false;
            agent.currentBehaviorState = BehaviorState.Patrol;
            return NodeState.SUCCESS;
        }
        else if (agent.currentlySearchingRoom)
        {
            agent.currentBehaviorState = BehaviorState.Patrol;
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }

    }
}
