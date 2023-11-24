using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBeforeBoss : MonoBehaviour
{
    public BossManager bossManager;
    private void Start()
    {
        this.GetComponent<MeshRenderer>().enabled = true;
        bossManager = FindObjectOfType<BossManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Invoke("EnableMeshAndTrigger", 1f);
        }
    }

    public void EnableMeshAndTrigger()
    {
        this.GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<BoxCollider>().isTrigger = false;
        bossManager.enabled = true;
    }
    public void DisableMeshAndTrigger()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<BoxCollider>().isTrigger = true;
    }
}
