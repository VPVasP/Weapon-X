using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Security.Cryptography;

public class GameManager : MonoBehaviour
{
    public AudioSource aud;
    public List<Transform> players = new List<Transform>(); //our player list of transforms
    public static GameManager instance; 
    public AudioClip InGameMusic;
    public TextMeshProUGUI enemiesKilledText; //refrence to our enemies killed ui text
    public int enemiesKilled; //enemies killed number
    public GameObject winText; //our wintext gameobject
    public GameObject[] waves; //our array of waves
    public GameObject fruit; //our fruit gameobject to be spawned
    public GameObject fruitEffect; //our fruit effect gameobject 
    public Transform fruitPos; //the fruit transform to be spawned the fruit
    //our bools to handle the game
    private bool hasSpawnedFruit = false;
    private bool hasSpawnedEnemiesHall = false;
    private bool hasSpawnedEnemiesPhase2 = false;
    private bool hasSpawnedEnemiesHall2 = false;
    private bool hasDoneCase21 = false;
    public bool triggeredCase21=false;
    public bool hasSpawnedSecondPlayer = false;
    public bool isInPhase1=false;
    public bool isInPhase2 = false;
    public bool isInPhase3 = false;
    public bool isInRound1 = false;
    public bool isInRound2 = false;
    private bool hasSpawnedEnemies = false;
    private bool hasTriggeredEndOfWave1 = false;
    private bool isSlowedTime = false;
    public bool disableMeshAndTrigger = false;
    public Camera[] cameras; //the cameras array
    public GameObject[] enemies; //our enemies array of gameobjects to be spawned
    public Vector3 gizmosCubeSize = new Vector3(5f, 5f, 5f); //initial gizmoscubesize 
    public Vector3 gizmosPosition = new Vector3(-82,50, -70);//initialgizmosposiiton
    public int enemiesToSpawn; //the enemies number to be spawned
    public Transform secondPlayer; //our second playertransform
    public Slider secondPlayerSlider; //our second player slider
    public Slider secondPlayerRage; //our second player rage slider
    public int secondPlayerHealth; //our second player health
    public GameObject spawnPlayer2Text,player1Text; //the texts ui for the names of the players
    public GameObject[] phases; //our phases array
    public GameObject[] doors; //our doors array
    public Transform[] spawnPoint; //the spawn points array for the enemies to be spawned to 
    public Transform[] reSpawnPoints;// the spawn points array for the player when he dies to respawn to
    private Transform mainCameraTransform; //our main camera refrence
    public GameObject wilsonFiskDialogue; //our dialogue gameobject 
    public GameObject protectiveWallHealth; //the protective wall health game object
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
        Invoke("PlayStart", 1f); //play the playstart method after 1 second
        cameras[1].gameObject.SetActive(false);
        enemiesKilledText.text = "Enemies Killed: " + enemiesKilled;
        hasSpawnedSecondPlayer = false;
        isInRound1 = true;
        enemiesToSpawn = 3;
        EnemySpawner(enemiesToSpawn);
        secondPlayerSlider.gameObject.SetActive(false);
        doors[0].SetActive(true);
        phases[0].SetActive(false);
        BossManager.instance.enabled = false;
        isInPhase1 = true;
        isInPhase2 = false;
        isInPhase3 = false;
        mainCameraTransform = Camera.main.transform;
        wilsonFiskDialogue.SetActive(false);
        secondPlayerRage.gameObject.SetActive(false);
    }



    void Update()
    {
      //if the enemies killed are 21 we trigger the case 21
        if (enemiesKilled == 21)
        {
            triggeredCase21 = true;
        }
        //switch based on the enemies killed number
        switch (enemiesKilled)
        {
            case 3://check if 3 enemies are killed
                if (!hasTriggeredEndOfWave1 && enemiesKilled == 3)
                {
                    StartCoroutine(EndOfWave1());
                    hasTriggeredEndOfWave1 = true;
                }
                break;
            case 5://check if 5 enemies are killed
                if (!hasSpawnedEnemies) 
                {
                    enemiesToSpawn = 4;
                    EnemySpawner(enemiesToSpawn);
                    hasSpawnedEnemies = true;
                }

                break;
            case 6://check if 6 enemies are killed and call the fruit spawned function
                if (!hasSpawnedFruit)
                {
                    FruitSpawner();
                    hasSpawnedFruit = true;
                }
                break;


            case 10: //check if 10 enemies are killed
                if (!hasSpawnedEnemiesHall)
                {
                    waves[0].GetComponent<TextMeshProUGUI>().text = "Progress Through the Hall";
                    waves[0].SetActive(true);
                    Invoke("DeactivateText", 3f);
                    doors[0].SetActive(false);
                    phases[0].SetActive(true);
                    gizmosPosition = new Vector3(-74.2f, 51.4f, 4f);
                    Instantiate(enemies[0], spawnPoint[0].position, Quaternion.identity);
                    Instantiate(enemies[0], spawnPoint[1].position, Quaternion.identity);
                    GameObject newFruit = Instantiate(fruit, spawnPoint[0].position + new Vector3(5, 1, -5f), Quaternion.identity);
                    hasSpawnedEnemiesHall = true;
                    hasSpawnedFruit = true;
                }
                break;




            case 12://check if 12 enemies are killed 
                doors[1].SetActive(false);
                enemiesToSpawn = 7;
                isInPhase1 = false;
                isInPhase2 = true;
                if (!hasSpawnedEnemiesPhase2)
                {

                    EnemySpawnerPhase2(enemiesToSpawn);
                    hasSpawnedEnemiesPhase2 = true;
                }
                break;
            case 19:

                doors[2].SetActive(false);
             
                if (!hasSpawnedEnemiesHall2)
                {
                    waves[0].GetComponent<TextMeshProUGUI>().text = "Procceed,Boss fight after this Hall";
                    waves[0].SetActive(true);
                    Invoke("DeactivateTextSecond", 5f);
                    Instantiate(enemies[0], spawnPoint[2].position, Quaternion.identity);
                    Instantiate(enemies[1], spawnPoint[3].position, Quaternion.identity);
                    hasSpawnedEnemiesHall2 = true;
                }
                break;

            case 21:
                if (triggeredCase21 == true)
                {
                    TriggerCase21();
                    isInPhase3 = true;
                    isInPhase2 = false;
                }
                break;

            case 23:

                break;


        }  //check if 'B' key is pressed and second player is not spawned
        if (Input.GetKeyDown(KeyCode.B) && !hasSpawnedSecondPlayer)
        {


            secondPlayer.gameObject.SetActive(true);
            secondPlayerSlider.gameObject.SetActive(true);
            cameras[1].gameObject.SetActive(true);

            player1Text.SetActive(true);
            secondPlayerSlider.value = secondPlayerHealth;

            spawnPlayer2Text.SetActive(true);
            hasSpawnedSecondPlayer = true;
            secondPlayerRage.gameObject.SetActive(true);
        } //follow the second player if he has been spawned
        if (hasSpawnedSecondPlayer == true)
        {
            FollowSecondPlayer();
        }
    }//trigger case 21 when 21 enemies are killedlo
    public void TriggerCase21()
    {

        if (!disableMeshAndTrigger)
        {
            doors[3].GetComponent<DoorBeforeBoss>().DisableMeshAndTrigger();//we call the function of the door to disable mesh and trigger
            disableMeshAndTrigger = true;
          
        }

        isInPhase2 = false;
        isInPhase3 = true;
        triggeredCase21 = true;
        
        
}
        
    //end of wave 1 enumerator
        IEnumerator EndOfWave1()
    {
        aud.volume = 0.3f;

        yield return new WaitForSeconds(3f);
        EndWave1();
    }

    public void EndWave1()
    {

        aud.volume = 1f;
        isInRound1 = false;
        isInRound2 = true;
        enemiesToSpawn = 3;
        EnemySpawner(enemiesToSpawn); //we call the enemies spawner and we assign the 3 value to it to spawn 3 enemies
    }
    //ondraw gizmos function
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(gizmosPosition, gizmosCubeSize); 
    }
    //we get a random point in the gizmos and we get a random x,y,z values
    Vector3 GetRandomPointInCube(Vector3 offset)
    {
        Vector3 cubeCenter = gizmosPosition;
        Vector3 cubeExtents = gizmosCubeSize / 2f;

        float randomX = Random.Range(cubeCenter.x - cubeExtents.x, cubeCenter.x + cubeExtents.x);
        float randomY = Random.Range(cubeCenter.y - cubeExtents.y, cubeCenter.y + cubeExtents.y);
        float randomZ = Random.Range(cubeCenter.z - cubeExtents.z, cubeCenter.z + cubeExtents.z);


        return new Vector3(randomX, randomY, randomZ) + offset;
    }
 
    //enemies spawner function 
    public void EnemySpawner(int numberOfEnemies)
    {
        int enemiesClones = numberOfEnemies;


        if (enemies.Length > 0)
        {
            for (int i = 0; i < enemiesClones; i++)
            {
                GameObject enemy = enemies[0]; //we assign the enemy 0 from the array
                //we set random x,y,z values
                float offsetX = Random.Range(-4f,4f);
                float offsetY = Random.Range(-0.5f, 0.5f);
                float offsetZ = Random.Range(-4f,7f);
                //we assign the random spawn point the randompoint in cube function
                Vector3 randomSpawnPoint = GetRandomPointInCube(new Vector3(offsetX, offsetY, offsetZ)); 
                Instantiate(enemy, randomSpawnPoint, Quaternion.identity); //we instatiate the enemy in the randomspawn point
            }


        }
        
    }
    //same stuff as the previous function
    public void EnemySpawnerPhase2(int numberOfEnemies)
    {
        int enemiesClones = numberOfEnemies;


        if (enemies.Length > 0)
        {
            for (int i = 0; i < enemiesClones; i++)
            {
                GameObject enemy = enemies[1];

                float offsetX = Random.Range(-4f, 4f);
                float offsetY = Random.Range(-0.5f, 0.5f);
                float offsetZ = Random.Range(-4f, 7f);

                Vector3 randomSpawnPoint = GetRandomPointInCube(new Vector3(offsetX, offsetY, offsetZ));
                Instantiate(enemy, randomSpawnPoint, Quaternion.identity);
            }


        }

    }//function that spawns enemies in transform positions 
    public void EnemySpawnerInTransforms(int numberOfEnemies)
    {
        int enemiesClones = numberOfEnemies;


        if (enemies.Length > 0)
        {
            for (int i = 0; i < enemiesClones; i++)
            {
                GameObject enemy = enemies[Random.Range(0, enemies.Length)]; //we get a random range from the array of the enemies

                
                //our transform that is the spawnpoint at 0 from the array spawn point
                Transform transformPoint1 = spawnPoint[0].transform; 

                Instantiate(enemy, transformPoint1.position, Quaternion.identity); //we instantiate the enemy at the transformpoint
            }


        }

    }
    //we deactivate the wilson dialogue and the waves 0 from the array waves
    public void DeactivateText()
    {
        waves[0].SetActive(false);
        wilsonFiskDialogue.SetActive(false);
    }
    public void DeactivateTextSecond()
    {
        waves[0].SetActive(false);
        wilsonFiskDialogue.SetActive(false);
    }
    //play start function
    public void PlayStart()
    {
        waves[0].SetActive(true);

        Invoke("PlayMusic", 3f); //play music function after 3 seconds
    }

  //play end function
    public void PlayEnd()
    {
        winText.SetActive(true);
    
        Invoke("Close", 5f);//play close function after 3 seconds
    }
    //play music function
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
    //our restart function that restarts the game
    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    //the fruit spawner that spawns the fruit gameobject in the fruitposiiton transform
    public void FruitSpawner()
    {

        GameObject spawnEffect = Instantiate(fruitEffect, fruitPos.position, Quaternion.identity);
        Destroy(spawnEffect, 1.5f);


        GameObject newFruit = Instantiate(fruit, fruitPos.position, Quaternion.identity);
    }

    //our follow second player function
    public void FollowSecondPlayer()
    {

        if (players.Count >= 2) //if the players are 2 or more 
        {
            
            if (cameras.Length >= 2)//if the cameras are 2 or more 
            {
                //we change the rect of both cameras to create a split screen
                cameras[0].rect = new Rect(0f, 0.5f, 1f, 0.5f);

               
                cameras[1].rect = new Rect(0f, 0f, 1f, 0.5f);

            }
        }
    }
    
}

