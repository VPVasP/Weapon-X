using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    public TextMeshProUGUI EnemiesKilled;
    //public TextMeshProUGUI playTimeText;
    void Start()
    {
   //     SavingManager.instance.LoadData();


        EnemiesKilled.text = "Enemies Killed: " + SavingManager.instance.EnemiesKilledData.enemiesKilled.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
