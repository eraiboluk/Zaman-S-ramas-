using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatSystem : MonoBehaviour
{
    public GameObject otherObject;
    public ProgressBar Pb;
    private Animator animator;
    public BoxCollider boxCollider;
    public float cooldownTime = 2f;
    public float nextFireTime = 0f;
    public  int noOfClicks = 0; 
    float lastClickedTime = 0;
    float maxComboDelay = 3;
    float cooldown = 0;

    void Start()
    {
        boxCollider = otherObject.GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
        Pb.gameObject.SetActive(false);
    }
    void Update()
    {
        cooldown = Mathf.Clamp(Time.time - lastClickedTime, 0, maxComboDelay);
        Pb.BarValue = (maxComboDelay-cooldown)*(100/maxComboDelay);
        if(animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 0.3f && (animator.GetCurrentAnimatorStateInfo(1).IsName("hit1") || animator.GetCurrentAnimatorStateInfo(1).IsName("hit2") || animator.GetCurrentAnimatorStateInfo(1).IsName("hit3"))){
            boxCollider.enabled = true;
            FindObjectOfType<AudioManager>().Toggle("Grunt",1);

        }

        if(animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.95f && animator.GetCurrentAnimatorStateInfo(1).IsName("hit1"))
        {
            animator.SetBool("hit1", false);
            if(!animator.GetCurrentAnimatorStateInfo(1).IsName("hit2"))
                Pb.gameObject.SetActive(true);
            boxCollider.enabled = false;
        }
        if(animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.95f && animator.GetCurrentAnimatorStateInfo(1).IsName("hit2"))
        {
            animator.SetBool("hit2", false);
            if(!animator.GetCurrentAnimatorStateInfo(1).IsName("hit3"))
                Pb.gameObject.SetActive(true);
            boxCollider.enabled = false;
        }
        if(animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.95f && animator.GetCurrentAnimatorStateInfo(1).IsName("hit3"))
        {
            animator.SetBool("hit3", false);
            Pb.gameObject.SetActive(true);
            boxCollider.enabled = false;
        }
        if(Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
            Pb.gameObject.SetActive(false);
        }
        if(Time.time > nextFireTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnClick();
            }
        }
    }
    void OnClick()
    {
        if(animator.GetBool("hasSword")){
            lastClickedTime = Time.time;
            noOfClicks++;
            if(noOfClicks == 1){
                animator.SetBool("hit1",true);
            }
            noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);                                                                                                                                                                                                     
            if(noOfClicks >= 2 && animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.6f && animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 0.8f && animator.GetCurrentAnimatorStateInfo(1).IsName("hit1"))
            {
                animator.SetBool("hit1", false);
                animator.SetBool("hit2", true);
            }
            if(noOfClicks >= 3 && animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.6f && animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 0.8f && animator.GetCurrentAnimatorStateInfo(1).IsName("hit2"))
            {  

                animator.SetBool("hit2", false); 
                animator.SetBool("hit3", true);
            }
        }
    }
}

