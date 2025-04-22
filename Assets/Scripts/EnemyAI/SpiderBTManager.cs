using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class SpiderBTManager : MonoBehaviour
{
    private BTNode rootNode;
    private Transform roomPointsPrefab;
    private Transform patrolPointsPrefab;

    public NavMeshAgent navigation;
    public Transform test;
    public Animator animator;

    
    public List<Transform> roomPoints;
    public Transform[] R1patrolPoints;
    public Transform[] R2patrolPoints;
    public Transform[] R3patrolPoints;
    public Transform[] R4patrolPoints;
    public Transform[] R5patrolPoints;

    //PatrolState

    public bool walkToNewRoomAllowed = false;
    public bool walkingToNewRoom = false;
    void Awake()
    {


        navigation = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("Agent_Idle", true);
    }

    // Update is called once per frame
    void Update()
    {

        rootNode.Evaluate(this);
    }

    private void GetPatrolPositions()
    {
        foreach(Transform t in roomPointsPrefab)
        {
            roomPoints.Add(t);
        }
    }
    private void ConstructBT()
    {


        //rootNode = new Selector(new List<Node>() { chase, idle });
    }
}
