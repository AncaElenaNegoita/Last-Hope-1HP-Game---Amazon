using UnityEngine;

public class EnemyInsanity : MonoBehaviour
{
    private Rigidbody2D my_rb;
    private Animator animator;
    private Transform player;

    [Header("Movement Settings")]
    [SerializeField] private float normalSpeed = 1.5f;
    [SerializeField] private float dashSpeed = 6f;
    [SerializeField] private float dashInterval = 2f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float lifetime = 7f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private int damage = 2;
    [SerializeField] private GameObject explosionEffect;

    [Header("Vision Settings")]
    [SerializeField] private float detectionRadius = 8f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer;

    private Vector2 movementDirection;
    private bool isDashing = false;
    private float dashTimer;
    private float lifeTimer;
    private float dashCooldown;

    void Start()
    {
        my_rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        my_rb.freezeRotation = true;
        lifeTimer = lifetime;
        dashCooldown = dashInterval;
    }

    void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Destroy(gameObject);
            return;
        }

        dashCooldown -= Time.deltaTime;
        if (dashCooldown <= 0f && !isDashing)
        {
            StartDash();
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                EndDash();
            }
        }

        UpdateMovement();
        UpdateAnimator();
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            float currentSpeed = isDashing ? dashSpeed : normalSpeed;
            my_rb.linearVelocity = movementDirection * currentSpeed;
        }
    }

    private void UpdateMovement()
    {
        if (player != null)
        {
            movementDirection = (player.position - transform.position).normalized;
        }
    }

    private void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        // You could add visual/audio effects here for dash start
    }

    private void EndDash()
    {
        isDashing = false;
        dashCooldown = dashInterval;
        // You could add visual/audio effects here for dash end
    }

    private void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetFloat("Horizontal", movementDirection.x);
            animator.SetFloat("Vertical", movementDirection.y);
            animator.SetFloat("Speed", movementDirection.magnitude);
            animator.SetBool("IsDashing", isDashing);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Explode();
            collision.gameObject.SetActive(false);  
        }
    }

    private void Explode()
    {
        //Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, playerLayer);
        //foreach (Collider2D hit in hits)
        //{
        //    if (hit.CompareTag("Player"))
        //    {
        //        hit.GetComponent<PlayerHealth>().TakeDamage(damage);
        //    }
        //}

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0.5f, 0, 0.3f);
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = new Color(1, 0, 1, 0.3f);
        Gizmos.DrawWireSphere(transform.position, explosionRadius);

        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}