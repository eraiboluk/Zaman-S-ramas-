using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MissionProgress : MonoBehaviour
{
    private bool KillingAnimals=false;
    public int HowManyAnimals=10;
    public int killCount = 0;
    private bool Trading=false;
    public int tradeCount = 10;
    public int totalTrades = 0;
    private bool DoingGoodToPoor=false;
    public int GivingCount = 10;
    public int TotalGivings = 0;
    private bool BeingGood=false;
    public Text text;
    public bool farid;
    public bool father;
    public bool king;
    public bool serena;
    public bool entered=false;
    public int missionCount = 0;

    void Update()
    {
        if(killCount >= HowManyAnimals && !KillingAnimals){
            KillingAnimals = true;
            text.enabled=true;
            FindObjectOfType<AudioManager>().Toggle("MissionComplete",1);
            Invoke("MissionCompleted", 3f);
            missionCount++;
            if(missionCount >= 4){
                Invoke("GameWon", 3f);    
            }
        }
        if(TotalGivings >= GivingCount && !DoingGoodToPoor){
            DoingGoodToPoor = true;
            text.enabled=true;
            FindObjectOfType<AudioManager>().Toggle("MissionComplete",1);
            Invoke("MissionCompleted", 3f);
            missionCount++;
            if(missionCount >= 4){
                Invoke("GameWon", 3f);   
            }
        }
        if(totalTrades >= tradeCount && !Trading){
            Trading = true;
            text.enabled=true;
            FindObjectOfType<AudioManager>().Toggle("MissionComplete",1);
            Invoke("MissionCompleted", 3f);
            missionCount++;
            if(missionCount >= 4){
                Invoke("GameWon", 3f);   
            }
        }
        if(farid && father && king && serena && !entered){
            entered=true;
            text.enabled=true;
            FindObjectOfType<AudioManager>().Toggle("MissionComplete",1);
            Invoke("MissionCompleted", 3f);
            missionCount++;
            if(missionCount >= 4){
                Invoke("GameWon", 3f);   
            }
        }
    }
    void MissionCompleted(){
        text.enabled=false;
    }
    void GameWon(){
        FindObjectOfType<AudioManager>().Toggle("GameWon",1);
        SceneManager.LoadScene(3);
    }
}
