using Unity.VisualScripting;
using UnityEngine;

public class InspectSound : BTNode
{
    private float alertDistance;
    private float inspectDistance;
    private float hearingDistance;
    //private Vector3 calculatedPlayerPos;

    private float totalIdleTime = 7f;
    private float currentIdleTime = 0f;

    private bool startIdleTime = false;
    private bool idleTimerFinished = false;

    public override NodeState Evaluate(SpiderBTManager agent)
    {

        SetSpiderSpeed(agent);
        CalculatePlayerPosition(agent);
        

        agent.navigation.SetDestination(agent.calculatedPlayerPos);
        agent.navigation.isStopped = false;

        inspectDistance = Vector3.Distance(agent.transform.position, agent.calculatedPlayerPos);
        if (inspectDistance < 6)
        {
            SetAnimation(agent,"Idle");
            agent.navigation.isStopped = true;
            startIdleTime = true;
        }
        else
        {
            SetAnimation(agent, "Run");
            agent.navigation.SetDestination(agent.calculatedPlayerPos);
            agent.navigation.isStopped = false;
            startIdleTime = false;
            currentIdleTime = 0f;
        }

        if (startIdleTime)
        {
            ActivateTimer();
        }

        if (idleTimerFinished)
        {
            idleTimerFinished = false;
            currentIdleTime = 0f;
            startIdleTime = false;
            agent.alertStateActivated = false;
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;

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

    private void CalculatePlayerPosition(SpiderBTManager agent)
    {
        alertDistance = Vector3.Distance(agent.transform.position, agent.player.position);
        //hearingDistance = agent.hearingSensitivity * PlayerManager.Instance.normalizedSoundLevel;
        hearingDistance = agent.hearingSensitivity * agent.playerManager.soundIntensity;

        //if (PlayerManager.Instance.isCurrentlyMakingSound && alertDistance < hearingDistance)
        //{
        //    agent.calculatedPlayerPos = agent.player.position;
        //}

        if (agent.playerManager.currentlyMakingSound && alertDistance < hearingDistance)
        {
            agent.calculatedPlayerPos = agent.player.position;
        }

    }

    private void ActivateTimer()
    {
        currentIdleTime += Time.deltaTime;

        if (currentIdleTime > totalIdleTime)
        {
            idleTimerFinished = true;
        }
    }

    private void SetAnimation(SpiderBTManager agent, string type)
    {

        if (type == "Run")
        {
            agent.animator.SetBool("Agent_Run", true);
            agent.animator.SetBool("Agent_Idle", false);
        }
        else if (type == "Idle")
        {
            agent.animator.SetBool("Agent_Run", false);
            agent.animator.SetBool("Agent_Idle", true);
        }
        agent.animator.SetBool("Agent_Walk", false);
        agent.animator.SetBool("Agent_Terrify", false);
        agent.animator.SetBool("Agent_Sprint", false);
        agent.animator.SetBool("Agent_Scared", false);
        agent.animator.SetBool("Agent_Attack", false);
    }
}
