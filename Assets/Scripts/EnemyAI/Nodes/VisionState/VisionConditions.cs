using UnityEngine;

public class VisionConditions : BTNode
{


    public override NodeState Evaluate(SpiderBTManager agent)
    {
        if (agent.vision.playerIsVisible && !agent.visionSequenceActivated)
        {
            agent.ResetHearState();
            agent.ResetPatrolState();

            agent.currentVisionState = VisionState.ScreamState;
            agent.visionSequenceActivated = true; //blir false efter runaway är färdig
            return NodeState.SUCCESS;
        }
        else if (agent.visionSequenceActivated)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
