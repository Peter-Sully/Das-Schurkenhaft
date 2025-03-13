using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public SPUM_Prefabs _prefabs;
    private PlayerState _currentState = PlayerState.IDLE;
    public Dictionary<PlayerState, int> IndexPair = new Dictionary<PlayerState, int>();
    private Vector3 originalScale;

    public float moveSpeed = 3f; 
    //public float patrolRadius = 5f; 
    //public float changeDirectionTime = 4f; 
    public float minLength = 3f;      // Minimum length of the rectangular patrol path
    public float maxLength = 8f;      // Maximum length of the rectangular patrol path
    public float minWidth = 3f;       // Minimum width of the rectangular patrol path
    public float maxWidth = 8f;       // Maximum width of the rectangular patrol path
    public float minPauseTime = 1f;   // Minimum pause time at corners
    public float maxPauseTime = 3f;   // Maximum pause time at corners

    public float stuckTimeLimit = 3f;
    private float stuckTimer = 0f; // Timer to detect if the enemy is stuck

    private Vector2[] patrolPoints = new Vector2[4]; //Four patrol points
    private int currentPatrolIndex = 0;   // The current patrol point index
    private Rigidbody2D rb;  // Rigidbody2D component for movement
    private bool isPatrolling = true;   // Whether the enemy is currently patrolling
    //private float timeToChangeDirection; 
    public float patrolPointTolerance = 0.1f;
    //public Dictionary<PlayerState, int> IndexPair = new Dictionary<PlayerState, int>();
    public float raycastDistance = 2f; // How far the ray will check for obstacles
    public LayerMask wallLayer;

    private Transform player; // Reference to the player's transform
    private bool playerInRange = false; // Flag to track if the player is in range 
    public float detectionRadius = 4f;

    void Start()
    {
        if (_prefabs == null)
        {
            _prefabs = transform.GetChild(0).GetComponent<SPUM_Prefabs>();
            if (!_prefabs.allListsHaveItemsExist())
            {
                _prefabs.PopulateAnimationLists();
            }
        }
        _prefabs.OverrideControllerInit();

        originalScale = _prefabs.transform.localScale;
        foreach (PlayerState state in Enum.GetValues(typeof(PlayerState)))
        {
            IndexPair[state] = 0;

        }

        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
        Debug.LogError("Rigidbody2D component is not assigned to the GameObject " + gameObject.name);
        }
        else
        {
           Debug.Log("Rigidbody2D successfully assigned to " + gameObject.name);
        }
        player = GameObject.FindGameObjectWithTag("Player").transform;
        wallLayer = LayerMask.GetMask("Wall");

        GenerateNewPatrolPath();
        StartCoroutine(Patrol());
    }

    void Update()
    {
        // Detect the player within the detection radius
        playerInRange = Physics2D.OverlapCircle(transform.position, detectionRadius, LayerMask.GetMask("Player"));

        if (playerInRange)
        {
            // If the player is in range, stop patrolling and move towards the player
            isPatrolling = false;
            MoveTowardsPlayer();
        }
        else
        {
            // If the player is not in range, continue patrolling
            isPatrolling = true;
        }

        if (isPatrolling && rb.linearVelocity.magnitude < 0.1f)  // The enemy is moving very slowly or not at all
        {
            stuckTimer += Time.deltaTime;

            _currentState = PlayerState.IDLE;
            PlayStateAnimation(_currentState);
            if (stuckTimer > stuckTimeLimit)
            {
                // Enemy is stuck, generate a new patrol path
                Debug.Log("Enemy is stuck, generating a new patrol path!");
                GenerateNewPatrolPath();
                stuckTimer = 0f;  // Reset the stuck timer
            }
        }
        else
        {
            stuckTimer = 0f;  // Reset stuck timer if moving
        }
    }
    // Move towards the current patrol point
    IEnumerator Patrol()
    {
        while (isPatrolling)
        {
            Vector2 targetPoint = patrolPoints[currentPatrolIndex];  // Get the target patrol point
            Vector2 direction = (targetPoint - (Vector2)transform.position).normalized;

            if (IsWallAhead(direction))
            {
                // If wall detected, stop and switch to the next direction
                Debug.Log("Wall detected! Changing direction.");
                if (!TryMoveSideways())
                {
                // If no way to move, just switch the patrol point
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                }
                yield return new WaitForSeconds(Random.Range(minPauseTime, maxPauseTime)); // Pause before continuing
                continue; // Skip the normal movement logic and move to the next patrol point
            }

            if (direction.x > 0) // Moving to the right
            {
                _prefabs.transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);  // Face right
            }
            else if (direction.x < 0) // Moving to the left
            {
                _prefabs.transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);  // Face left
            }

            // Move towards the target point
            while (Vector2.Distance(transform.position, targetPoint) > 0.1f)
            {
                _currentState = PlayerState.MOVE;

                rb.linearVelocity = direction * moveSpeed;  
                PlayStateAnimation(_currentState);
                //transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
                yield return null;
            }

            // Stop moving when the target is reached
            
            rb.linearVelocity = Vector2.zero; 
            _currentState = PlayerState.IDLE;
            PlayStateAnimation(_currentState);
            // Wait for a random pause before moving to the next point
            float pauseTime = Random.Range(minPauseTime, maxPauseTime);
            yield return new WaitForSeconds(pauseTime);

            // Update the patrol index to the next corner
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

   
    bool IsWallAhead(Vector2 direction)
    {
        // Cast a ray from the enemy's position in the direction it's facing
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastDistance, wallLayer);
        Debug.DrawRay(transform.position, direction * raycastDistance, Color.red);
        return hit.collider != null; // If the ray hits a wall, return true
    }

    /*void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            Debug.Log("Wall hit! Avoiding obstacle.");
        }
    }*/

    void OnDrawGizmos()
    {
        if (patrolPoints != null && patrolPoints.Length == 4)
        {
            Gizmos.color = Color.green;

            // Draw the rectangle patrol path
            Gizmos.DrawLine(patrolPoints[0], patrolPoints[1]);
            Gizmos.DrawLine(patrolPoints[1], patrolPoints[2]);
            Gizmos.DrawLine(patrolPoints[2], patrolPoints[3]);
            Gizmos.DrawLine(patrolPoints[3], patrolPoints[0]);
        }

        // Draw the detection radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        if (direction.x > 0) // Moving to the right
        {
            _prefabs.transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);  // Face right
        }
        else if (direction.x < 0) // Moving to the left
        {
            _prefabs.transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);  // Face left
        }

        _currentState = PlayerState.MOVE;
        PlayStateAnimation(_currentState);
    }

    void GenerateNewPatrolPath()
    {
        // Generate new random patrol path values
        float length = Random.Range(minLength, maxLength);
        float width = Random.Range(minWidth, maxWidth);

        int randomDirection = Random.Range(0, 4);

        switch (randomDirection)
        {
            case 0: // Right
                patrolPoints[0] = new Vector2(transform.position.x, transform.position.y); // Start position (corner 1)
                patrolPoints[1] = new Vector2(transform.position.x + length, transform.position.y); // Move right (corner 2)
                patrolPoints[2] = new Vector2(transform.position.x + length, transform.position.y + width); // Move up (corner 3)
                patrolPoints[3] = new Vector2(transform.position.x, transform.position.y + width); // Move left (corner 4)
                break;
            case 1: // Up
                patrolPoints[0] = new Vector2(transform.position.x, transform.position.y); // Start position (corner 1)
                patrolPoints[1] = new Vector2(transform.position.x, transform.position.y + length); // Move up (corner 2)
                patrolPoints[2] = new Vector2(transform.position.x + width, transform.position.y + length); // Move right (corner 3)
                patrolPoints[3] = new Vector2(transform.position.x + width, transform.position.y); // Move down (corner 4)
                break;
            case 2: // Left
                patrolPoints[0] = new Vector2(transform.position.x, transform.position.y); // Start position (corner 1)
                patrolPoints[1] = new Vector2(transform.position.x - length, transform.position.y); // Move left (corner 2)
                patrolPoints[2] = new Vector2(transform.position.x - length, transform.position.y + width); // Move down (corner 3)
                patrolPoints[3] = new Vector2(transform.position.x, transform.position.y + width); // Move up (corner 4)
                break;
            case 3: // Down
                patrolPoints[0] = new Vector2(transform.position.x, transform.position.y); // Start position (corner 1)
                patrolPoints[1] = new Vector2(transform.position.x, transform.position.y - length); // Move down (corner 2)
                patrolPoints[2] = new Vector2(transform.position.x - width, transform.position.y - length); // Move left (corner 3)
                patrolPoints[3] = new Vector2(transform.position.x - width, transform.position.y); // Move up (corner 4)
                break;
        }

        /*for (int i = 0; i < patrolPoints.Length; i++)
        {
            if (IsWallAhead((patrolPoints[i] - (Vector2)transform.position).normalized))
            {
                Debug.Log("Wall detected while generating patrol path! Regenerating...");
                GenerateNewPatrolPath();
                return;
            }
        }*/
        // Reset patrol index to the first corner
        currentPatrolIndex = 0;
    }

    bool TryMoveSideways()
    {
        // Try to move the enemy sideways to avoid getting stuck
        Vector2 leftDirection = Vector2.left;
        Vector2 rightDirection = Vector2.right;

        if (IsWallAhead(leftDirection))
        {
            // If there's a wall to the left, try moving right
            return !IsWallAhead(rightDirection);
        }
        else
        {
            // If there's no wall to the left, move left
            return !IsWallAhead(leftDirection);
        }
    }
    public void SetStateAnimationIndex(PlayerState state, int index = 0)
    {
        IndexPair[state] = index;
    }

    public void PlayStateAnimation(PlayerState state)
    {
        _prefabs.PlayAnimation(state, IndexPair[state]);
    }
    
}

