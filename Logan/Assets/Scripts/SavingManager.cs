using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class SavingManager : MonoBehaviour
{
    public static SavingManager instance;
    public TextMeshProUGUI enemiesText;
    [SerializeField] private PlaytimeData playtimeData = new PlaytimeData();
    [SerializeField] private EnemiesKilleData enemiesKilledData =new EnemiesKilleData();
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        LoadData();
    }
    public EnemiesKilleData EnemiesKilledData
    {
        get {
            return enemiesKilledData;
        }
    }
    public void SaveData()
    { 
        enemiesKilledData.enemiesKilled = GameManager.instance.enemiesKilled;
        string enemiesKilled = JsonUtility.ToJson(enemiesKilledData,true);
        File.WriteAllText(Application.dataPath + "/EnemiesKilledData.json", enemiesKilled);
    }
    public void LoadData()
    {
        string loadEnemiesKilled = File.ReadAllText(Application.dataPath + "/EnemiesKilledData.json");
        EnemiesKilleData data = JsonUtility.FromJson<EnemiesKilleData>(loadEnemiesKilled);
        enemiesText.text = loadEnemiesKilled;
    }
    public void loadDataIntoText()
    {
        enemiesText.text = enemiesKilledData.enemiesKilled.ToString();
    }
    private void Update()
    {
        
            SaveData();
        
    }
}

[System.Serializable]
public class PlaytimeData
{

}
[System.Serializable]
public class EnemiesKilleData
{
    public int enemiesKilled;
    
}
