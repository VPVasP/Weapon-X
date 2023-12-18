using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineFreeLook;

public class FruitSnack : MonoBehaviour
{
    public PlayerController controller; //refrence to the player controller
    public GameObject fruitSound; //fruit sound for the clone object
    public GameObject player;//refrence to our player
    public float followSpeed = 5f; //the follow speed
    private bool isFollowingPlayer = false; //bool to check if the fruit follows the player
    private void Start()
    {
        // we find the fruit eating sound object by name
        GameObject myFruitSound = GameObject.Find("EatSound");
        fruitSound = myFruitSound;
        controller = FindObjectOfType<PlayerController>(); //we find the player scripts
        player = GameObject.FindGameObjectWithTag("Player");//we find the object with tag player
        isFollowingPlayer = false; //we set it to false so it wont follow player at the start
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && controller.health < 100) //if the collided object has tag player and his health is below 100
        {
            float randomHealth = Random.Range(20, 40); //random health to add

            fruitSound.GetComponent<AudioSource>().Play(); //we play the audio
            Destroy(gameObject, 0.5f);//we destroy this fruit object

            float healthToAdd = Mathf.Min(100 - controller.health, randomHealth); //we don't want it to go above 100 the health

            controller.health += healthToAdd; //we add the health to the player
            controller.healthSlider.value = controller.health; // we update the ui health slider
        }
    }

    private void Update()
    {
        //we check if the fruit is not following the player and the distance is at a certain distance
        if (!isFollowingPlayer && Vector3.Distance(transform.position, player.transform.position) < 5f)
        {
            isFollowingPlayer = true; 
        }
        //we cant follow the player if the health is 100
        if(player.GetComponent<PlayerController>().health == 100)
        {
            isFollowingPlayer = false;
        }
        //if the fruit can follow the player we move the fruit towards the player with a speed value
        if (isFollowingPlayer)
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize();
            transform.position += direction * followSpeed * Time.deltaTime;
        }
    }
}
