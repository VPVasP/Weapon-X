using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    public Collision enemy;//our enemy refrence collision
    public GameObject destroyableEffect;//the destroyable ffect
    public AudioSource destroyAudio;//our destroyable audiosource
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Enemy")) //if the collided object has tag enemy 
        {
        
            GameManager.instance.enemiesKilled += 1; //we update our enemies killed number from the game manager
            GameManager.instance.enemiesKilledText.text = "Enemies Killed: " + GameManager.instance.enemiesKilled;//we update our enemies killed UI from the game manager
            Destroy(other.gameObject); //we destroy our enemy
            GameObject DestroyableEffect = Instantiate(destroyableEffect, transform.position, Quaternion.identity); //we instatiate the destroyable effect
            destroyAudio.Play();//we play the destroy audio
            Destroy(this.gameObject,0.5f);//we destroy this object
        }
    }
   
}