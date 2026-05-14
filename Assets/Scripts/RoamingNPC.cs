using UnityEngine;
using UnityEngine.AI;

//roaming npc using navmesh
public class RoamingNPC : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float roamingSpeed = 3.5f;
    [SerializeField] private float stoppingDistance = 0.5f;
    [SerializeField] private float roamingRange = 50f; // Maximum distance from spawn point
    
    [Header("Behavior Settings")]
    [SerializeField] private float minWaitTime = 2f;
    [SerializeField] private float maxWaitTime = 5f;
    [SerializeField] private float destinationCheckInterval = 0.5f;
    
    [Header("Obstacle Avoidance")]
    [SerializeField] private float raycastDistance = 5f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float avoidanceForce = 2f;
    
    private NavMeshAgent navMeshAgent;
    private Vector3 spawnPoint;
    private float waitTimer;
    private bool isWaiting;
    private float destinationCheckTimer;

    private void Start()
    {
        // Get or create NavMeshAgent
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        }

        // Configure NavMeshAgent
        navMeshAgent.speed = roamingSpeed;
        navMeshAgent.stoppingDistance = stoppingDistance;

        // Store spawn point
        spawnPoint = transform.position;

        // Start roaming
        SetNewDestination();
    }

    private void Update()
    {
        if (isWaiting)
        {
            HandleWaiting();
        }
        else
        {
            HandleMovement();
        }
    }

   
    // Handles the waiting state between movements.
    private void HandleWaiting()
    {
        waitTimer -= Time.deltaTime;
        
        if (waitTimer <= 0)
        {
            isWaiting = false;
            SetNewDestination();
        }
    }

    // handles movement and npc checkpoints
    private void HandleMovement()
    {
        destinationCheckTimer -= Time.deltaTime;

        // Checks to see if the npc has reached a destination
        if (destinationCheckTimer <= 0)
        {
            destinationCheckTimer = destinationCheckInterval;

            if (!navMeshAgent.hasPath || navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || !navMeshAgent.velocity.sqrMagnitude > 0.2f * 0.2f)
                {
                    // Start waiting at destination
                    isWaiting = true;
                    waitTimer = Random.Range(minWaitTime, maxWaitTime);
                }
            }
        }

        AvoidObstacles();
    }

    // Sets a new random destination within a range
    private void SetNewDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamingRange;
        randomDirection += spawnPoint;

        //constraint to navmesh
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, roamingRange, NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(hit.position);
        }
        else
        {
            SetNewDestination();
        }
    }

    
    //Raycast obstacle avoidance
    private void AvoidObstacles()
    {
        // Forward raycast
        if (Physics.Raycast(transform.position, transform.forward, raycastDistance, obstacleLayer))
        {
            // Turn away from obstacle
            Vector3 avoidDirection = GetAvoidanceDirection();
            navMeshAgent.velocity += avoidDirection * avoidanceForce * Time.deltaTime;
        }
    }

    private Vector3 GetAvoidanceDirection()
    {
        Vector3 avoidanceDirection = Vector3.zero;
        int rayCount = 5;
        float rayAngleStep = 45f;

        for (int i = -rayCount; i <= rayCount; i++)
        {
            float angle = i * rayAngleStep;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 rayDirection = rotation * transform.forward;

            if (!Physics.Raycast(transform.position, rayDirection, raycastDistance, obstacleLayer))
            {
                avoidanceDirection += rayDirection;
            }
        }

        return avoidanceDirection.normalized;
    }

    public void ReturnToSpawn()
    {
        navMeshAgent.SetDestination(spawnPoint);
        isWaiting = false;
    }


    //checkpoints
    public void GoToPosition(Vector3 position)
    {
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, roamingRange, NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(hit.position);
            isWaiting = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        //roaming range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, roamingRange);

        //raycast distance
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * raycastDistance);
    }
}
