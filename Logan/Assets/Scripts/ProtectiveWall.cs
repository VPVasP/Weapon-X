using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProtectiveWall : MonoBehaviour
{
    public Slider wallHealthSlider;
    public float wallHealth;
    public AudioSource wallSound;
    public AudioSource wallBreak;
    private void Start()
    {
     //   wallHealthSlider.gameObject.SetActive(true);
        wallHealth = 100;
        wallHealthSlider.value = wallHealth;
    }
    public void LoseHealth()
    {
        wallHealth -= 20;
        wallHealthSlider.value = wallHealth;
        wallSound.Play();
    }
    private void Update()
    {
        if (wallHealth <= 0)
        {
            wallBreak.Play();
            GameManager.instance.PlayEnd();
        }
    }
}
