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
            Destroy(theDeathEffect, 0.3f);
            Destroy(gameObject,0.3f);
            
        }
        if (collision.gameObject.CompareTag("Player"))
        {

            collision.gameObject.GetComponent<PlayerController>().health -= 10;
            collision.gameObject.GetComponent<PlayerController>().aud.clip = collision.gameObject.GetComponent<PlayerController>().hurtSound;
            collision.gameObject.GetComponent<PlayerController>().aud.Play();
            collision.gameObject.GetComponent<PlayerController>().healthSlider.value = collision.gameObject.GetComponent<PlayerController>().health;
            collision.gameObject.GetComponent<Animator>().SetTrigger("DamageSmall");
            GameObject theDeathEffect = Instantiate(bombEffect, transform.position, Quaternion.identity); 
            Destroy(theDeathEffect, 0.3f);
            Destroy(this.gameObject, 0.3f);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {

            collision.gameObject.GetComponent<Enemy>().health -= 10;
            collision.gameObject.GetComponent<Animator>().SetTrigger("damageSmall");

          

            GameObject theDeathEffect = Instantiate(bombEffect, transform.position, Quaternion.identity);
            Destroy(theDeathEffect, 0.3f);
            Destroy(gameObject, 0.3f);
        }
    }

    }
