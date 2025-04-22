using UnityEngine;

public class PatrolRoom : BTNode
{


    public override NodeState Evaluate(SpiderBTManager agent)
    {
        throw new System.NotImplementedException();
    }



    private Transform[] chosenPoints;
    private void GetCorrectPointList(SpiderBTManager agent)
    {
        if (agent.chosenRoom.name == "Room1")
        {
            chosenPoints = agent.R1patrolPoints.ToArray();
        }

        else if (agent.chosenRoom.name == "Room2")
        {
            chosenPoints = agent.R2patrolPoints.ToArray();
        }

        else if (agent.chosenRoom.name == "Room3")
        {
            chosenPoints = agent.R3patrolPoints.ToArray();
        }

        else if (agent.chosenRoom.name == "Room4")
        {
            chosenPoints = agent.R4patrolPoints.ToArray();
        }

        else if (agent.chosenRoom.name == "Room5")
        {
            chosenPoints = agent.R5patrolPoints.ToArray();
        }

    }
}
