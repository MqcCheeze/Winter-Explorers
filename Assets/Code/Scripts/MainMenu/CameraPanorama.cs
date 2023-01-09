using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanorama : MonoBehaviour
{
    [SerializeField] private Vector3 rotationSpeed;                                 // Rotation speed
    [SerializeField] private float rotationMultiplier;                              // Rotation multiplier

    private void FixedUpdate() {
        transform.Rotate(rotationSpeed, rotationMultiplier * Time.deltaTime);       // Rotate camera
    }
}
