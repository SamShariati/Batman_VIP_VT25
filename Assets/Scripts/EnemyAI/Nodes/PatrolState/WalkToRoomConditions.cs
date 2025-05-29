using UnityEngine;

public class WalkToRoomConditions : BTNode
{
    private Transform previousRoom;
    private bool loopActivated;
    private bool runOnce = false;
    private int nrTimes = 0;

    public override NodeState Evaluate(SpiderBTManager agent)
    {

        if (!runOnce)
        {
            //Debug.Log("Entered WalkToRoomConditions");
            runOnce = true;
        }
        //Debug.Log("Entered WalkToRoomConditions");

        if (agent.walkToNewRoomAllowed)
        {
            agent.currentBehaviorState = BehaviorState.Patrol;
            agent.walkToNewRoomAllowed = false;
            loopActivated = true;
            ChooseRoom(agent);

            return NodeState.SUCCESS;
        }

        else if (agent.currentlyWalkingToRoom)
        {
            agent.currentBehaviorState = BehaviorState.Patrol;
            return NodeState.SUCCESS;
        }

        else
        {
            return NodeState.FAILURE;
        }

    }

    private void ChooseRoom(SpiderBTManager agent)
    {

        while (loopActivated)
        {
            int range = agent.roomPoints.Count;
            int randNr = Random.Range(0, range);

            agent.chosenRoom = agent.roomPoints[randNr];
            if (agent.chosenRoom != previousRoom)
            {
                previousRoom = agent.chosenRoom;
                loopActivated=false;
                nrTimes++;
            }


        }
    }

    
}
