using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPlayer : MonoBehaviour
{
    public Transform round1Transform;
    public Transform round2Transform;
    public Transform Player;
    public GameObject smoke;
    public GameObject spawnEffect;
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            GameObject Smoke = Instantiate(smoke, Player.transform.position, Quaternion.identity);
            Destroy(Smoke.gameObject,1f);
            Invoke("SpawnPlayerAfterTime", 2f);
        }
    }
    void SpawnPlayerAfterTime()
    {
   
        if (GameManager.instance.isInRound1 == true)
        {
            Player.transform.position = round1Transform.transform.position;
            GameObject Spawn = Instantiate(spawnEffect, round1Transform.transform.position, Quaternion.identity);
            Destroy(Spawn.gameObject,1f);
        }
        if (GameManager.instance.isInRound2 == true)
        {
            GameObject Spawn = Instantiate(spawnEffect, round2Transform.transform.position, Quaternion.identity);
            Player.transform.position = round2Transform.transform.position;
            Destroy(Spawn.gameObject, 1f);
        }
    }
}
