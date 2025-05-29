using Unity.VisualScripting;
using UnityEngine;

public class HearingConditions : BTNode
{
    private float distance;
    private float hearingDistance;

    public override NodeState Evaluate(SpiderBTManager agent)
    {

        if (CheckPlayerSound(agent) && !agent.alertStateActivated)
        {
            agent.currentBehaviorState = BehaviorState.Hear;
            agent.alertStateActivated = true;
            agent.ResetPatrolState();

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

        //RIKTIGA SCEN

        //hearingDistance = agent.hearingSensitivity * PlayerManager.Instance.normalizedSoundLevel;

        //if (PlayerManager.Instance.isCurrentlyMakingSound && distance < hearingDistance)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}

        //SAMS SCEN

        hearingDistance = agent.hearingSensitivity * agent.playerManager.soundIntensity;

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
