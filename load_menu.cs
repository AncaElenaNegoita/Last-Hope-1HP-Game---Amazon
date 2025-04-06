using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class load_menu : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other){
        // Debug.Log("hereeeeeee");
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
