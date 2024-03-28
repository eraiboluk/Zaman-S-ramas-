using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame(){
        SceneManager.LoadScene(1);
    }
    public void QuitGame(){
        Debug.Log("User quitted from the game");
        Application.Quit();
    }
    // Start is called before the first frame update
    
}
