using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject bombEffect;
    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.CompareTag("Ground"))
        {
            GameObject theDeathEffect = Instantiate(bombEffect, transform.position, Quaternion.identity);
            Destroy(gameObject,0.3f);
        }
        if (collision.gameObject.CompareTag("Player"))
        {

            collision.gameObject.GetComponent<PlayerController>().health -= 10;
            collision.gameObject.GetComponent<Animator>().SetTrigger("DamageSmall");
            Destroy(gameObject, 0.3f);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {

            collision.gameObject.GetComponent<Enemy>().health -= 10;
            collision.gameObject.GetComponent<Animator>().SetTrigger("damageSmall");
            Destroy(gameObject, 0.3f);
        }
    }

    }
