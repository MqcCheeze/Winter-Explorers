using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [Header("Player")]                              // Player
    public CharacterController playerController;    // Player controller
    public Transform playerBody;                    // Player body
    public Transform playerModel;                   // Player model
    public Transform playerCamera;                  // Player camera
    public Transform playerHand;                    // Player hand


    [Header("Player Abilities")]                    // Player abilities
    public bool canMove;                            // Enable/disable move ability
    public bool canJump;                            // Enable/disable jump ability
    public bool canSprint;                          // Enable/disable sprint ability
    public bool canSneak;                           // Enable/disable sneak ability
    public bool cannotUnsneak;                      // Enable/disable unsneak ability

    [Header("Player Speed")]                        // Player movement
    public float speed;                             // Speed
    public float sneakSpeed;                        // Sneaking speed
    public float walkSpeed;                         // Walking speed
    public float sprintSpeed;                       // Running speed

    public float sneakFOV;
    public float walkFOV;
    public float sprintFOV;

    public float gravity;                           // Player's gravity
    public float jumpHeight;                        // Player's jump height

    [Header("Movement")]                            // Movement
    public Vector3 velocity;                        // Player velocity
    public Vector3 move;
    public float x;                                 // Left and right
    public float z;                                 // Forwards and backwards

    [Header("Grounded")]                            // Is player on the ground?
    public Transform groundCheck;                   // Object to check ground with

    public float groundDistance;                    // Ground distance
    public LayerMask groundMask;                    // Ground layer
    public bool grounded;                           // True/false is player touching ground

    [Header("Sprint")]                              // Sprint
    public bool isSprinting;                        // If the player is sprinting

    [Header("Sneak")]                               // Sneak
    public bool isSneaking;                         // If the player is sneaking

    public Transform ceilingCheck;                  // Check if the player has enough room to unsneak
    public float ceilingDistance;                   // Check the distance
    public LayerMask ceilingMask;                   // Check the objects it should be looking for

    void Start() {
        canMove = true;                             // Allow player to move
        canJump = true;                             // Allow player to jump
        canSprint = true;                           // Allow player to sprint
        canSneak = true;                            // Allow player to sneak
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

    public void Sneak() {                                                                  // Sneak
        if (canSneak) {
            if (!isSneaking) {
                StartSneak();
            } else if (isSneaking && !cannotUnsneak) {
                UnSneak();
            }
        }
    }

    public void StartSneak() {                                                             // Start sneaking
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

    public void UnSneak() {                                                                // Stop sneaking
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
