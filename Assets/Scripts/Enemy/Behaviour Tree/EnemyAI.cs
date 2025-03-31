using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IHear
{
    [SerializeField] private float chasingRange = 12;
    [SerializeField] private float attackRange = 5.0f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private EcholocationController echolocationController;

    private Animation anim;
    private Material material;
    private NavMeshAgent agent;
    private Node topNode;
    private InvestigatingNode investigatingNode;
    
    private Vector3 investigatePosition;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animation>();
        material = GetComponentInChildren<MeshRenderer>().material;
    }

    private void Start()
    {
        ConstructBehaviorTree();
        if (echolocationController == null)
        {
            echolocationController = FindObjectOfType<EcholocationController>();
            if (echolocationController == null)
            {
                Debug.LogError("EcholocationController not found in the scene!");
            }
        }
    }

    private void ConstructBehaviorTree()
    {
        // Create nodes
        IdleNode idleNode = new IdleNode(playerTransform, agent, this, chasingRange);
        ChaseNode chaseNode = new ChaseNode(playerTransform, agent, this, chasingRange);
        AttackingNode attackingNode = new AttackingNode(playerTransform, agent, this, attackRange);
        PatrolNode patrolNode = new PatrolNode(agent, this, waypoints);
        investigatingNode = new InvestigatingNode(agent, this, investigatePosition);

        // Define behavior tree sequence
        topNode = new Selector(new List<Node>
        {
            attackingNode,      // Attack if within attack range
            chaseNode,          // Chase if within chase range but not in attack range
            investigatingNode,  // Investigate if triggered by sound
            patrolNode,         // Patrol, go to waypoints
            idleNode            // Idle if none of the above are met
        });
    }

    private void Update()
    {
        topNode.Evaluate();
    }

    public void RespondToSound(Sound sound)
    {
        if (echolocationController == null || playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        float pulseDistance = echolocationController.GetPulseDistance();

        // Check if the enemy should start investigating
        if (distanceToPlayer <= pulseDistance)
        {
            //Debug.Log("EnemyAI: Investigating pulse sound...");
            investigatePosition = sound.pos;
            investigatingNode.SetSoundPosition(investigatePosition);
        }
    }
    public void PlayIdleAnimation()
    {
        if (anim != null && anim["idle"] != null)
        {
            anim.Stop();
            anim.Play("idle");
            //Debug.Log("Playing idle animation");
        }
        else
        {
            Debug.LogWarning("Idle animation not found or Animation component missing!");
        }
    }
    public void PlayRunAnimation()
    {
        if (anim != null && anim["run"] != null)
        {
            anim.Stop();
            //Debug.Log("Playing run animation");
            anim.Play("run");
        }
        else
        {
            Debug.LogWarning("Run animation not found or Animation component missing!");
        }
    }
    public void PlayWalkAnimation()
    {
        if (anim != null && anim["walk"] != null)
        {
            anim.Stop();
            anim.Play("walk");
            //Debug.Log("Playing walk animation");
        }
        else
        {
            Debug.LogWarning("Walk animation not found or Animation component missing!");
        }
    }
    public void PlayPatrolAnimation()
    {
        if (anim != null && anim["walk"] != null)
        {
            anim.Stop();
            anim.Play("walk");
            //Debug.Log("Playing walk animation");
        }
        else
        {
            Debug.LogWarning("Walk animation not found or Animation component missing!");
        }
    }
    public void PlayAttackAnimation()
    {
        if (anim != null && anim["attack1"] != null)
        {
            anim.Stop();
            //Debug.Log("Playing attack animation");
            anim.Play("attack1");
        }
        else
        {
            Debug.LogWarning("Attack animation not found or Animation component missing!");
        }
    }

    public void StopAllAnimations()
    {
        if (anim != null)
        {
            anim.Stop();
            //Debug.Log("Stopped all animations");
        }
    }

    public void InterruptInvestigation()
    {
        if (investigatingNode != null)
        {
            investigatingNode.InterruptInvestigation();
        }
    }

    public void SetColor(Color color)
    {
        material.color = color;
    }
}