using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class lvl_1 : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other){
        // Debug.Log("hereeeeeee");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    
}
