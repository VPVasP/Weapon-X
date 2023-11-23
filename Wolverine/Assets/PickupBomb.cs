using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBomb : MonoBehaviour
{
    private Rigidbody rb;
    private bool isPickedUp;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Transform playerTransform = collision.gameObject.transform;

            collision.gameObject.GetComponent<PlayerController>().isPickedUp = true;
            transform.SetParent(playerTransform);

           
            transform.localPosition = new Vector3(0f, 3f, 0f);

            isPickedUp = true;
            Debug.Log("PickedUp");
        }
    }

    private void Update()
    {
        if (isPickedUp && Input.GetKey(KeyCode.T))
        {
            float forcePower = 10f;
            Vector3 force = new Vector3(0f, forcePower, forcePower);

            rb.AddForce(force, ForceMode.Impulse);
            isPickedUp = false;
        }
    }
}
