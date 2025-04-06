using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class InteractableWall : MonoBehaviour
{
    [Header("Visual Feedback")]
    public Color highlightColor = Color.yellow;

    private SpriteRenderer _renderer;
    private Color _originalColor;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _originalColor = _renderer.color;
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        gameObject.tag = "Interactable";
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            _renderer.color = highlightColor;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            _renderer.color = _originalColor;
    }
}