using Unity.VisualScripting;
using UnityEngine;

public class SmartSlimeController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private Transform player;
    public int enemy_sanity = 10;
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float chaseSpeed = 3.5f;
    [SerializeField] private float moveDuration = 2f;
    [SerializeField] private float pauseDuration = 2f;
    [SerializeField] private float wallCheckDistance = 0.6f;

    [Header("Vision Settings")]
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField][Range(0, 360)] private float fovAngle = 90f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer;

    private Vector2 currentDirection;
    private float moveTimer;
    private bool isMoving = false;
    private bool isChasing = false;
    private Vector2 playerLastSeenPosition;
    private float lostPlayerTimer;
    private const float giveUpChaseTime = 2f; // Time to keep chasing after losing sight

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        rb.freezeRotation = true;
        moveTimer = pauseDuration; // Start with pause
        ChooseSmartDirection();
    }

    void Update()
    {
        if (!isChasing)
        {
            PatrolBehavior();
        }
        else
        {
            // Count how long we've lost sight of player
            if (!CanSeePlayer())
            {
                lostPlayerTimer += Time.deltaTime;
                if (lostPlayerTimer > giveUpChaseTime)
                {
                    StopChasing();
                }
            }
            else
            {
                lostPlayerTimer = 0f; // Reset timer if we see player again
            }
        }

        CheckForPlayer();
        UpdateAnimator();
    }

    void FixedUpdate()
    {
        if (isChasing)
        {
            ChaseBehavior();
        }
    }

    private bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector2 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // Check if player is within detection radius and FOV
        if (distanceToPlayer <= detectionRadius &&
            Vector2.Angle(currentDirection, directionToPlayer) <= fovAngle / 2)
        {
            // Check line of sight
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                directionToPlayer.normalized,
                distanceToPlayer,
                obstacleLayer);

            return (hit.collider == null);
        }
        return false;
    }

    private void StopChasing()
    {
        isChasing = false;
        lostPlayerTimer = 0f;
        currentDirection = Vector2.zero;
        moveTimer = pauseDuration; // Start with pause
        isMoving = false;
    }

    private void PatrolBehavior()
    {
        if (isChasing) return;

        moveTimer -= Time.deltaTime;

        if (moveTimer <= 0)
        {
            isMoving = !isMoving;

            if (isMoving)
            {
                ChooseSmartDirection();
                moveTimer = moveDuration;
            }
            else
            {
                currentDirection = Vector2.zero;
                moveTimer = pauseDuration;
            }
        }

        rb.linearVelocity = currentDirection * moveSpeed;
    }

    private void ChooseSmartDirection()
    {
        Vector2[] possibleDirections = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
        bool[] validDirections = new bool[4];
        int validCount = 0;

        // Check which directions are clear
        for (int i = 0; i < 4; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                possibleDirections[i],
                wallCheckDistance,
                obstacleLayer);

            validDirections[i] = (hit.collider == null);
            if (validDirections[i]) validCount++;
        }

        // If all directions blocked, just wait
        if (validCount == 0)
        {
            currentDirection = Vector2.zero;
            return;
        }

        // Choose randomly from valid directions
        int randomIndex = Random.Range(0, validCount);
        int actualIndex = 0;

        for (int i = 0; i < 4; i++)
        {
            if (validDirections[i])
            {
                if (randomIndex == 0)
                {
                    currentDirection = possibleDirections[i];
                    break;
                }
                randomIndex--;
            }
        }
    }

    private void ChaseBehavior()
    {
        Vector2 chaseDirection = (player.position - transform.position).normalized;
        rb.linearVelocity = chaseDirection * chaseSpeed;
        currentDirection = chaseDirection; // Update facing direction while chasing
    }

    private void CheckForPlayer()
    {
        if (isChasing || player == null) return;

        if (CanSeePlayer())
        {
            isChasing = true;
            playerLastSeenPosition = player.position;
            lostPlayerTimer = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Handle player damage here
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<sanity_mechanic>().sanity -= enemy_sanity;
            Destroy(this.gameObject);
            //Destroy(collision.gameObject); // Optional: destroy the bullet on impact
            collision.gameObject.SetActive(false);
            
            return;

        }
        else if (collision.gameObject.layer == obstacleLayer)
        {
            // Hit a wall - stop and choose new direction
            if (!isChasing)
            {
                currentDirection = Vector2.zero;
                moveTimer = pauseDuration;
                isMoving = false;
            }
        }
    }

    private void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetFloat("Horizontal", currentDirection.x);
            animator.SetFloat("Vertical", currentDirection.y);
            animator.SetFloat("Speed", rb.linearVelocity.magnitude);
            animator.SetBool("IsChasing", isChasing);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw detection radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Draw FOV lines
        Vector3 fovLine1 = Quaternion.Euler(0, 0, fovAngle / 2) * currentDirection * detectionRadius;
        Vector3 fovLine2 = Quaternion.Euler(0, 0, -fovAngle / 2) * currentDirection * detectionRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + fovLine1);
        Gizmos.DrawLine(transform.position, transform.position + fovLine2);

        // Draw wall detection rays
        Vector2[] dirs = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
        foreach (Vector2 dir in dirs)
        {
            Gizmos.color = Physics2D.Raycast(transform.position, dir, wallCheckDistance, obstacleLayer)
                ? Color.red : Color.green;
            Gizmos.DrawRay(transform.position, dir * wallCheckDistance);
        }

        // Draw chase line
        if (isChasing)
        {
            Gizmos.color = CanSeePlayer() ? Color.magenta : Color.gray;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {

            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<sanity_mechanic>().sanity -= enemy_sanity;
            Destroy(this.gameObject);
            Destroy(other.gameObject); // Optional: destroy the bullet on impact
            return; 
        }

    
    }
    


}