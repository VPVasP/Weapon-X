using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineFreeLook;

public class FruitSnack : MonoBehaviour
{
    public PlayerController controller;
    public GameObject fruitSound;
    public GameObject player;
    public float followSpeed = 5f; 
    private bool isFollowingPlayer = false;
    public GameObject work;
    private void Start()
    {
        GameObject myobject = GameObject.Find("EatSound");
        fruitSound = myobject;
        controller = FindObjectOfType<PlayerController>();
        player = GameObject.FindGameObjectWithTag("Player");
        isFollowingPlayer = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        int randomHealth = Random.Range(20, 40);

        if (other.CompareTag("Player") && controller.health < 100)
        {
            fruitSound.GetComponent<AudioSource>().Play();
            Destroy(gameObject, 0.5f);

            int healthToAdd = Mathf.Min(100 - controller.health, randomHealth);

            controller.health += healthToAdd;
            controller.healthSlider.value = controller.health;
        }
    }

    private void Update()
    {
      
        if (!isFollowingPlayer && Vector3.Distance(transform.position, player.transform.position) < 5f)
        {
            isFollowingPlayer = true;
        }
        if (isFollowingPlayer)
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize();
            transform.position += direction * followSpeed * Time.deltaTime;
        }
    }
}
