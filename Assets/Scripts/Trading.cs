using UnityEngine;
using UnityEngine.UI;

public class Trading : MonoBehaviour
{
    public float amplitude = 1.0f;  
    public float speed = 1.0f;      
    public PlayerInventory playerInventory;
    public MissionProgress playerMission;
    public string playerTag = "Player";
    private bool isAvalible = false;
    public Text textAlt;
    public Text textUst;

    private Vector3 initialPosition;

    void Start()
    {
        GameObject player = GameObject.Find("Player");
        playerInventory = player.GetComponent<PlayerInventory>();
        playerMission = player.GetComponent<MissionProgress>();
        initialPosition = transform.position;
    }

    void Update()
    {
        float newY = initialPosition.y + amplitude * Mathf.Sin(speed * Time.time);
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);

        if (isAvalible)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                if(playerInventory.foodCount>=1){
                    playerInventory.foodCount -= 1;
                    playerInventory.coinCount +=100;
                    playerMission.totalTrades++;
                }
                else{
                    ShowLegacyTextUst("Yemek Yetersiz");
                    Invoke("HideLegacyTextUst", 2f);
                }
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            ShowLegacyTextAlt("Yemek Satmak İçin V'ye basın (-1 Yemek, +100 Para)");
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
