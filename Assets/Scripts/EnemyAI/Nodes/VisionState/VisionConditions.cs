using UnityEngine;

public class VisionConditions : BTNode
{


    public override NodeState Evaluate(SpiderBTManager agent)
    {
        if (agent.vision.playerIsVisible && !agent.visionSequenceActivated)
        {
            agent.currentBehaviorState = BehaviorState.Chase;

            agent.ResetHearState();
            agent.ResetPatrolState();

            agent.currentVisionState = VisionState.ScreamState;
            agent.visionSequenceActivated = true;
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
