using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private EventSystem eventSystem;

    [Header("Player")]                                                  // Player
    [SerializeField] private CharacterController playerController;      // Player controller
    [SerializeField] private Transform playerBody;                      // Player body
    [SerializeField] private Transform playerModel;                     // Player model
    [SerializeField] private Transform playerCamera;                    // Player camera
    [SerializeField] private Transform playerHand;                      // Player hand


    [Header("Player Abilities")]                                        // Player abilities
    [SerializeField] private bool canMove;                              // Enable/disable move ability
    [SerializeField] private bool canJump;                              // Enable/disable jump ability
    [SerializeField] private bool canSprint;                            // Enable/disable sprint ability
    [SerializeField] private bool canSneak;                             // Enable/disable sneak ability
    [SerializeField] private bool cannotUnsneak;                        // Enable/disable unsneak ability

    [Header("Player Speed")]                                            // Player movement
    [SerializeField] private float speed;                               // Speed
    [SerializeField] private float sneakSpeed;                          // Sneaking speed
    [SerializeField] private float walkSpeed;                           // Walking speed
    [SerializeField] private float sprintSpeed;                         // Running speed

    [SerializeField] private float gravity;                             // Player's gravity
    [SerializeField] private float jumpHeight;                          // Player's jump height

    [Header("Movement")]                                                // Movement
    [SerializeField] private Vector3 velocity;                          // Player velocity
    private Vector3 move;
    private float x;                                                    // Left and right
    private float z;                                                    // Forwards and backwards
    [SerializeField] private CinemachineVirtualCamera cinemachineVC;    // Main camera shake when moving
    private CinemachineBasicMultiChannelPerlin cinemachineBMCP;
    private float notMovingAmplitude = 0.5f;
    private float movingAmplitude = 2;

    [Header("Grounded")]                                                // Is player on the ground?
    [SerializeField] private Transform groundCheck;                     // Object to check ground with

    [SerializeField] private float groundDistance;                      // Ground distance
    [SerializeField] private LayerMask groundMask;                      // Ground layer
    [SerializeField] private bool grounded;                             // True/false is player touching ground

    [Header("Sprint")]                                                  // Sprint
    public bool isSprinting;                                            // If the player is sprinting

    [Header("Sneak")]                                                   // Sneak
    public bool isSneaking;                                             // If the player is sneaking

    [SerializeField] private Transform ceilingCheck;                    // Check if the player has enough room to unsneak
    [SerializeField] private float ceilingDistance;                     // Check the distance
    [SerializeField] private LayerMask ceilingMask;                     // Check the objects it should be looking for

    void Start() {
        eventSystem.PlayerInteractions += EventSystem_PlayerInteractions;

        canMove = true;                                                 // Allow player to move
        canJump = true;                                                 // Allow player to jump
        canSprint = true;                                               // Allow player to sprint
        canSneak = true;                                                // Allow player to sneak
        cinemachineBMCP = cinemachineVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void FixedUpdate() {
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);   // Check if the player is touching the ground

        if (isSneaking) {
            cannotUnsneak = Physics.Raycast(ceilingCheck.position, Vector3.up, ceilingDistance, ceilingMask);
        }

        if (grounded && velocity.y < 0f) {                                                  // Gravity when not in the air
            velocity.y = -2;
        }

    }

    void Update() {
        if (canMove) {
            Move();
        }
    }

    private void EventSystem_PlayerInteractions(object sender, EventSystem.KeyPressed e) {
        switch (e.keyPressed) {
            case "lShift":
                Sneak();
                break;
            case "lControl":
                Sprint();
                break;
            case "space":
                if (grounded && canJump) {
                    Jump();
                }
                break;
        }
    }

    private void Move() {                                                                   // Move
        x = Input.GetAxis("Horizontal");                                                    // Left and right
        z = Input.GetAxis("Vertical");                                                      // Forwards and backwards

        move = (transform.right * x) + (transform.forward * z);                             // Where to move to
        if (x > 0 || z > 0) {
            cinemachineBMCP.m_AmplitudeGain = movingAmplitude;
            cinemachineBMCP.m_FrequencyGain = movingAmplitude;
        } else {
            cinemachineBMCP.m_AmplitudeGain = notMovingAmplitude;
            cinemachineBMCP.m_FrequencyGain = notMovingAmplitude;
        }
        playerController.Move(speed * Time.deltaTime * move);                               // Move the player with the set speed
                                                                                            // Time.deltaTime so player moves at the same speed on all hardware

        velocity.y += gravity * Time.deltaTime;                                             // Gravity
        playerController.Move(velocity * Time.deltaTime);
    }

    private void Jump() {                                                                   // Jump
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);                                // Calculate the vertical speed
    }

    private void Sprint() {                                                                 // Sprint
        if (canSprint) {                                                                    // Check if the player is allowed to sprint
            if (!isSprinting) {
                isSprinting = true;
                speed = sprintSpeed;                                                        // Set player speed to sprinting speed
            } else if (isSprinting) {
                isSprinting = false;
                speed = walkSpeed;                                                          // Set player speed to walking speed
            }
        }
    }

    public void Sneak() {                                                                   // Sneak
        if (canSneak) {
            if (!isSneaking) {
                StartSneak();
            } else if (isSneaking && !cannotUnsneak) {
                UnSneak();
            }
        }
    }

    public void StartSneak() {                                                              // Start sneaking
        isSneaking = true;
        canJump = false;                                                                    // Disable jump ability
        canSprint = false;                                                                  // Disable sprint ability

        speed = sneakSpeed;                                                                 // Set player speed to sneaking speed
        playerModel.localScale = new Vector3(1f, 0.5f, 1f);
        playerController.height = 1f;
        playerCamera.localPosition = new Vector3(0f, playerCamera.localPosition.y - 0.5f, 0f);
        groundCheck.localPosition = new Vector3(0f, groundCheck.localPosition.y + 0.5f, 0f);
        ceilingCheck.localPosition = new Vector3(0f, ceilingCheck.localPosition.y - 0.5f, 0f);
    }

    public void UnSneak() {                                                                 // Stop sneaking
        isSneaking = false;
        canJump = true;                                                                     // Enable jump ability
        canSprint = true;                                                                   // Enable sprint ability

        speed = walkSpeed;                                                                  // Set player speed to walking speed
        playerModel.localScale = new Vector3(1f, 1f, 1f);
        playerController.height = 2f;
        playerCamera.localPosition = new Vector3(0f, playerCamera.localPosition.y + 0.5f, 0f);
        groundCheck.localPosition = new Vector3(0f, groundCheck.localPosition.y - 0.5f, 0f);
        ceilingCheck.localPosition = new Vector3(0f, ceilingCheck.localPosition.y + 0.5f, 0f);
    }
}
