using UnityEngine;

public class move_bullet : MonoBehaviour

{
    public float speed = 10f;
    public float lifetime = 5f; // Optional: destroy after a few seconds to prevent lingering

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime); // Assumes projectile faces right
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Projectile hit: {collision.name}");
        if (collision.CompareTag("PlayerBullet") || collision.CompareTag("Text"))
            return;
        Destroy(gameObject);
    }
}

