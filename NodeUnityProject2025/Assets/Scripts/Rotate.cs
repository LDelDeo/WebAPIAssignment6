using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotationSpeed = 30f; // Rotation speed in degrees per second

    void Update()
    {

        // Rotate the object around the Y-axis

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

    }
}
