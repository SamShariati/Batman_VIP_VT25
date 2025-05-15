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
            //agent.alertStateActivated = true;
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
        //hearingDistance = agent.hearingSensitivity * PlayerManager.Instance.normalizedSoundLevel;
        hearingDistance = agent.hearingSensitivity * agent.playerManager.soundIntensity;

        //RIKTIGA SCEN

        //if (PlayerManager.Instance.isCurrentlyMakingSound && distance < hearingDistance)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}

        //SAMS SCEN
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
