using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBomb : MonoBehaviour
{
    private Rigidbody rb;
    private bool isPickedUp;
    private bool isGrounded;
    public GameObject bombEffect;
    public PlayerController controller;
    public BossManager bossManager;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = FindObjectOfType<PlayerController>();
        bossManager = FindObjectOfType<BossManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")&&isGrounded ==true)
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
            Destroy(this.gameObject, 2f);
            Debug.Log("HitWall");
        }
            if (collision.gameObject.CompareTag("Ground"))
            {
            isGrounded = true;
                
                Debug.Log("HitFloor");
            }
        }

    private void Update()
    {
       
        if (isPickedUp && Input.GetKey(KeyCode.T))
        {
            rb.constraints = RigidbodyConstraints.None;
            float forcePower = 5f;
            Vector3 force = new Vector3(0f, forcePower, forcePower);
            controller.GetComponent<PlayerController>().isThrowing = true;
            rb.AddForce(force, ForceMode.Impulse);
            isPickedUp = false;
            transform.SetParent(null);
        }
    }
}
