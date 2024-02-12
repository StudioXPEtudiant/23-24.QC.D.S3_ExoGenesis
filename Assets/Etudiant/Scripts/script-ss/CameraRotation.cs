using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public Transform target; // The object the camera will rotate around
    public float rotationSpeed = 5.0f; // Speed of rotation

    void Update()
    {
        // Check if target exists
        if (target)
        {
            // Get the horizontal input for rotation
            float mouseX = Input.GetAxis("Mouse X");

            // Calculate the rotation amount
            float rotationAmount = mouseX * rotationSpeed;

            // Rotate the camera around the target
            transform.RotateAround(target.position, Vector3.up, rotationAmount);
        }
    }
}

