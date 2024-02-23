using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public Transform[] waypoints; // Array of waypoints for patrol route
    public float patrolSpeed = 2f;
    public float seekSpeed = 4f;
    public float seekRange = 10f;
    public float fleeSpeed = 6f;
    public float fleeDistance = 5f;
    public float obstacleAvoidanceRange = 2f;

    private int currentWaypointIndex = 0;
    private Transform player;
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Assuming player tag is "Player"
    }

    void Update()
    {
        if (IsPlayerInRange(fleeDistance))
        {
            FleeFromPlayer();
        }
        else if (IsPlayerInRange(navMeshAgent.stoppingDistance))
        {
            SeekPlayerLN();
        }
        else if (IsPlayerInRange(seekRange)) // Check if player is in seek range
        {
            SeekPlayerLN();
        }
        else if (currentWaypointIndex == 2)
        {
            SeekPlayerLN();
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

    //void SeekPlayer()//necessary?
    //{
        //navMeshAgent.speed = seekSpeed;
        //navMeshAgent.SetDestination(player.position);
    //}
    void SeekPlayerLN()
    {
        navMeshAgent.speed = seekSpeed;

        // Calculate direction to the player
        Vector3 directionToPlayer = player.position - transform.position;

        RaycastHit hit;
        // Check if there is a direct line of sight between the NPC and the player
        if (Physics.Linecast(transform.position, player.position, out hit))
        {
            Debug.DrawRay(transform.position, directionToPlayer.normalized * hit.distance, Color.red);
            if (hit.collider.CompareTag("Player"))
            {
                // If the player is in line of sight, set destination to player's position
                navMeshAgent.SetDestination(player.position);

                navMeshAgent.speed = seekSpeed;//necessary?
            }
            else
            {
                // If there's an obstacle in the way, find a position near the obstacle to navigate around it
                Vector3 obstacleAvoidancePosition = hit.point + hit.normal * obstacleAvoidanceRange;
                navMeshAgent.SetDestination(obstacleAvoidancePosition);
            }
        }
        else
        {
            // If there is no obstacle in the way, directly navigate to the player
            //navMeshAgent.SetDestination(player.position);
            //SeekPlayer();
            Patrol();

        }
    }

    void FleeFromPlayer()
    {
        Vector3 fleeDirection = transform.position - player.position;
        Vector3 fleePosition = transform.position + fleeDirection.normalized * fleeDistance;

        navMeshAgent.speed = fleeSpeed;
        navMeshAgent.SetDestination(fleePosition);
    }

    bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.position) <= range;
    }
    //bool IsPlayerInRange(float range)
    //{
        //return Vector3.Distance(transform.position, player.position) <= range;
    //}
}
