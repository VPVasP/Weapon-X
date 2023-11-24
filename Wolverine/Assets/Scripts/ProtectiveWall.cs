using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProtectiveWall : MonoBehaviour
{
    public Slider wallHealthSlider;
    public float wallHealth;


    private void Start()
    {
  //    wallHealthSlider = GetComponentInChildren<Slider>();
        wallHealth = 100;
        wallHealthSlider.value = wallHealth;
    }
    public void LoseHealth()
    {
        wallHealth -= 20;
        wallHealthSlider.value = wallHealth;
    }
 
}
