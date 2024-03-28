using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animals : MonoBehaviour
{
    public int maxCan = 100;
    public int currentCan;
    public float panikModuSure = 10f;
    public float minWanderInterval = 2f;
    public float maxWanderInterval = 5f;
    public float wanderRadius = 10f;
    public int SwordDamage = 30;

    private bool panikModuAktif = false;
    private Transform target;
    private NavMeshAgent agent;
    private float wanderTimer;
    public int wanderSpeed = 2;
    public int panicSpeed = 7;

    private float panicTimer = 2;
    public float panicTime = 2;

    public float panicDuration = 10;
    public float panicDurationTimer = 10;

    public float cooldownTime = 1f;
    private bool canTrigger = true;

    public PlayerInventory playerInventory;
    public MissionProgress playerMission;

    void Start()
    {
        currentCan = maxCan;
        agent = GetComponent<NavMeshAgent>();
        target = new GameObject().transform; 
        GameObject player = GameObject.Find("Player");
        playerInventory = player.GetComponent<PlayerInventory>();
        playerMission = player.GetComponent<MissionProgress>();
        SetRandomWanderTimer();
    }

    void Update()
    {
        if (!panikModuAktif)
        {
            Wander();
        }
        else{
            panicDurationTimer -= Time.deltaTime;
            if (panicDurationTimer >= 0f)
            {
                Panic();
            }
            else{
                panicDurationTimer = panicDuration;
                panikModuAktif = false;
            }      
        }
        if (!IsInNavMesh())
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (canTrigger)
        {
            if (other.CompareTag("Sword")) 
            {
                panikModuAktif = true;
                HasarVer(SwordDamage);
                canTrigger = false;
                Invoke("ResetCooldown", cooldownTime);
                FindObjectOfType<AudioManager>().Toggle("Combat",1);
            }
        }
    }

    private void ResetCooldown()
    {
        canTrigger = true;
    }

    void HasarVer(int hasarMiktari)
    {
        currentCan -= hasarMiktari;

        if (currentCan <= 0)
        {
            currentCan = 0;
            HayvanOldu();
            playerInventory.foodCount++;
            playerMission.killCount++;
        }
    }

    void HayvanOldu()
    {
        agent.enabled = false;
        transform.rotation = Quaternion.Euler(90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        Invoke("YokEt", 1f);
    }

    void YokEt()
    {
        Destroy(gameObject);
    }

    void Wander()
    {
        agent.speed = wanderSpeed;
        wanderTimer -= Time.deltaTime;

        if (wanderTimer <= 0f)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            SetRandomWanderTimer();
        }
    }

    void SetRandomWanderTimer()
    {
        wanderTimer = Random.Range(minWanderInterval, maxWanderInterval);
    }

    Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }
    void Panic()
    {
        panicTimer -= Time.deltaTime;

        if (panicTimer <= 0f)
        {
            agent.speed = panicSpeed;
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            panicTimer=panicTime;
        }    
    }
    bool IsInNavMesh()
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(transform.position, out hit, 0.1f, NavMesh.AllAreas);
    }
}
