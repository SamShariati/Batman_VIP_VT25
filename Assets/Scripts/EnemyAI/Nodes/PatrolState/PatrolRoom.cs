using UnityEngine;

public class PatrolRoom : BTNode
{
    public bool runOnce = false;
    private Transform[] patrolPoints;
    //private bool chooseNewPatrolPoint = true;
    private Transform chosenPatrolPoint;
    private Transform previousPatrolPoint;
    private bool loopActivated = true;
    private bool activateNewPatrolPoint = true;
    private float distance;

    private float totalIdleTime = 7f;
    private float currentIdleTime = 0f;

    private bool startIdleTime = true;
    private int nrTimesSwappedPoint = 0;




    public override NodeState Evaluate(SpiderBTManager agent)
    {
        agent.currentlySearchingRoom = true;
        agent.navigation.speed = agent.walkSpeed;

        //agent.test = chosenPatrolPoint;
        if (agent.getNewPointList)
        {
            GetCorrectPointList(agent);
            agent.getNewPointList = false;
        }

        if (activateNewPatrolPoint)
        {
            loopActivated = true;
            activateNewPatrolPoint = false;
            GetNewPatrolPoint(agent);
        }

        if (startIdleTime)
        {
            ActivateTimer();
        }

        distance = Vector3.Distance(agent.transform.position, chosenPatrolPoint.position);

        if(distance > 6f)
        {
            SetAnimation(agent, "Walk");
            agent.navigation.SetDestination(chosenPatrolPoint.position);
            agent.navigation.isStopped = false;

        }
        else
        {
            SetAnimation(agent, "Idle");
            startIdleTime = true;
            agent.navigation.isStopped = true;
        }

        if (nrTimesSwappedPoint == 3)
        {
            nrTimesSwappedPoint = 0;
            agent.getNewPointList = true;
            agent.walkToNewRoomAllowed = true;
            agent.currentlySearchingRoom = false;
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.RUNNING;
        }

    }




    private void GetNewPatrolPoint(SpiderBTManager agent)
    {

        while (loopActivated)
        {
            int range = patrolPoints.Length;
            int randNr = Random.Range(0, range);
            chosenPatrolPoint = patrolPoints[randNr];

            if (chosenPatrolPoint != previousPatrolPoint)
            {
                previousPatrolPoint = chosenPatrolPoint;
                loopActivated = false;
            }

        }
    }

    private void ActivateTimer()
    {
        currentIdleTime += Time.deltaTime;

        if (currentIdleTime > totalIdleTime)
        {
            startIdleTime = false;
            activateNewPatrolPoint = true;
            currentIdleTime = 0;
            nrTimesSwappedPoint++;
        }
    }


    private void GetCorrectPointList(SpiderBTManager agent)
    {
        if (agent.chosenRoom.name == "Room1")
        {
            patrolPoints = agent.R1patrolPoints.ToArray();
        }

        else if (agent.chosenRoom.name == "Room2")
        {
            patrolPoints = agent.R2patrolPoints.ToArray();
        }

        else if (agent.chosenRoom.name == "Room3")
        {
            patrolPoints = agent.R3patrolPoints.ToArray();
        }

        else if (agent.chosenRoom.name == "Room4")
        {
            patrolPoints = agent.R4patrolPoints.ToArray();
        }

        else if (agent.chosenRoom.name == "Room5")
        {
            patrolPoints = agent.R5patrolPoints.ToArray();
        }

    }

    private void SetAnimation(SpiderBTManager agent, string type)
    {

        if (type == "Walk")
        {
            agent.animator.SetBool("Agent_Walk", true);
            agent.animator.SetBool("Agent_Idle", false);
        }
        else if (type == "Idle")
        {
            agent.animator.SetBool("Agent_Walk", false);
            agent.animator.SetBool("Agent_Idle", true);
        }
        agent.animator.SetBool("Agent_Run", false);
        agent.animator.SetBool("Agent_Terrify", false);
        agent.animator.SetBool("Agent_Sprint", false);
        agent.animator.SetBool("Agent_Scared", false);
        agent.animator.SetBool("Agent_Attack", false);
    }
}
