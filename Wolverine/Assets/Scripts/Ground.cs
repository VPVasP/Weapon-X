using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public AudioSource enemyDead;
    public Collision enemy;
  
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            enemy = other;
            GameManager.instance.enemiesKilled += 1;
            GameManager.instance.enemiesKilledText.text = "Enemies Killed: " + GameManager.instance.enemiesKilled;
            GameManager.instance.aud.Pause();
            enemyDead.Play();
            Invoke("KillEnemyAfterTime", 2f);
        }
    }
    void KillEnemyAfterTime()
    {
        Destroy(enemy.gameObject);
        GameManager.instance.aud.UnPause();
    }
}
