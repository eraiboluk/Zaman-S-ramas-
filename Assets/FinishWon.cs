using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishWon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(){
        Invoke("PlayGame", 6.0f);
    }
    public void PlayGame(){
        SceneManager.LoadScene(0);
    }
    
}
