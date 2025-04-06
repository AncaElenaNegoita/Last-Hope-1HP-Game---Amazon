//using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D))]
//public class PlayerController : MonoBehaviour
//{
//    [Header("Movement")]
//    public float moveSpeed = 5f;
//    public float acceleration = 10f;
//    public float deceleration = 10f;
//    private Vector2 movementInput;
//    private Rigidbody2D rb;

//    [Header("Interaction")]
//    public KeyCode interactKey = KeyCode.E;
//    public float interactRange = 2f;
//    private GameObject currentInteractable;
//    private MapController mapController;
//    public GameObject interactPrompt;

//    [Header("Trapdoor")]
//    public float disappearDuration = 3f;
//    private Vector2 respawnPosition;
//    private bool isActive = true;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        if (interactPrompt) interactPrompt.SetActive(false);
//        respawnPosition = transform.position;
//    }

//    void Update()
//    {
//        if (!isActive) return;

//        movementInput = new Vector2(
//            Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0,
//            Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0
//        ).normalized;

//        CheckForInteractables();

//        if (Input.GetKeyDown(interactKey))
//        {
//            TryInteract();
//        }
//    }

//    void FixedUpdate()
//    {
//        if (!isActive) return;

//        Vector2 targetVelocity = movementInput * moveSpeed;
//        Vector2 velocityChange = targetVelocity - rb.linearVelocity;

//        if (movementInput.magnitude > 0.1f)
//        {
//            rb.AddForce(velocityChange * acceleration);
//        }
//        else
//        {
//            rb.AddForce(-rb.linearVelocity * deceleration);
//        }
//    }

//    void CheckForInteractables()
//    {
//        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, interactRange);
//        currentInteractable = null;

//        foreach (var hitCollider in hitColliders)
//        {
//            if (hitCollider.CompareTag("Interactable") || hitCollider.CompareTag("Trapdoor"))
//            {
//                currentInteractable = hitCollider.gameObject;
//                break;
//            }
//        }

//        if (interactPrompt)
//        {
//            interactPrompt.SetActive(currentInteractable != null);
//            if (currentInteractable)
//            {
//                interactPrompt.transform.position =
//                    Camera.main.WorldToScreenPoint(currentInteractable.transform.position + Vector3.up * 0.5f);
//            }
//        }
//    }

//    void TryInteract()
//    {
//        if (currentInteractable == null) return;

//        if (currentInteractable.CompareTag("Interactable"))
//        {
//            if (mapController == null) mapController = FindObjectOfType<MapController>();
//            mapController.ToggleMap();
//        }
//        else if (currentInteractable.CompareTag("Trapdoor"))
//        {
//            Disappear();
//        }
//    }

//    public void Disappear()
//    {
//        isActive = false;
//        rb.linearVelocity = Vector2.zero;
//        rb.simulated = false;
//        GetComponent<Collider2D>().enabled = false;
//        GetComponent<SpriteRenderer>().enabled = false;

//        Invoke("Respawn", disappearDuration);
//    }

//    void Respawn()
//    {
//        transform.position = respawnPosition;
//        isActive = true;
//        rb.simulated = true;
//        GetComponent<Collider2D>().enabled = true;
//        GetComponent<SpriteRenderer>().enabled = true;
//    }

//    public void Kill()
//    {
//        if (!isActive) return;
//        Disappear();
//    }

//    void OnDrawGizmosSelected()
//    {
//        Gizmos.color = Color.blue;
//        Gizmos.DrawWireSphere(transform.position, interactRange);
//    }
//}