using UnityEngine;
using UnityEngine.UI;

public class TextTrigger : MonoBehaviour
{
    [Header("UI Text Reference")]
    public Text infoText; // Assign your UI Text in Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && infoText != null)
        {
            infoText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && infoText != null)
        {
            infoText.gameObject.SetActive(false);
        }
    }
}