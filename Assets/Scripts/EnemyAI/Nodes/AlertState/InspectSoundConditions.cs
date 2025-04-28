using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class InspectSoundConditions : BTNode
{
    private float distance;
    private float hearingSensitivity = 80f;
    private float hearingDistance;

    public override NodeState Evaluate(SpiderBTManager agent)
    {

        if (CheckPlayerSound(agent) && !agent.alertStateActivated)
        {
            agent.alertStateActivated = true;

            return NodeState.SUCCESS;
        }
        else if (agent.alertStateActivated)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }

    }

    private bool CheckPlayerSound(SpiderBTManager agent)
    {
        distance = Vector3.Distance(agent.transform.position, agent.player.position);
        hearingDistance = hearingSensitivity * agent.playerManager.soundIntensity;

        if (agent.playerManager.currentlyMakingSound && distance < hearingDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
