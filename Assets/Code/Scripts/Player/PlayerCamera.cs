using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Zoom Settings")]
    public bool isZoomed;                                                           // Check if player is zoomed in

    [Header("Sensitivity")]
    public float mouseSensitivity;                                                  // Look speed

    [Header("Player Body")]
    [SerializeField] private Transform playerBody;                                  // To move player model with camera

    [Header("Look Around")]
    private float xRotation;
    public bool canLook;                                                            // Lock/unlock ability to look around

    private CinemachineVirtualCamera cinemachineCamera;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;                                   // Lock cursor so the mouse doesn't move when looking around
        cinemachineCamera = GetComponent<CinemachineVirtualCamera>();
        canLook = true;                                                             // Enable/disable ability to look around
    }

    private void FixedUpdate() {
        if (Input.GetKey(KeyCode.C)) {                                              // Zoom feature
            Zoom();
        } else {
            Unzoom();
        }
    }

    void Update() {
        if (canLook) {
            LookAround();
        } 
    }

    private void LookAround() {
        float num = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;   // Horizontal
        xRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;  // Vertical
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);                              // Restrict vertical look about to feet and sky
        transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);          // Rotate camera
        playerBody.Rotate(Vector3.up * num);                                        // Rotate player body to face direction of camera
    }

    private void Zoom() {
        cinemachineCamera.m_Lens.FieldOfView = 30;                                               // Set field of view
        isZoomed = true;                                                            // Set the isZoomed to true or false
    }

    private void Unzoom() {
        cinemachineCamera.m_Lens.FieldOfView = 60;                                               // Set field of view
        isZoomed = false;                                                           // Set the isZoomed to true or false
    }
}
