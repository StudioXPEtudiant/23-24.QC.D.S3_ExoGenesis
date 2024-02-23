using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomMovement : MonoBehaviour
{
    public Vector3 center;
    public float range = 10f;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        SetRandomDestination();
    }

    void SetRandomDestination()
    {
        Vector3 randomPos = center + new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, range, NavMesh.AllAreas);
        agent.SetDestination(hit.position);
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            SetRandomDestination();
        }
    }
}

