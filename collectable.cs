using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CollectibleSquare : MonoBehaviour
{
    [Header("Settings")]
    public float destroyDelay = 0f;
    public Door doorController; // Reference to the door

    private void Start()
    {
        // Automatically find the door if not set
        if (doorController == null)
        {
            doorController = FindObjectOfType<Door>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Notify door controller this was collected
            if (doorController != null)
            {
                doorController.CollectSquare();
            }

            // Disappear with delay
            Destroy(gameObject, destroyDelay);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(transform.position, GetComponent<BoxCollider2D>().size);
    }
}