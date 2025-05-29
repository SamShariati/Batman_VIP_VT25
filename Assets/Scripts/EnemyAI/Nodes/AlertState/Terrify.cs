using UnityEngine;

public class Terrify : BTNode
{
    private float totalTerrifyTimer = 3.5f;
    private float currentTimer = 0f;
    private float delayTime = 1f;
    private bool soundReady = true;

    public override NodeState Evaluate(SpiderBTManager agent)
    {

        //FIXA SÅ ATT SPINDELN ROTERAR SIG MOT LJUDET
        //if (agent.audioManager.spiderKillSound != null && soundReady)
        //{
        //    soundReady = false;
        //    //SFXManager.instance.PlaySFXClip(agent.audioManager.spiderKillSound, agent.transform, 1f);
        //    SFXManager.instance.PlayRandomSFXClip(agent.audioManager.spiderScreechSounds, agent.transform, 1f);
        //}

        SetSpiderSpeed(agent);

        currentTimer += Time.deltaTime;
        agent.navigation.isStopped = true;

        if (currentTimer < delayTime)
        {
            SetAnimation(agent, "Idle");
        }
        else
        {
            SetAnimation(agent, "Terrify");
        }

        if (currentTimer > totalTerrifyTimer)
        {
            currentTimer = 0;
            agent.terrifyStateActivated = false;
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.RUNNING;
        }




    }


    private void SetSpiderSpeed(SpiderBTManager agent)
    {
        float distance = Vector3.Distance(agent.transform.position, agent.player.position);

        if (distance < agent.hearingSensitivity && PlayerManager.Instance.isCurrentlyMakingSound)
        {
            agent.navigation.speed = agent.chaseSpeed;
        }
        else
        {
            agent.navigation.speed = agent.sprintSpeed;
        }
    }

    private void SetAnimation(SpiderBTManager agent, string type)
    {

        if (type == "Terrify")
        {
            agent.animator.SetBool("Agent_Terrify", true);
            agent.animator.SetBool("Agent_Idle", false);
        }
        else if (type == "Idle")
        {
            agent.animator.SetBool("Agent_Terrify", false);
            agent.animator.SetBool("Agent_Idle", true);
        }
        agent.animator.SetBool("Agent_Walk", false);
        agent.animator.SetBool("Agent_Run", false);
        agent.animator.SetBool("Agent_Sprint", false);
        agent.animator.SetBool("Agent_Scared", false);
        agent.animator.SetBool("Agent_Attack", false);
    }
}
