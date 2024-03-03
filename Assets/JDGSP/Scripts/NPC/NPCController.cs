using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public Transform[] waypoints;
    public float patrolSpeed = 2f;
    public float seekSpeed = 4f;
    public float seekRange = 30f;
    public float fleeSpeed = 6f;
    public float fleeDistance = 5f;
    public float obstacleAvoidanceRange = 2f;

    private Transform player;
    private NavMeshAgent navMeshAgent;
    private int currentWaypointIndex = 0;
    private Vector3 playerPosition;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        playerPosition = player.position;

        if (IsPlayerInRange(fleeDistance))
        {
            FleeFromPlayer();
        }
        else if (IsPlayerInRange(seekRange) || currentWaypointIndex == 2)
        {
            SeekPlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        navMeshAgent.speed = patrolSpeed;
        if (waypoints.Length == 0) return;

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void SeekPlayer()
    {
        navMeshAgent.speed = seekSpeed;

        NavMeshHit hit;
        if (NavMesh.Raycast(transform.position, playerPosition, out hit, NavMesh.AllAreas))
        {
            // Obstacle detected
            Vector3 obstacleAvoidancePosition = hit.position + hit.normal * obstacleAvoidanceRange;
            navMeshAgent.SetDestination(obstacleAvoidancePosition);
        }
        else if (IsPlayerInRange(seekRange))
        {
            // The player is within the specified range
            // Add code here to execute
            navMeshAgent.SetDestination(playerPosition);
            Debug.Log("Player is within range!");
        }
        else
        {
            // No obstacle, directly navigate to player
            Patrol();
        }
    }


    void FleeFromPlayer()
    {
        Vector3 fleeDirection = transform.position - playerPosition;
        Vector3 fleePosition = transform.position + fleeDirection.normalized * fleeDistance;

        navMeshAgent.speed = fleeSpeed;
        navMeshAgent.SetDestination(fleePosition);
    }

    bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, playerPosition) <= range;
    }
}


