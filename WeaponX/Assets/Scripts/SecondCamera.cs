using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondCamera : MonoBehaviour
{
    private Camera cam;
    public Transform player;
    public Vector3 camOffset;
    public float Smoothness = 0.5f;

    private void Start()
    {
        cam = Camera.main; //we set our camera to be our main camera
        camOffset = transform.position - player.transform.position;//we calculate the camera offset as the diference from the player posiiton
    }

    void Update()
    {
        Vector3 newPos = player.position + camOffset; //we calculate the new posiiton for the camera by adding the player posiiton and offset
        transform.position = Vector3.Slerp(transform.position, newPos, Smoothness); //we set  the camera's position towards the new position using slerp
    }



}