using Unity.VisualScripting;
using UnityEngine;

public class AttackConditions : BTNode
{
    private float distance;

    
    public override NodeState Evaluate(SpiderBTManager agent)
    {
        distance = Vector3.Distance(agent.transform.position, agent.player.position);

        if (CheckConditions(agent))
        {
            agent.attackStateActivated = true;
            SpiderSFXManager.instance.PlaySFXClip(agent.audioManager.spiderKillSound, agent.transform, 1f);
            return NodeState.SUCCESS;
        }
        else if (agent.attackStateActivated)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }

    private bool CheckConditions(SpiderBTManager agent)
    {
        if (distance < 6 && agent.isAttackAllowed && !agent.attackStateActivated)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
