using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
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
        canMove = true;                                                 // Allow player to move
        canJump = true;                                                 // Allow player to jump
        canSprint = true;                                               // Allow player to sprint
        canSneak = true;                                                // Allow player to sneak
    }
    void Update() {

        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);   // Check if the player is touching the ground

        cannotUnsneak = Physics.Raycast(ceilingCheck.position, Vector3.up, ceilingDistance, ceilingMask);

        if (canMove) {
            Move();
        }

        if (Input.GetButtonDown("Jump") && grounded && canJump) {                           // Jump if the player is on the ground and can jump
            Jump();
        }

        if (grounded && velocity.y < 0f) {                                                  // Gravity when not in the air
            velocity.y = -2;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) {                                        // Sprint
            Sprint();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) {                                          // Sneak
            Sneak();
        }
    }

    private void Move() {                                                                   // Move
        x = Input.GetAxis("Horizontal");                                                    // Left and right
        z = Input.GetAxis("Vertical");                                                      // Forwards and backwards

        move = (transform.right * x) + (transform.forward * z);                             // Where to move to
        playerController.Move(move * speed * Time.deltaTime);                               // Move the player with the set speed
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
