using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    private void OnCollisionEnter(Collider other) { // Execute the following code if an object collides with the player
        if (other.gameObject.CompareTag("Enemy")) // Check if the collision is with an enemy
        {
            ChangeHealth(1); // Call the change health method and remove health specified within the method 
        }
    }

    private void ChangeHealth(int amount) {
        playerHealth -= amount; // Take one point off the player's health when theyre attacked

        if (playerHealth == 0) { // Check if the player has any health left
            GameOverAnimation(); // Play the game over animation
	        playerMovement.enabled = false; // Disable player movement script (holds PlayerMovement class)
	        playerCamera.enabled = false; // Disable player camera script (holds PlayerCamera class)
	        Cursor.lockState = CursorLockMode.None; // Enable the cursor so the player can click the quit button
        }
    }
}
