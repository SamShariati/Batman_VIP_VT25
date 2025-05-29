using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;


public enum VisionState
{

    ScreamState,
    ChaseState,
    FleeState,
    Inactive

}

public enum BehaviorState
{

    Attack,
    Chase,
    Hear,
    Patrol

}

public class SpiderBTManager : MonoBehaviour
{
    private BTNode rootNode;
    public Transform roomPointsPrefab;
    public Transform patrolPointsPrefab;
    public Transform player;
    [HideInInspector] public SimpleMovement playerManager;
    

    //Stats
    public float walkSpeed;
    public float chaseSpeed;
    public float runSpeed;
    public float sprintSpeed;

    [HideInInspector] public NavMeshAgent navigation;
    public Transform test;
    [HideInInspector] public Animator animator;


    [HideInInspector] public List<Transform> roomPoints;
    [HideInInspector] public List<Transform> R1patrolPoints;
    [HideInInspector] public List<Transform> R2patrolPoints;
    [HideInInspector] public List<Transform> R3patrolPoints;
    [HideInInspector] public List<Transform> R4patrolPoints;
    [HideInInspector] public List<Transform> R5patrolPoints;

    [HideInInspector] public Transform[] testarray;

    //PatrolState

    [HideInInspector] public bool walkToNewRoomAllowed = true;
    [HideInInspector] public bool currentlyWalkingToRoom = false;
    public Transform chosenRoom;
    [HideInInspector] public bool startSearchingRoom = true;
    [HideInInspector] public bool currentlySearchingRoom = false;
    [HideInInspector] public bool getNewPointList = true;

    //AlertState

    [HideInInspector] public bool terrifyReady = true;
    [HideInInspector] public bool terrifyStateActivated = false;
    [HideInInspector] public bool alertStateActivated = false;
    [HideInInspector] public float hearingSensitivity = 200f;
    [HideInInspector] public Vector3 calculatedPlayerPos;

    //VisionState

    [HideInInspector] public SpiderVision vision;
    [HideInInspector] public bool visionSequenceActivated = false;
    [HideInInspector] public bool screamStateActivated = false;
    [HideInInspector] public bool screamStateAllowed = true;
    [HideInInspector] public bool chaseStateActivated = false;
    [HideInInspector] public bool fleeStateActivated = false;
    [HideInInspector] public VisionState currentVisionState;
    public Transform fleeRoomChosen;

    [HideInInspector] public bool isAttackAllowed = true;
    [HideInInspector] public bool attackStateActivated = false;


    [HideInInspector] public BehaviorState currentBehaviorState;



    void Awake()
    {

        navigation = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        vision = GetComponent<SpiderVision>();
        animator.SetBool("Agent_Idle", true);
        walkToNewRoomAllowed = true;
        GetPatrolPositions();
       
        chosenRoom = roomPoints[0];
        ConstructBT();
        
        playerManager = player.GetComponent<SimpleMovement>();
        
    }


    void Update()
    {
        
        rootNode.Evaluate(this);
        
    }

    public void ResetPatrolState()
    {
        walkToNewRoomAllowed = true;
        currentlyWalkingToRoom = false;
        startSearchingRoom = true;
        currentlySearchingRoom = false;
        getNewPointList = true;
    }

    public void ResetHearState()
    {
        alertStateActivated = false;
        terrifyStateActivated = false;
        //
    }

    private void GetPatrolPositions()
    {
        foreach(Transform t in roomPointsPrefab)
        {
            roomPoints.Add(t);
        }

        foreach(Transform room in patrolPointsPrefab)
        {
            
            if (room.name == "Room1")
            {
                foreach (Transform p in room)
                {
                    R1patrolPoints.Add(p);
                }
            }
            else if (room.name == "Room2")
            {
                foreach (Transform p in room)
                {
                    R2patrolPoints.Add(p);
                }
            }
            else if (room.name == "Room3")
            {
                foreach (Transform p in room)
                {
                    R3patrolPoints.Add(p);
                }
            }
            else if (room.name == "Room4")
            {
                foreach (Transform p in room)
                {
                    R4patrolPoints.Add(p);
                }
            }
            else if (room.name == "Room5")
            {
                foreach (Transform p in room)
                {
                    R5patrolPoints.Add(p);
                }
            }
        }
    }
    private void ConstructBT()
    {
        WalkToRoomConditions walkToRoomConditions = new WalkToRoomConditions();
        WalkToRoom walkToRoom = new WalkToRoom();
        PatrolRoomConditions patrolRoomConditions = new PatrolRoomConditions();
        PatrolRoom patrolRoom = new PatrolRoom();
        HearingConditions hearingConditions = new HearingConditions();
        TerrifyConditions terrifyConditions = new TerrifyConditions();
        Terrify terrify = new Terrify();
        InspectSoundConditions inspectSoundConditions = new InspectSoundConditions();
        InspectSound inspectSound = new InspectSound();
        VisionConditions visionConditions = new VisionConditions();
        ScreamConditions screamConditions = new ScreamConditions();
        Scream scream = new Scream();
        ChaseConditions chaseConditions = new ChaseConditions();
        ChasePlayer chasePlayer = new ChasePlayer();
        FleeConditions fleeConditions = new FleeConditions();
        Flee flee = new Flee();
        AttackConditions attackConditions = new AttackConditions();
        AttackPlayer attackPlayer = new AttackPlayer();

        //Patrol Branch
        Sequence walkToRoomState = new Sequence(new List<BTNode>() { walkToRoomConditions, walkToRoom });
        Sequence patrolRoomState = new Sequence(new List<BTNode>() { patrolRoomConditions, patrolRoom });
        Selector patrolState = new Selector(new List<BTNode>() { walkToRoomState, patrolRoomState });

        //Hear Branch
        Sequence terrifyState = new Sequence(new List<BTNode>() { terrifyConditions, terrify });
        Sequence inspectSoundState = new Sequence(new List<BTNode>() { inspectSound });
        Selector alertState = new Selector(new List<BTNode>() { terrifyState, inspectSoundState });
        Sequence hearState = new Sequence(new List<BTNode>() { hearingConditions, alertState });

        //Vision Branch

        Sequence screamState = new Sequence(new List<BTNode>() { screamConditions, scream });
        Sequence chaseState = new Sequence(new List<BTNode>() { chaseConditions, chasePlayer });
        Sequence fleeState = new Sequence(new List<BTNode>() { fleeConditions, flee });
        Selector visionBehavior = new Selector(new List<BTNode>() { screamState, chaseState, fleeState });
        Sequence visionState = new Sequence(new List<BTNode>() { visionConditions, visionBehavior });

        //Attack Branch

        Sequence attackState = new Sequence(new List<BTNode>() { attackConditions, attackPlayer });


        rootNode = new Selector(new List<BTNode>() { attackState, visionState, hearState, patrolState });
    }
}
