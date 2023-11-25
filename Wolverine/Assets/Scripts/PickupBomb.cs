using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickupBomb : MonoBehaviour
{
    private Rigidbody rb;
    private bool isPickedUp;
    private bool isGrounded;
    public GameObject bombEffect;
    public PlayerController controller;
    public BossManager bossManager;
    public Enemy[] enemy;
    public string[] wilsonFiskSentences;
    private AudioSource boomSound;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = FindObjectOfType<PlayerController>();
        bossManager = FindObjectOfType<BossManager>();
        enemy = FindObjectsOfType<Enemy>();
        boomSound = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && isGrounded == true)
        {

            Transform playerTransform = collision.gameObject.transform;

            collision.gameObject.GetComponent<PlayerController>().isCurrentlyPickingUp = true;
            transform.SetParent(playerTransform);

            transform.localPosition = new Vector3(0f, 3f, 0f);
            isPickedUp = true;
            Debug.Log("PickedUp");
            rb.constraints = RigidbodyConstraints.FreezePosition;
        }
        if (collision.gameObject.CompareTag("ProtectiveWall"))
        {
            collision.gameObject.GetComponent<ProtectiveWall>().LoseHealth();
            GameObject theDeathEffect = Instantiate(bombEffect, transform.position, Quaternion.identity);
            bossManager.hasSpawnedEnemies = false;
            StopCoroutine(bossManager.BossEnumerator());
            StartCoroutine(bossManager.BossEnumerator());
            StopCoroutine(bossManager.SpawnPlayerBomb());
            StartCoroutine(bossManager.SpawnPlayerBomb());
            this.GetComponent<Renderer>().enabled = false;
            this.GetComponent<BoxCollider>().enabled = false;
            this.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            GameManager.instance.wilsonFiskDialogue.SetActive(true);
            GameManager.instance.wilsonFiskDialogue.GetComponentInChildren<TextMeshProUGUI>().text ="WILSON FISK " +wilsonFiskSentences[Random.Range(0, wilsonFiskSentences.Length)];
            Invoke("DisableWilsonFiskDialogue", 3f);
            boomSound.Play();
            Debug.Log("HitWall");
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            Debug.Log("HitFloor");
        }
    }
    public void DisableWilsonFiskDialogue()
    {
        GameManager.instance.wilsonFiskDialogue.SetActive(false);
    }
    private void Update()
    {

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
        if (isPickedUp)
        {
            foreach (Enemy enemyInstance in enemy)
            {
                if (enemyInstance.isPickedUpEnemyAttack)
                { 
                  
                    rb.constraints = RigidbodyConstraints.None;
                    float forcePower = 5f;
                    Vector3 force = new Vector3(0f, forcePower, forcePower);
                    controller.GetComponent<PlayerController>().isThrowing = false;
                    rb.AddForce(force, ForceMode.Impulse);
                    isPickedUp = false;
                    transform.SetParent(null);
                }
            }
        }
        
    }
   
}
