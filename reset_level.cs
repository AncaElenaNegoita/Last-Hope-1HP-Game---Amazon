using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;



public class reset_level : MonoBehaviour
{
    //Start is called once before the first execution of Update after the MonoBehaviour is created


    [SerializeField] GameObject background;


    GameObject player = null;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null )
            player = GameObject.FindGameObjectWithTag("Player");
        if(!player.activeInHierarchy)
            background.SetActive(true);

        if (Input.GetKeyDown(KeyCode.Backspace) == true) { 
            background.SetActive(true);
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
            
        }

    
}
