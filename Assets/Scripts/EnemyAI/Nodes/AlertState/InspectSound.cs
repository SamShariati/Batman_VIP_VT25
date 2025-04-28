using Unity.VisualScripting;
using UnityEngine;

public class InspectSound : BTNode
{
    private float alertDistance;
    private float inspectDistance;
    private float hearingSensitivity = 80f;
    private float hearingDistance;
    private Vector3 calculatedPlayerPos;

    private float totalIdleTime = 7f;
    private float currentIdleTime = 0f;

    private bool startIdleTime = false;
    private bool idleTimerFinished = false;

    public override NodeState Evaluate(SpiderBTManager agent)
    {

        CalculatePlayerPosition(agent);

        agent.navigation.SetDestination(calculatedPlayerPos);
        agent.navigation.isStopped = false;

        inspectDistance = Vector3.Distance(agent.transform.position, calculatedPlayerPos);
        if (inspectDistance < 6)
        {
            agent.navigation.isStopped = true;
            startIdleTime = true;
        }
        else
        {
            agent.navigation.SetDestination(calculatedPlayerPos);
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

    private void CalculatePlayerPosition(SpiderBTManager agent)
    {
        alertDistance = Vector3.Distance(agent.transform.position, agent.player.position);
        hearingDistance = hearingSensitivity * agent.playerManager.soundIntensity;

        if (agent.playerManager.currentlyMakingSound && alertDistance < hearingDistance)
        {
            calculatedPlayerPos = agent.transform.position;
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
}
