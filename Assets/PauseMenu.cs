using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    // Update is called once per frame
    void Start(){
        pauseMenuUI.SetActive(false);
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)){
            if(GameIsPaused){
                Resume();
            }else{
                Pause();

            }
        }
        if(Input.GetKeyDown(KeyCode.Q)){
            QuitGame();
        }
    }
    public void Resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

    }
    void Pause(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.01f;
        GameIsPaused = true;

    }
    public void QuitGame (){
        Application.Quit();
    }
}
