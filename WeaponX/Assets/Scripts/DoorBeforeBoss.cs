using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorBeforeBoss : MonoBehaviour
{
    private void Start()
    {
        this.GetComponent<MeshRenderer>().enabled = true; //we make sure the door mesh rendered is true at the start
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) //if the object that enters the trigger has tag player
        {
            Invoke("EnableMeshAndTrigger", 1f); //we invoke the method EnableMeshAndTrigger after 1 second
        }
    }

    public void EnableMeshAndTrigger()
    {
        GameManager.instance.aud.Stop(); //we stop the audio from the game manager
        this.GetComponent<MeshRenderer>().enabled = true; // enable the meshrenderer of this object
        this.GetComponent<BoxCollider>().isTrigger = false; //we get the box collider trigger and we set it to false
        BossManager.instance.enabled = true;//our boss manager gets enabled so the boss fight can begin
        BossManager.instance.bossMusic.Play();//our boss music is played
        GameManager.instance.TriggerCase21();//we trigger the case 21 of the game manager that starts the boss fight
        GameManager.instance.wilsonFiskDialogue.SetActive(false);//we set gamemanager the dialogue to false
    }
    public void DisableMeshAndTrigger()
    {
        this.GetComponent<MeshRenderer>().enabled = false;// disable the meshrenderer of this object
        this.GetComponent<BoxCollider>().isTrigger = true;//we get the box collider trigger and we set it to true
        GameManager.instance.wilsonFiskDialogue.SetActive(true);//we set the gamemanager dialogue to true
        GameManager.instance.wilsonFiskDialogue.GetComponentInChildren<TextMeshProUGUI>().text = "Wilson Fisk " + "Logan,how dare you come here?!GET HIM!"; //our starting dialogue sentecne
    }
}
