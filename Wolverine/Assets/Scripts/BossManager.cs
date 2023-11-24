using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public GameObject bombs;
    public GameObject playerBomb;
    public Vector3 gizmosCubeSize1 = new Vector3(5f, 5f, 5f);
    public Vector3 gizmosPosition1;
    private PlayerController controller;
    public GameObject protectiveWall;
    public GameObject healthSlidrer;
    public GameObject[] enemies;
    public bool hasSpawnedEnemies=false;
    void Start()
    {
        controller = GetComponent<PlayerController>();
        StartCoroutine(BossEnumerator());
        StartCoroutine(SpawnPlayerBomb());
        protectiveWall.SetActive(true);
        healthSlidrer.SetActive(true);
}

  public  IEnumerator BossEnumerator()
    {
      //  protectiveWall.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        float bombDuration = Random.Range(20f, 30f);
        StartCoroutine(DropBombs(bombDuration));

        yield return new WaitForSeconds(bombDuration);
        hasSpawnedEnemies = false;
        //    protectiveWall.gameObject.SetActive(false);
    }

  
   IEnumerator DropBombs(float duration)
    {
        float randomEnemies = Random.Range(3f, 8f);
        EnemySpawner(randomEnemies);
       
        while (duration > 0 && !hasSpawnedEnemies)
        {
            float offsetX = Random.Range(-5f, 5f);
            float offsetY = Random.Range(-0.5f, 0.5f);
            float offsetZ = Random.Range(-5f, 5f);
            Vector3 randomSpawnPoint = GetRandomPointInCube(new Vector3(offsetX, offsetY, offsetZ));
            GameObject theBombs = Instantiate(bombs, randomSpawnPoint, Quaternion.identity);

            yield return new WaitForSeconds(1f);
            duration -= 1f;
        }

    }

   public IEnumerator SpawnPlayerBomb()
    {
        yield return new WaitForSeconds(Random.Range(3f, 20f));

        float offsetX = Random.Range(-5f, 5f);
        float offsetY = Random.Range(-0.5f, 0.5f);
        float offsetZ = Random.Range(-5f, 5f);
        Vector3 randomSpawnPoint = GetRandomPointInCube(new Vector3(offsetX, offsetY, offsetZ));

        Instantiate(playerBomb, randomSpawnPoint, Quaternion.identity);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
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
    public void EnemySpawner(float numberOfEnemies)
    {



        float enemiesClones = numberOfEnemies;


        if (enemies.Length > 0)
        {
            for (int i = 0; i < enemiesClones; i++)
            {
                GameObject enemy = enemies[0];

                float offsetX = Random.Range(-4f, 4f);
                float offsetY = Random.Range(-0.5f, 0.5f);
                float offsetZ = Random.Range(-4f, 7f);

                Vector3 randomSpawnPoint = GetRandomPointInCube(new Vector3(offsetX, offsetY, offsetZ));
                Instantiate(enemy, randomSpawnPoint, Quaternion.identity);
            }


        }

    }
}



