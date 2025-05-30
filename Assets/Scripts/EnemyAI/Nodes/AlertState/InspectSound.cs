using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private float lastUpdatedTime = 0;
    private float soundCooldown = 0;

    public override NodeState Evaluate(SpiderBTManager agent)
    {
        agent.currentlyInspectingSound = true;
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

            if (SoundCooldown())
            {
                SFXManager.instance.PlaySFXClip(agent.audioManager.spiderRunningSound, agent.transform, 1f);
            }

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
            SFXManager.instance.PlaySFXClip(agent.audioManager.spiderScreechSounds[1], agent.transform, 1f);
            idleTimerFinished = false;
            currentIdleTime = 0f;
            startIdleTime = false;
            agent.currentlyInspectingSound = false;
            agent.alertStateActivated = false;
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;

    }


    private bool SoundCooldown()
    {
        if (Time.time > lastUpdatedTime + soundCooldown)
        {
            soundCooldown = 1.2f;
            lastUpdatedTime = Time.time;
            return true;
        }
        else
        {
            return false;
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

    private void CalculatePlayerPosition(SpiderBTManager agent)
    {
        alertDistance = Vector3.Distance(agent.transform.position, agent.player.position);

        if (SceneManager.GetActiveScene().name == "Spider AI Test Scene")
        {
            hearingDistance = agent.hearingSensitivity * agent.playerManager.soundIntensity;

            if (agent.playerManager.currentlyMakingSound && alertDistance < hearingDistance)
            {
                agent.calculatedPlayerPos = agent.player.position;
            }
        }
        else
        {
            hearingDistance = agent.hearingSensitivity * PlayerManager.Instance.normalizedSoundLevel;

            if (PlayerManager.Instance.isCurrentlyMakingSound && alertDistance < hearingDistance)
            {
                agent.calculatedPlayerPos = agent.player.position;
            }
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
