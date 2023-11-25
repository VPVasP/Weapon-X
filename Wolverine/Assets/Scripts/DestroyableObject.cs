using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    public Collision enemy;
    public GameObject destroyableEffect;
    public AudioSource destroyAudio;
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            enemy = other;
            GameManager.instance.enemiesKilled += 1;
            GameManager.instance.enemiesKilledText.text = "Enemies Killed: " + GameManager.instance.enemiesKilled;
            Destroy(other.gameObject);
            GameObject theDeathEffect = Instantiate(destroyableEffect, transform.position, Quaternion.identity);
            destroyAudio.Play();
            Destroy(this.gameObject,0.5f);
        }
    }
   
}