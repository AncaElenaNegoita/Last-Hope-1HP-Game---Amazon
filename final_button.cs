using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FinalButton : MonoBehaviour
{
    private bool playerInRange = false;
    public float interactionCooldown = 0.5f;
    private bool canInteract = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void Update()
    {
        if (playerInRange && canInteract && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(TransitionToNextScene());
        }
    }

    IEnumerator TransitionToNextScene()
    {
        canInteract = false;

        // Fade to black
        if (FadeManager.Instance != null)
        {
            yield return FadeManager.Instance.FadeToBlack();
        }

        // Load next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);

        // Reset interaction cooldown
        yield return new WaitForSeconds(interactionCooldown);
        canInteract = true;
    }
}