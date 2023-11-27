using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondCamera : MonoBehaviour
{
    private Camera cam;
    private float camFOV;
    public float zoomSpeed;
    private float mouseScrollInput;
    public float minZoom = 30f;
    public float maxZoom = 60f;
    public Transform player;
    public Vector3 camOffset;
    public float Smoothness = 0.5f;

    private void Start()
    {
        //cam = Camera.main;
        player = GameManager.instance.players[1];
        camFOV = cam.fieldOfView;
        camOffset = transform.position - player.transform.position;
    }

    void Update()
    {
        Vector3 newPos = player.transform.position +camOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, Smoothness);
        CameraZoom();


    }

    private void CameraZoom()
    {
        mouseScrollInput = Input.GetAxis("Mouse ScrollWheel");
        camFOV -= mouseScrollInput * zoomSpeed;
        camFOV = Mathf.Clamp(camFOV, minZoom, maxZoom);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, camFOV, zoomSpeed);
        Debug.Log("Current Zoom: " + camFOV);

    }



}