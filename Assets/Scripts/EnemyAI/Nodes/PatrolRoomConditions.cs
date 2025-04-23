using UnityEngine;

public class PatrolRoomConditions : BTNode
{
    


    public override NodeState Evaluate( SpiderBTManager agent)
    {


        if (agent.startSearchingRoom)
        {
            agent.startSearchingRoom = false;
            return NodeState.SUCCESS;
        }
        else if (agent.currentlySearchingRoom)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }











        return NodeState.SUCCESS;
    }
}
