using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panorama : MonoBehaviour
{
    public bool rotate;                                                             // Enable/disable ability to rotate
    public Vector3 rotationSpeed;                                                   // Rotation speed
    public float rotationMultiplier;                                                // Rotation multiplier

    private void FixedUpdate() {
        if (rotate) {
            transform.Rotate(rotationSpeed, rotationMultiplier * Time.deltaTime);   // Rotate camera
        }
    }
}
