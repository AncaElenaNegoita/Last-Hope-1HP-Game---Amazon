using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Settings")]
    public int totalSquares = 4;
    public float disappearDelay = 0.5f;

    private int collectedSquares = 0;

    public void CollectSquare()
    {
        collectedSquares++;

        // Check if all squares are collected
        if (collectedSquares >= totalSquares)
        {
            // Disappear after delay
            Destroy(gameObject, disappearDelay);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f); // Semi-transparent red
        Gizmos.DrawCube(transform.position, GetComponent<BoxCollider2D>().size);
    }
}