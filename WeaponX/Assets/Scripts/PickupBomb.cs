using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickupBomb : MonoBehaviour
{
    private Rigidbody rb; //refrence to our rigibody component 
    //bools to check if it grounded and ispickedup
    private bool isPickedUp;
    private bool isGrounded;
    public GameObject bombEffect; //the bomb effect when it explodes gameobject
    public PlayerController controller; //our movement script
    public Enemy[] enemy; //our enemy array to spawn enemies
    public string[] wilsonFiskSentences; //the sentences of wilson fisk string
    private AudioSource boomSound; //bomb sound effect
    private void Start()
    {
        //refrences to the components
        rb = GetComponent<Rigidbody>();
        controller = FindObjectOfType<PlayerController>();
        enemy = FindObjectsOfType<Enemy>();
        boomSound = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if it collides with the gameobject with tag player and it is grounded 
        if (collision.gameObject.CompareTag("Player") && isGrounded == true)
        {
            //the player transform is equal to the collision
            Transform playerTransform = collision.gameObject.transform;

            collision.gameObject.GetComponent<PlayerController>().isCurrentlyPickingUp = true; //we set the bool of the player movement to true
            transform.SetParent(playerTransform);
            //we update the local position so it is on top of the player
            transform.localPosition = new Vector3(0f, 3f, 0f);
            isPickedUp = true;
            Debug.Log("PickedUp");
            rb.constraints = RigidbodyConstraints.FreezePosition; //we freeze the bomb rigibody constrains
        }
        //if it collides with the gameobject with tag protectiveWall
        if (collision.gameObject.CompareTag("ProtectiveWall"))
        {
            //we call the lose function of the collision gameobject
            collision.gameObject.GetComponent<ProtectiveWall>().LoseHealth();
            //we instatiate the deatheffect when the bomb hits the ground
            GameObject theDeathEffect = Instantiate(bombEffect, transform.position, Quaternion.identity);
            //we set the bossmanager bool to false
            BossManager.instance.hasSpawnedEnemies = false;
            //we stop coroutines from the boss script and start new ones
            StopCoroutine(BossManager.instance.BossEnumerator());
            StartCoroutine(BossManager.instance.BossEnumerator());
            StopCoroutine(BossManager.instance.SpawnPlayerBomb());
            StartCoroutine(BossManager.instance.SpawnPlayerBomb());
            //we disable the rendered and boxcollider and the text
            this.GetComponent<Renderer>().enabled = false;
            this.GetComponent<BoxCollider>().enabled = false;
            this.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            //we set the dialogue to true and change the dialogue text to a random value from the strings array
            GameManager.instance.wilsonFiskDialogue.SetActive(true);
            GameManager.instance.wilsonFiskDialogue.GetComponentInChildren<TextMeshProUGUI>().text = "WILSON FISK " + wilsonFiskSentences[Random.Range(0, wilsonFiskSentences.Length)];
            // we call the function DisableWilsonFiskDialogue after 3 seconds
            Invoke("DisableWilsonFiskDialogue", 3f);
            boomSound.Play(); //we play the sound effect
            Debug.Log("HitWall");
        }
        //we check if it grounded 
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            Debug.Log("HitFloor");
        }
    }
    //we disable the wilson fisk dialouge function
    public void DisableWilsonFiskDialogue()
    {
        GameManager.instance.wilsonFiskDialogue.SetActive(false);
    }
    private void Update()
    {
        //if we have picked it up and we press T we throw the bomb
        if (isPickedUp && Input.GetKey(KeyCode.T))
        {
            rb.constraints = RigidbodyConstraints.None;
            float forcePower = 10f;
            Vector3 force = new Vector3(0f, forcePower, forcePower);
            controller.GetComponent<PlayerController>().isThrowing = true;
            rb.AddForce(force, ForceMode.Impulse);
            isPickedUp = false;
            transform.SetParent(null);
        }
    }
}
   

