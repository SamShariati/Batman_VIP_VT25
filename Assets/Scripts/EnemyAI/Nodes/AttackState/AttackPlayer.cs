using UnityEngine;

public class AttackPlayer : BTNode
{
    private float timer = 0;
    private float totalIdleTime = 1;
    static public System.Action Attack; 

    public override NodeState Evaluate(SpiderBTManager agent)
    {
        
        agent.navigation.isStopped = true;
        SetAnimation(agent);

        timer += Time.deltaTime;

        if (timer > totalIdleTime)
        {

            //LÄGG TILL LOSING-CONDITION / SCENBYTE HÄR JACK!
            Attack?.Invoke();


            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.RUNNING;
        }
        
        

    }

    private void SetAnimation(SpiderBTManager agent)
    {
        agent.animator.SetBool("Agent_Attack", true);
        agent.animator.SetBool("Agent_Idle", false);
        agent.animator.SetBool("Agent_Walk", false);
        agent.animator.SetBool("Agent_Terrify", false);
        agent.animator.SetBool("Agent_Sprint", false);
        agent.animator.SetBool("Agent_Scared", false);
        agent.animator.SetBool("Agent_Run", false);
    }

}
