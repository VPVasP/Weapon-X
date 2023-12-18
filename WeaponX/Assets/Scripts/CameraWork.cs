using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraWork : MonoBehaviour
{
 

    private Camera cam; //our camera refrence
    public Transform[] player; //our player transfom array
    public  Vector3 camOffset; //our camera offset
    public float Smoothness =0.5f;//smoothness value
    private void Start()
    {
        cam = Camera.main; //we set our camera to be our main camera
        camOffset = transform.position - player[0].transform.position;//we calculate the camera offset as the diference from the player posiiton
    }

    void Update()
    {
        Vector3 newPos = player[0].position + camOffset; //we calculate the new posiiton for the camera by adding the player posiiton and offset
        transform.position = Vector3.Slerp(transform.position, newPos, Smoothness); //we set  the camera's position towards the new position using slerp
    }
}