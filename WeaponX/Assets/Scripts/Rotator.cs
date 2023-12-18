using UnityEngine;
public class Rotator : MonoBehaviour { 
public float rotationSpeed = 100.0f;  //our rotation value
void Update()
{
    //we rotate this transform
    transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
}

}
