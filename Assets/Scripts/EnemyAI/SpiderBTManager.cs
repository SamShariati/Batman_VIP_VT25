using UnityEngine;
using UnityEngine.AI;

public class SpiderBTManager : MonoBehaviour
{
    public NavMeshAgent navigation;
    public Transform test;
    void Awake()
    {
        navigation = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        navigation.destination = test.position;
    }
}
