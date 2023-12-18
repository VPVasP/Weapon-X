using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager instance; //our instance so it can be refrenced in other scripts
    public GameObject bombs;//the bomb object
    public GameObject playerBomb;//the player bomb object
    public Vector3 gizmosCubeSize1 = new Vector3(5f, 5f, 5f);//starting size of the gizmos cube 
    public Vector3 gizmosPosition1; //our gizmos position
    private PlayerController controller;//refrence to the player movement script
    public GameObject protectiveWall;//our protective wall refrence
    public GameObject bossHealthSlider;//our bosshealthslider refrence
    public GameObject[] enemies;//our array of enemies
    public bool hasSpawnedEnemies=false;//bool if it has spawned enemies
    public AudioSource bossMusic;//our music audio for boss fight
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        controller = GetComponent<PlayerController>(); //our refrence to the player movement script
        StartCoroutine(BossEnumerator());//we begin coroutines for the boss fight
        StartCoroutine(SpawnPlayerBomb());
        protectiveWall.SetActive(true);//we set both protective wall and health sliders to true
        bossHealthSlider.SetActive(true);
}

  public  IEnumerator BossEnumerator()
    {

        yield return new WaitForSeconds(1f); //we wait for the x amount of seconds

        float bombDuration = Random.Range(20f, 30f); //our random bomb duration values
        StartCoroutine(DropBombs(bombDuration)); //we start the coroutine to drop bombs

        yield return new WaitForSeconds(bombDuration);
        hasSpawnedEnemies = false; //we set it to false since we do not want to spawn any enemies
     
    }

  //coroutine to drop the bombs 
   IEnumerator DropBombs(float duration)
    {
        float randomEnemies = Random.Range(3f, 8f); //the random enemies to be spawned
        EnemySpawner(randomEnemies); // we call our enemy spawned function 
       
        while (duration > 0 && !hasSpawnedEnemies) //if the duration is above zero and we havent spawned enemies
        {
            //our values for random x,y,z values to be dropped bombs
            float offsetX = Random.Range(-5f, 5f);
            float offsetY = Random.Range(-0.5f, 0.5f);
            float offsetZ = Random.Range(-5f, 5f);
            Vector3 randomSpawnPoint = GetRandomPointInCube(new Vector3(offsetX, offsetY, offsetZ));
            GameObject theBombs = Instantiate(bombs, randomSpawnPoint, Quaternion.identity); //we instatiate the bomb at a random spawn point

            yield return new WaitForSeconds(1f);
            duration -= 1f;
        }

    }
    //coroutine for player bombs
   public IEnumerator SpawnPlayerBomb()
    {
        yield return new WaitForSeconds(Random.Range(3f, 20f));
        //our values for random x,y,z values to be dropped for player bomb
        float offsetX = Random.Range(-5f, 5f);
        float offsetY = Random.Range(-0.5f, 0.5f);
        float offsetZ = Random.Range(-5f, 5f);
        Vector3 randomSpawnPoint = GetRandomPointInCube(new Vector3(offsetX, offsetY, offsetZ));

        Instantiate(playerBomb, randomSpawnPoint, Quaternion.identity);//we instatiate the player bomb to a random spawn point
    }
    //we draw the gizmos to draw the cube in the scene view 
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(gizmosPosition1, gizmosCubeSize1);
    }
    //function that gets a random point inside the gizmos cube
    Vector3 GetRandomPointInCube(Vector3 offset)
    {
        Vector3 cubeCenter = gizmosPosition1;
        Vector3 cubeExtents = gizmosCubeSize1 / 2f;
        //our values for random x,y,z values  for the random point inside the gizmos cube
        float randomX = Random.Range(cubeCenter.x - cubeExtents.x, cubeCenter.x + cubeExtents.x);
        float randomY = Random.Range(cubeCenter.y - cubeExtents.y, cubeCenter.y + cubeExtents.y);
        float randomZ = Random.Range(cubeCenter.z - cubeExtents.z, cubeCenter.z + cubeExtents.z);

        return new Vector3(randomX, randomY, randomZ) + offset;
    }
    //enemy spawner function
    public void EnemySpawner(float numberOfEnemies)
    {



        float enemiesClones = numberOfEnemies;


        if (enemies.Length > 0)
        {
            for (int i = 0; i < enemiesClones; i++)
            {
                GameObject enemy = enemies[0]; //we spawn the first enemy of the array
                //our values for random x,y,z values  to spawn enemies at a random position of these values
                float offsetX = Random.Range(-4f, 4f);
                float offsetY = Random.Range(-0.5f, 0.5f);
                float offsetZ = Random.Range(-4f, 7f);

                Vector3 randomSpawnPoint = GetRandomPointInCube(new Vector3(offsetX, offsetY, offsetZ));
                Instantiate(enemy, randomSpawnPoint, Quaternion.identity); //we instatiate the enemy in the a random spawb point
            }


        }

    }
}



