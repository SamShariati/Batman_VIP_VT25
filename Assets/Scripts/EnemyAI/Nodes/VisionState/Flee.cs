using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Flee : BTNode
{
    private bool runOnce = false;
    private float distance;
    private bool startTimer = false;
    private float timer = 0;
    private float totalIdleTime = 5;
    public override NodeState Evaluate(SpiderBTManager agent)
    {
        
        if (!runOnce)
        {
            runOnce = true;
            agent.fleeRoomChosen = SelectFleeRoom(agent);
        }

        agent.navigation.speed = agent.sprintSpeed;
        agent.navigation.isStopped = false;
        agent.navigation.SetDestination(agent.fleeRoomChosen.position);

        distance = Vector3.Distance(agent.transform.position, agent.fleeRoomChosen.position);

        if (distance < 6)
        {
            agent.navigation.isStopped = true;
            startTimer = true;
            SetAnimation(agent, "Idle");
        }
        else
        {
            SetAnimation(agent, "Sprint");
        }

        if (startTimer)
        {
            timer += Time.deltaTime;
        }

        if (timer > totalIdleTime)
        {
            agent.visionSequenceActivated = false;
            agent.currentVisionState = VisionState.Inactive;
            startTimer = false;
            runOnce = false;
            timer = 0;  

            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.RUNNING;
        }


    }

    private void SetAnimation(SpiderBTManager agent, string type)
    {

        if (type == "Sprint")
        {
            agent.animator.SetBool("Agent_Sprint", true);
            agent.animator.SetBool("Agent_Idle", false);
        }
        else if (type == "Idle")
        {
            agent.animator.SetBool("Agent_Sprint", false);
            agent.animator.SetBool("Agent_Idle", true);
        }
        agent.animator.SetBool("Agent_Run", false);
        agent.animator.SetBool("Agent_Terrify", false);
        agent.animator.SetBool("Agent_Walk", false);
        agent.animator.SetBool("Agent_Scared", false);
        agent.animator.SetBool("Agent_Attack", false);
    }


    public Transform SelectFleeRoom(SpiderBTManager agent)
    {
        // Calculate direction FROM spider TO player (this is what we want to flee away from)
        Vector3 spiderToPlayer = (agent.player.position - agent.transform.position).normalized;

        // Flee in the opposite direction (away from player)
        Vector3 fleeDirection = -spiderToPlayer;

        Vector3 spiderPosition = agent.transform.position;
        Vector3 playerPosition = agent.player.position;

        Transform bestRoom = null;
        float bestScore = float.MinValue;
        float minFleeDistance = 5f;

        foreach (Transform roomPoint in agent.roomPoints)
        {
            // Skip rooms that contain the spider or player
            if (IsPositionInRoom(spiderPosition, roomPoint) || IsPositionInRoom(playerPosition, roomPoint))
                continue;

            // Calculate direction from spider's current position to this room
            Vector3 roomDirection = (roomPoint.position - spiderPosition).normalized;

            // Calculate how aligned this room is with our flee direction (away from player)
            float directionalAlignment = Vector3.Dot(fleeDirection, roomDirection);

            // Calculate distance to room
            float distanceToRoom = Vector3.Distance(spiderPosition, roomPoint.position);

            // Skip rooms that are too close
            if (distanceToRoom < minFleeDistance)
                continue;

            // Score this room (prioritize direction alignment, with distance as secondary factor)
            float score = directionalAlignment * 2f + (distanceToRoom / 10f);

            if (score > bestScore)
            {
                bestScore = score;
                bestRoom = roomPoint;
            }
        }

        // Fallback logic remains the same
        if (bestRoom == null)
        {
            float maxDistance = 0f;
            foreach (Transform roomPoint in agent.roomPoints)
            {
                if (IsPositionInRoom(spiderPosition, roomPoint) || IsPositionInRoom(playerPosition, roomPoint))
                    continue;

                float distance = Vector3.Distance(spiderPosition, roomPoint.position);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    bestRoom = roomPoint;
                }
            }
        }

        return bestRoom;
    }

    // Helper method to check if a position is within a room's bounds
    private bool IsPositionInRoom(Vector3 position, Transform roomPoint)
    {
        float roomRadius = 100f; // Adjust based on your room sizes
        return Vector3.Distance(position, roomPoint.position) < roomRadius;
    }
}
