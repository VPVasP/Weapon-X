﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;
public class GameManager : MonoBehaviour
{
    public AudioSource[] roundsEffect;
    public AudioSource aud;
    public List<Transform> players = new List<Transform>();
//    public AudioClip Door;
    public static GameManager instance;
    public AudioClip InGameMusic;
    public TextMeshProUGUI enemiesKilledText;
    public int enemiesKilled;
    public GameObject endText;
    public GameObject winText;
    public GameObject[] waves;
    public GameObject fruit;
    public GameObject fruitEffect;
    public Transform fruitPos;
    private bool hasSpawnedFruit = false;
    public bool hasSpawnedSecondPlayer = false;
    public Camera[] cameras;
    public bool isInRound1 = false;
    public bool isInRound2 = false;
    public GameObject[] enemies;
    public Vector3 gizmosCubeSize1 = new Vector3(5f, 5f, 5f);
    public Vector3 gizmosPosition1;
    private bool hasSpawnedEnemies = false;
    private bool hasTriggeredEndOfWave1 = false;
    private bool isSlowedTime = false;
    public int enemiesToSpawn;
    public GameObject EndCanvas;
    public int Coins;
   // public TextMeshProUGUI coinsText;
    public Transform secondPlayer;
    public Slider secondPlayerSlider;
    public int secondPlayerHealth;
    public GameObject spawnPlayer2Text,player1Text;
    public GameObject[] phases;
    public GameObject[] doors;
    public Transform spawnPoint;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
        Invoke("PlayStart", 1f);
        cameras[1].gameObject.SetActive(false);
        endText.SetActive(false);
        enemiesKilledText.text = "Enemies Killed: " + enemiesKilled;
        hasSpawnedSecondPlayer = false;
        isInRound1 = true;
        enemiesToSpawn = 3;
        EnemySpawner(enemiesToSpawn);
        secondPlayerSlider.gameObject.SetActive(false);
        spawnPlayer2Text.SetActive(true);
        doors[0].SetActive(true);
        phases[0].SetActive(false);
    }



    void Update()
    {

        switch (enemiesKilled)
        {
            case 3:
                if (!hasTriggeredEndOfWave1 && enemiesKilled == 3)
                {
                    StartCoroutine(EndOfWave1());
                    hasTriggeredEndOfWave1 = true;
                }
                break;
            case 5:
                if (!hasSpawnedEnemies)
                {
                    enemiesToSpawn = 4;
                    EnemySpawner(enemiesToSpawn);
                    hasSpawnedEnemies = true;
                }

                break;
            case 6:
                if (!hasSpawnedFruit)
                {
                    FruitSpawner();
                    hasSpawnedFruit = true;
                }
                break;


            case 10:

                if (!hasSpawnedEnemies)
                {
                    doors[0].SetActive(false);
                    phases[0].SetActive(true);
                    enemiesToSpawn = 2;
                    EnemySpawnerInTransforms(enemiesToSpawn);
                    hasSpawnedEnemies = true;
                }
                
                
               
                break;
            case 12:
                doors[1].SetActive(false);
                break;
            default:

                break;
        }
        if (Input.GetKeyDown(KeyCode.B) && !hasSpawnedSecondPlayer)
        {

            
            secondPlayer.gameObject.SetActive(true);
            secondPlayerSlider.gameObject.SetActive(true);
            cameras[1].gameObject.SetActive(true);

            player1Text.SetActive(true);
            secondPlayerSlider.value = secondPlayerHealth;

            spawnPlayer2Text.SetActive(false);
            hasSpawnedSecondPlayer = true;

        }
        if (hasSpawnedSecondPlayer == true)
        {
            FollowSecondPlayer();
        }
    }
    IEnumerator EndOfWave1()
    {
        roundsEffect[1].Play();
        aud.volume = 0.3f;
        waves[1].SetActive(true);

        yield return new WaitForSeconds(3f);
        waves[1].SetActive(false);
        EndWave1();
    }

    public void EndWave1()
    {

        aud.volume = 1f;
        isInRound1 = false;
        isInRound2 = true;
        enemiesToSpawn = 3;
        EnemySpawner(enemiesToSpawn);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(gizmosPosition1, gizmosCubeSize1);
    }

    Vector3 GetRandomPointInCube(Vector3 offset)
    {
        Vector3 cubeCenter = gizmosPosition1;
        Vector3 cubeExtents = gizmosCubeSize1 / 2f;

        float randomX = Random.Range(cubeCenter.x - cubeExtents.x, cubeCenter.x + cubeExtents.x);
        float randomY = Random.Range(cubeCenter.y - cubeExtents.y, cubeCenter.y + cubeExtents.y);
        float randomZ = Random.Range(cubeCenter.z - cubeExtents.z, cubeCenter.z + cubeExtents.z);


        return new Vector3(randomX, randomY, randomZ) + offset;
    }


    public void EnemySpawner(int numberOfEnemies)
    {



        int enemiesClones = numberOfEnemies;


        if (enemies.Length > 0)
        {
            for (int i = 0; i < enemiesClones; i++)
            {
                GameObject enemy = enemies[Random.Range(0, enemies.Length)];

                float offsetX = Random.Range(-4f,4f);
                float offsetY = Random.Range(-0.5f, 0.5f);
                float offsetZ = Random.Range(-4f,7f);

                Vector3 randomSpawnPoint = GetRandomPointInCube(new Vector3(offsetX, offsetY, offsetZ));
                Instantiate(enemy, randomSpawnPoint, Quaternion.identity);
            }


        }
        
    }
    public void EnemySpawnerInTransforms(int numberOfEnemies)
    {



        int enemiesClones = numberOfEnemies;


        if (enemies.Length > 0)
        {
            for (int i = 0; i < enemiesClones; i++)
            {
                GameObject enemy = enemies[Random.Range(0, enemies.Length)];

                float offsetX = Random.Range(-1f,1f);
                float offsetY = Random.Range(-0.5f, 0.5f);
                float offsetZ = Random.Range(-2f,2f);

                Transform transformPoint = spawnPoint;
                Instantiate(enemy, transformPoint.position, Quaternion.identity);
            }


        }

    }
    public void PlayStart()
    {
        waves[0].SetActive(true);
        roundsEffect[0].Play();

        Invoke("PlayMusic", 3f);
    }

    public void PlayClose()
    {
        endText.SetActive(true);
        Invoke("Close", 5f);
    }
    public void PlayEnd()
    {
        winText.SetActive(true);
    
        Invoke("Close", 5f);
    }

    public void PlayMusic()
    {
        aud.volume = 0.5f;
        aud.clip = InGameMusic;
        aud.Play();
        waves[0].SetActive(false);
    }

    void Close()
    {
        Restart();
    }

    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    public void FruitSpawner()
    {

        GameObject spawnEffect = Instantiate(fruitEffect, fruitPos.position, Quaternion.identity);
        Destroy(spawnEffect, 1.5f);


        GameObject newFruit = Instantiate(fruit, fruitPos.position, Quaternion.identity);
    }
    public void RageSlowMotion()
    {
        if (players[0].GetComponent<PlayerController>().isRaging == true)
        {
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<Enemy>().speed = 1;
            }
        }
    }
    public void RageEndMotion()
    {
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<Enemy>().speed = 4;
        }
    
    }
    public void AddRage()
    {
        if (players[0].GetComponent<PlayerController>().rage < 100 & players[0].GetComponent<PlayerController>().isRaging==false)
        {
            players[0].GetComponent<PlayerController>().AddRage();
        }
       
    }
    public void FollowSecondPlayer()
    {

        if (players.Count >= 2)
        {
            
            if (cameras.Length >= 2)
            {
                
                cameras[0].rect = new Rect(0f, 0.5f, 1f, 0.5f);

               
                cameras[1].rect = new Rect(0f, 0f, 1f, 0.5f);

            }
        }
    }
}
