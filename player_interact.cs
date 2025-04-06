using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Settings")]
    public KeyCode interactKey = KeyCode.E;
    public float interactRange = 2f;
    public GameObject interactPrompt;

    [Header("Trapdoor")]
    public float disappearDuration = 2f;

    private MapController _map;
    private bool _canInteract;
    private bool _canUseTrapdoor;
    private Rigidbody2D _rb;
    private Collider2D _col;
    private SpriteRenderer _sprite;

    void Start()
    {
        _map = FindObjectOfType<MapController>();
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
        _sprite = GetComponent<SpriteRenderer>();
        if (interactPrompt) interactPrompt.SetActive(false);
    }

    void Update()
    {
        // Check for both interactables AND trapdoors
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            interactRange,
            LayerMask.GetMask("Interactable", "Trapdoor")
        );

        _canInteract = false;
        _canUseTrapdoor = false;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Interactable")) _canInteract = true;
            if (hit.CompareTag("Trapdoor")) _canUseTrapdoor = true;
        }

        if (Input.GetKeyDown(interactKey))
        {
            if (_canInteract || _map.IsMapVisible())
            {
                _map.ToggleMap(); // Original map toggle
            }
        }

        if (interactPrompt)
            interactPrompt.SetActive(_canInteract && !_map.IsMapVisible());
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}