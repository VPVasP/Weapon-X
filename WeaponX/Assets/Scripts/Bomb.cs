using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject bombEffect; //Reference to our bomb effect
    private void OnCollisionEnter(Collision collision) //When bomb collides with something
    {
       
        if (collision.gameObject.CompareTag("Ground"))
        {

            GameObject theDeathEffect = Instantiate(bombEffect, transform.position, Quaternion.identity); //when the bomb touches the ground with tag ground it instatiates an effect and destroys the bomb
            Destroy(theDeathEffect, 0.3f);
            Destroy(gameObject,0.3f);
            
        }
        if (collision.gameObject.CompareTag("Player")) //if it collides with gameobject with tag player
        {

            collision.gameObject.GetComponent<PlayerController>().health -= 10; //we redudce the player health
            collision.gameObject.GetComponent<PlayerController>().aud.clip = collision.gameObject.GetComponent<PlayerController>().hurtSound; //we set the audiosource to the hurt sound
            collision.gameObject.GetComponent<PlayerController>().aud.Play();//we play the sound effect
            collision.gameObject.GetComponent<PlayerController>().healthSlider.value = collision.gameObject.GetComponent<PlayerController>().health; //we update the healthslider to our health
            collision.gameObject.GetComponent<Animator>().SetTrigger("DamageSmall");//we play the hurt animation
            GameObject theDeathEffect = Instantiate(bombEffect, transform.position, Quaternion.identity); //we insatiate the bomb effect
            Destroy(theDeathEffect, 0.3f); //we destroy both the effect and bomb
            Destroy(this.gameObject, 0.3f);
        }
        if (collision.gameObject.CompareTag("Enemy")) //if it collides with gameobject with tag enemy
        {

            collision.gameObject.GetComponent<Enemy>().health -= 10; //we reduce the enemy health
            collision.gameObject.GetComponent<Animator>().SetTrigger("damageSmall"); //we set the animation trigger to damage animation

          

            GameObject theDeathEffect = Instantiate(bombEffect, transform.position, Quaternion.identity); //we instantiate the bomb effect
            Destroy(theDeathEffect, 0.3f);
            Destroy(gameObject, 0.3f);
        }
    }

    }
