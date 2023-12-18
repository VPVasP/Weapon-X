using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProtectiveWall : MonoBehaviour
{
    public Slider wallHealthSlider; //our slider 
    public float wallHealth; //our wall health value
    //the audios
    public AudioSource wallSound;
    public AudioSource wallBreak;
    private void Start()
    {
     //   wallHealthSlider.gameObject.SetActive(true);
        wallHealth = 100;
        wallHealthSlider.value = wallHealth;
    }
    //the wall loses health and plays the audio
    public void LoseHealth()
    {
        wallHealth -= 20;
        wallHealthSlider.value = wallHealth;
        wallSound.Play();
    }
    private void Update()
    {
        //if the wall health is lower than 0 or equal we call GameManager.instance.PlayEnd();
        if (wallHealth <= 0)
        {
            wallBreak.Play();
            GameManager.instance.PlayEnd();
        }
    }
}
