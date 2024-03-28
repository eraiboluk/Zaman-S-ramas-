using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Convai.Scripts;

public class GameProgress : MonoBehaviour
{
    public int FatherBenedictusAboutHistory = 0;
    public int FatherBenedictusGoodPerson = 0;
    public int FaridMerchantAboutHistory = 0;
    public int FaridMerchantGoodPerson = 0;
    public int SerenaShadowGoodPerson = 0;
    public int SerenaShadowAboutHistory = 0;
    public int KingCedricGoodPerson = 0;
    public int KingCedricAboutHistory = 0;
    public ConvaiNPC convaiNPC;
    
    
    void Awake()
    {
        convaiNPC = GetComponent<ConvaiNPC>();
    }


    void Start()
    {
        
        FatherBenedictusGoodPerson = 0;
        FatherBenedictusAboutHistory = 0;
        FaridMerchantGoodPerson = 0;
        FaridMerchantAboutHistory = 0;

    }

    void Update()
    {
        
    }
}
