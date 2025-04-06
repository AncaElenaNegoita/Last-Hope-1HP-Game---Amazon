using UnityEngine;

public class RangedSlime : MonoBehaviour
{
    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint; // Position on the "front" of the slime
    public float fireRate = 0.5f;
    public float projectileSpeed = 8f;

    [Header("Detection Settings")]
    public float detectionRange = 10f;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    private Transform player;
    private float nextFireTime;
    public int enemy_sanity = 20;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (CanSeePlayer())
        {
            RotateToFacePlayer();

            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector2 direction = player.position - transform.position;
        float distance = direction.magnitude;

        if (distance > detectionRange) return false;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction.normalized,
            distance,
            obstacleLayer);

        return hit.collider == null;
    }

    void RotateToFacePlayer()
    {
        Vector2 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // Adjust for sprite rotation

        // Apply rotation to the entire slime
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Shoot()
    {
        if (projectilePrefab == null || firePoint == null) return;

        Vector2 fireDirection = (player.position - firePoint.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, transform.rotation);

        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.SetDirection(fireDirection);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        if (firePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(firePoint.position, firePoint.position + firePoint.right * 1f);
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
            Destroy(this.gameObject);
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
    }


    }