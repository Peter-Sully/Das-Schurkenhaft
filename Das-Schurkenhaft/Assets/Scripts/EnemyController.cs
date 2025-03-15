using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    public SPUM_Prefabs _prefabs;
    private PlayerState _currentState = PlayerState.IDLE;
    public Dictionary<PlayerState, int> IndexPair = new Dictionary<PlayerState, int>();
    private Vector3 originalScale;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float wanderRadius = 5f;              // How far from spawn to wander
    public float minPauseTime = 1f;              // Min pause between wander moves
    public float maxPauseTime = 3f;              // Max pause between wander moves

    [Header("Stuck Settings")]
    public float stuckTimeLimit = 3f;            // Time before teleporting back
    private float stuckTimer = 0f;

    [Header("Detection Settings")]
    public float detectionRadius = 4f;           // When the enemy will start chasing the player
    public float raycastDistance = 2f;           // Distance for wall detection
    public LayerMask wallLayer;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 spawnPoint;

    private Coroutine wanderCoroutine;
    private bool isChasing = false;

    void Awake()
    {
        // Initialize _prefabs
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

        // Initialize animation indices
        foreach (PlayerState state in System.Enum.GetValues(typeof(PlayerState)))
        {
            IndexPair[state] = 0;
        }

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is not assigned to " + gameObject.name);
        }
        else
        {
            Debug.Log("Rigidbody2D successfully assigned to " + gameObject.name);
        }
        player = GameObject.FindGameObjectWithTag("Player").transform;
        wallLayer = LayerMask.GetMask("Wall");
    }

    void Start()
    {
        spawnPoint = transform.position; // Save the original spawn position
        StartWandering();
    }

    // When re-enabled after being disabled, restart necessary routines and reset timers.
    void OnEnable()
    {
        stuckTimer = 0f;
        if (rb != null) rb.linearVelocity = Vector2.zero;
        if (!isChasing)
        {
            StartWandering();
        }
    }

    // Clean up coroutine when disabled.
    void OnDisable()
    {
        if (wanderCoroutine != null)
        {
            StopCoroutine(wanderCoroutine);
            wanderCoroutine = null;
        }
    }

    void Update()
    {
        // Check if the player is within the detection radius.
        bool playerDetected = Physics2D.OverlapCircle(transform.position, detectionRadius, LayerMask.GetMask("Player"));
        if (playerDetected)
        {
            if (!isChasing)
            {
                isChasing = true;
                // Stop wandering when chasing the player.
                if (wanderCoroutine != null)
                {
                    StopCoroutine(wanderCoroutine);
                    wanderCoroutine = null;
                }
            }
            MoveTowardsPlayer();
        }
        else
        {
            if (isChasing)
            {
                // Player has been lost; resume wandering.
                isChasing = false;
                StartWandering();
            }
        }

        // Check if the enemy is stuck (only when not chasing)
        if (!isChasing)
        {
            if (rb.linearVelocity.magnitude < 0.1f)
            {
                stuckTimer += Time.deltaTime;
                _currentState = PlayerState.IDLE;
                PlayStateAnimation(_currentState);
                if (stuckTimer > stuckTimeLimit)
                {
                    // Teleport enemy back to spawn if stuck too long
                    TeleportToSpawn();
                    stuckTimer = 0f;
                }
            }
            else
            {
                stuckTimer = 0f;
            }
        }
    }

    // Coroutine for random wandering near the original spawn point.
    IEnumerator Wander()
    {
        while (true)
        {
            Vector2 currentPosition = transform.position;
            Vector2 wanderTarget;

            // If the enemy is outside the wander circle, force it back toward spawn.
            if (Vector2.Distance(currentPosition, spawnPoint) > wanderRadius)
            {
                Vector2 directionToSpawn = (spawnPoint - currentPosition).normalized;
                wanderTarget = spawnPoint + directionToSpawn * (wanderRadius * 0.8f);
            }
            else
            {
                wanderTarget = spawnPoint + Random.insideUnitCircle * wanderRadius;
            }

            Vector2 direction = (wanderTarget - (Vector2)transform.position).normalized;

            // Set facing direction.
            if (direction.x > 0)
            {
                _prefabs.transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
            else if (direction.x < 0)
            {
                _prefabs.transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }

            _currentState = PlayerState.MOVE;
            PlayStateAnimation(_currentState);

            // Move toward the wander target.
            while (Vector2.Distance(transform.position, wanderTarget) > 0.1f && !isChasing)
            {
                if (IsWallAhead(direction))
                {
                    Debug.Log("Wall detected during wander. Choosing new target.");
                    break;
                }

                rb.linearVelocity = direction * moveSpeed;
                yield return null;
            }

            // Stop movement and set idle state when target is reached.
            rb.linearVelocity = Vector2.zero;
            _currentState = PlayerState.IDLE;
            PlayStateAnimation(_currentState);

            // Pause before selecting a new target.
            float pauseTime = Random.Range(minPauseTime, maxPauseTime);
            yield return new WaitForSeconds(pauseTime);
        }
    }

    // Moves the enemy toward the player.
    void MoveTowardsPlayer()
    {
        if (player == null) return;
        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        if (direction.x > 0)
        {
            _prefabs.transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (direction.x < 0)
        {
            _prefabs.transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }

        _currentState = PlayerState.MOVE;
        PlayStateAnimation(_currentState);
    }

    // Checks for obstacles in front of the enemy.
    bool IsWallAhead(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastDistance, wallLayer);
        Debug.DrawRay(transform.position, direction * raycastDistance, Color.red);
        return hit.collider != null;
    }

    // Teleports the enemy back to its original spawn position.
    void TeleportToSpawn()
    {
        Debug.Log("Enemy is stuck; teleporting back to spawn.");
        transform.position = spawnPoint;
        rb.linearVelocity = Vector2.zero;
    }

    // Starts the wandering coroutine.
    void StartWandering()
    {
        if (wanderCoroutine != null)
        {
            StopCoroutine(wanderCoroutine);
        }
        wanderCoroutine = StartCoroutine(Wander());
    }

    public void SetStateAnimationIndex(PlayerState state, int index = 0)
    {
        IndexPair[state] = index;
    }

    public void PlayStateAnimation(PlayerState state)
    {
        _prefabs.PlayAnimation(state, IndexPair[state]);
    }

    // Visualize detection and wander radii in the editor.
    void OnDrawGizmos()
    {
        // Draw detection radius.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Draw wander radius (centered on spawn).
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spawnPoint, wanderRadius);
    }
}