//using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
//public class Player_Kill : MonoBehaviour
//{
//    [Header("Respawn Settings")]
//    public float respawnTime = 3f;
//    private Vector3 spawnPoint;

//    // Components
//    private Rigidbody2D rb;
//    private Collider2D col;
//    private SpriteRenderer sprite;
//    private bool isDead = false;

//    void Start()
//    {
//        // Get required components
//        rb = GetComponent<Rigidbody2D>();
//        col = GetComponent<Collider2D>();
//        sprite = GetComponent<SpriteRenderer>();

//        // Set initial spawn point
//        spawnPoint = transform.position;
//    }



//    // Call this to update respawn location
//    public void SetSpawnPoint(Vector3 newPosition)
//    {
//        spawnPoint = newPosition;
//    }


//    void OnTriggerEnter2D(Collider2D other)
//    {
//        // Skip if already dead or not a projectile
//        if (isDead || !(other.CompareTag("Projectile") || other.CompareTag("Enemy"))) return;
//    }
//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (isDead || !(collision.gameObject.CompareTag("Projectile") || collision.gameObject.CompareTag("Enemy"))) return;

//    }
//}