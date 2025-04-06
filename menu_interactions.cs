using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class main_menu : MonoBehaviour
{
    public void Play(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void LoadTutorial(){
        SceneManager.LoadScene(5);
    }
    public void Quit(){
        Application.Quit();
    }
}
