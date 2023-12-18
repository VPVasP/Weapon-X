using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteadyCanvas : MonoBehaviour
{
    private Transform mainCameraTransform; //our main camera transform refrence
    // Start is called before the first frame update
    void Start()
    {
        //we set our camera to be our main camera
        mainCameraTransform = Camera.main.transform;
    }
    //we make this object to face the camera direction
    private void LateUpdate()
    {
        transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward, mainCameraTransform.rotation * Vector3.up);
    }
}
