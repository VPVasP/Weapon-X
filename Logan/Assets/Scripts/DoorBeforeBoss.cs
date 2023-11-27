using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorBeforeBoss : MonoBehaviour
{
    private void Start()
    {
        this.GetComponent<MeshRenderer>().enabled = true;
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
        GameManager.instance.aud.Stop();
        this.GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<BoxCollider>().isTrigger = false;
        BossManager.instance.enabled = true;
        BossManager.instance.bossMusic.Play();
        GameManager.instance.TriggerCase21();
        GameManager.instance.wilsonFiskDialogue.SetActive(false);
    }
    public void DisableMeshAndTrigger()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<BoxCollider>().isTrigger = true;
        GameManager.instance.wilsonFiskDialogue.SetActive(true);
        GameManager.instance.wilsonFiskDialogue.GetComponentInChildren<TextMeshProUGUI>().text = "Wilson Fisk " + "Logan,how dare you come here?!GET HIM!";
    }
}
