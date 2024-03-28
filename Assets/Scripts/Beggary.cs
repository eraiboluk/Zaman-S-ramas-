using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Beggary : MonoBehaviour
{
    public Text textAlt;
    public Text textUst;
    private bool isAvalible = false;
    public string playerTag = "Player";
    public PlayerInventory playerInventory;
    public MissionProgress playerMission;

    void Start()
    {
        GameObject player = GameObject.Find("Player");
        playerInventory = player.GetComponent<PlayerInventory>();
        playerMission = player.GetComponent<MissionProgress>();
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            ShowLegacyTextAlt("Dilenciye Para vermek İçin V'ye bas (-100 Para)");
            isAvalible = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            HideLegacyTextAlt();
            isAvalible = false;
        }
    }

    void Update()
    {
        if (isAvalible)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                if(playerInventory.coinCount>=100){
                    playerInventory.coinCount -= 100;
                    playerMission.TotalGivings++;
                }
                else{
                    ShowLegacyTextUst("Bakiye Yetersiz");
                    Invoke("HideLegacyTextUst", 2f);

                }
            }
        }
    }

    void ShowLegacyTextAlt(string text)
    {
        textAlt.text = text;
        textAlt.enabled = true;
    }
    void ShowLegacyTextUst(string text)
    {
        textUst.text = text;
        textUst.enabled = true;
    }
    void HideLegacyTextAlt()
    {
        textAlt.enabled = false;
    }
    void HideLegacyTextUst()
    {
        textUst.enabled = false;
    }
}
