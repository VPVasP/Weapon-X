using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script : MonoBehaviour
{
    public float rage;
    public float health;
    // Update is called once per frame
    void Update()
    {
        rage -= 10 * Time.deltaTime;
        health += 2 * Time.deltaTime;
        Debug.Log(rage);
        Debug.Log(health);
    }
}
