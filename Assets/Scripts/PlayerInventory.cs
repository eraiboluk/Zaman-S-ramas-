using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerInventory : MonoBehaviour
{
    public GameObject sword;
    public GameObject food;
    public float eatTime;
    public int foodCount = 0;
    public Text foodText;
    private Animator animator;
    public int coinCount = 100;
    public float playerHealth = 100;
    public ProgressBar Pb;
    public float Difficulty = 1;
    public Text CoinText;
    public float HealthRegen = 30;

    void Start()
    {
        animator = GetComponent<Animator>();
        ShowSword();
        HideFood();
        animator.SetBool("hasSword", true);
        InvokeRepeating("AzaltCan", 1f, 1f); // Belirli aralıklarla can azaltma işlemini başlat
    }

    void Update()
    {
        CoinText.text=coinCount.ToString();
        Pb.BarValue = playerHealth;
        foodText.text = foodCount.ToString();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ShowSword();
            HideFood();
            animator.SetBool("hasSword", true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            ShowFood();
            HideSword();
            animator.SetBool("hasSword", false);
        }

        if (Input.GetKeyDown(KeyCode.E) && foodCount > 0)
        {
            StartCoroutine(Eating());
            foodCount--;
            playerHealth += HealthRegen;
            playerHealth = Mathf.Clamp(playerHealth, -1, 100);
        }
        if(playerHealth <= 0)
        {
            SceneManager.LoadScene(2);
        }
    }
    IEnumerator Eating()
    {
        animator.SetBool("isEating", true);
        yield return new WaitForSeconds(eatTime);
        animator.SetBool("isEating", false);
    }

    void ShowSword()
    {
        sword.SetActive(true);
    }

    void HideSword()
    {
        sword.SetActive(false);
    }

    void ShowFood()
    {
        food.SetActive(true);
    }

    void HideFood()
    {
        food.SetActive(false);
    }
    void AzaltCan()
    {
        playerHealth -= Difficulty;
    }
}
