using UnityEngine;

public class WalkToRoom : BTNode
{
    private float distance;   
    public override NodeState Evaluate(SpiderBTManager agent)
    {
        SetAnimation(agent);
        agent.navigation.speed = agent.runSpeed;
        agent.navigation.isStopped = false;
        agent.navigation.SetDestination(agent.chosenRoom.position);
        agent.currentlyWalkingToRoom = true;

        distance = Vector3.Distance(agent.transform.position, agent.chosenRoom.position);

        if (distance < 6f)
        {
            agent.navigation.isStopped = true;
            agent.currentlyWalkingToRoom = false;
            agent.walkToNewRoomAllowed = true;

            return NodeState.SUCCESS;

        }
        else
        {
            return NodeState.RUNNING;
        }
    }

    private void SetAnimation(SpiderBTManager agent)
    {
        agent.animator.SetBool("Agent_Run", true);
        agent.animator.SetBool("Agent_Idle", false);
        agent.animator.SetBool("Agent_Walk", false);
        agent.animator.SetBool("Agent_Terrify", false);
        agent.animator.SetBool("Agent_Sprint", false);
        agent.animator.SetBool("Agent_Scared", false);
        agent.animator.SetBool("Agent_Attack", false);
    }

   
}
