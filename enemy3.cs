using UnityEngine;

public class StationaryKamikazeSlime : MonoBehaviour
{
    private Rigidbody2D my_rb;
    private Animator animator;
    private Transform player;
    public int enemy_sanity = 25;


    [Header("Movement Settings")]
    [SerializeField] private float chaseSpeed = 4f; // Slightly faster charge
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private int damage = 2;
    [SerializeField] private GameObject explosionEffect;

    [Header("Vision Settings")]
    [SerializeField] private float detectionRadius = 8f; // Increased from 5
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer;

    private Vector2 movementDirection;
    private bool isChasing = false;
    private Vector2 playerLastSeenPosition;
    private float lostSightTimer;
    private const float giveUpChaseTime = 1.5f;

    void Start()
    {
        my_rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        my_rb.freezeRotation = true;
        movementDirection = Vector2.zero; // Starts stationary
    }

    void Update()
    {
        if (isChasing)
        {
            ChaseUpdate();
        }
        else
        {
            // Stationary - just check for player
            if (CheckFieldOfView())
            {
                StartChasing();
            }
        }

        UpdateAnimator();
    }

    void FixedUpdate()
    {
        if (isChasing)
        {
            my_rb.linearVelocity = movementDirection * chaseSpeed;
        }
        else
        {
            my_rb.linearVelocity = Vector2.zero; // Stay completely still
        }
    }

    private void ChaseUpdate()
    {
        if (CheckFieldOfView())
        {
            // Player is visible - chase directly
            movementDirection = (player.position - transform.position).normalized;
            playerLastSeenPosition = player.position;
            lostSightTimer = 0f;
        }
        else
        {
            // Player not visible - count time lost
            lostSightTimer += Time.deltaTime;

            if (lostSightTimer > giveUpChaseTime)
            {
                StopChasing();
            }
            else
            {
                // Move toward last known position
                movementDirection = (playerLastSeenPosition - (Vector2)transform.position).normalized;
            }
        }
    }

    private void StartChasing()
    {
        isChasing = true;
        playerLastSeenPosition = player.position;
        lostSightTimer = 0f;
    }

    private void StopChasing()
    {
        isChasing = false;
        movementDirection = Vector2.zero; // Return to stationary
    }

    private bool CheckFieldOfView()
    {
        if (player == null) return false;

        Vector2 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // Check if player is within detection radius
        if (distanceToPlayer > detectionRadius) return false;

        // 360� vision - no angle check needed

        // Check line of sight (no obstacles blocking view)
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            directionToPlayer.normalized,
            distanceToPlayer,
            obstacleLayer);

        return hit.collider == null;
    }

    private void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetFloat("Horizontal", movementDirection.x);
            animator.SetFloat("Vertical", movementDirection.y);
            animator.SetFloat("Speed", movementDirection.magnitude);
            animator.SetBool("IsChasing", isChasing);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isChasing)
        {

                // Handle player damage here
                GameObject player = GameObject.FindWithTag("Player");
                player.GetComponent<sanity_mechanic>().sanity -= enemy_sanity;
                Explode();
                Destroy(this.gameObject);
                //Destroy(collision.gameObject); // Optional: destroy the bullet on impact
                collision.gameObject.SetActive(false);

                return;
            

        }
        else if (isChasing)
        {
            // Try to path around obstacles when chasing
            Vector2 avoidDirection = Vector2.Perpendicular(movementDirection);
            movementDirection = (avoidDirection + (playerLastSeenPosition - (Vector2)transform.position).normalized).normalized;
        }
    }

    private void Explode()
    {
        // Damage player if in range
        //Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, playerLayer);
        //foreach (Collider2D hit in hits)
        {
            //if (hit.CompareTag("Player"))
            //{
            //    hit.GetComponent<PlayerHealth>().TakeDamage(damage);
            //}
        }

        // Visual effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw detection radius (bigger and full 360�)
        Gizmos.color = new Color(1, 0.5f, 0, 0.3f); // Orange with transparency
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Draw explosion radius
        Gizmos.color = new Color(1, 0, 1, 0.3f); // Purple with transparency
        Gizmos.DrawWireSphere(transform.position, explosionRadius);

        // Draw line to player when chasing
        if (isChasing)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, playerLastSeenPosition);
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