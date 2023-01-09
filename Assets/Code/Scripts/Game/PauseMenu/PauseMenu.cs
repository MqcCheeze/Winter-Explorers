using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using SaveLoad;
using Unity.VisualScripting;

public class PauseMenu : MonoBehaviour
{
    public enum PauseState {                                        // Game paused/unpaused state
        Paused,                                                     // Paused
        Unpaused,                                                   // Unpaused
        InSettings
    }

    public static PauseState pauseState;                            // Call the enum PauseState

    [Header("Panels")]                                              // Panels
    [SerializeField] private GameObject pause;                      // Pause panel
    [SerializeField] private GameObject settings;                   // Settings panel
    [SerializeField] private GameObject inGameGUI;                  // In-game GUI panel

    [Header("Pause Menu Stuff")]                                    // Pause menu stuff
    [SerializeField] private PlayerCamera playerCamera;             // Camera toggle ability to look around while paused
    [SerializeField] private PlayerMovement playerMovement;         // Movement toggle ability to move around
    [SerializeField] private PlayerInventory playerInventory;       // Inventory toggle ability to interact
    

    [Header("Saving/Loading")]
    public GameSaveData gameSaveData;                               // Save the game data class

    [Header("Animations")]                                          // Animations
    [SerializeField] private Animation pauseAnim;                   // Pause menu animation

    private void Start() {
        pauseState = PauseState.Unpaused;                           // On start say the game isn't paused
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {                     // Only show pause menu if in-game
            PauseGame();                                            // Pause game
        }
    }    

    public void PauseGame() {                                       // Pause game
        switch (pauseState) {                                       // Check what the game state is
            case PauseState.Unpaused:                               // If it isn't paused
                pauseState = PauseState.Paused;                     // Pause the game
                StartCoroutine(PauseGameAnimation());               // Show pause menu

                Cursor.lockState = CursorLockMode.None;             // Unlock cursor

                playerCamera.enabled = false;                       // Disable abilities
                playerMovement.enabled = false;
                playerInventory.enabled = false;
                inGameGUI.SetActive(false);                         // Disable in-game GUI
                break;

            case PauseState.Paused:                                 // If the game is paused
                pauseState = PauseState.Unpaused;
                StartCoroutine(UnPauseGameAnimation());             // Close pause menu and continue game
                break;
        }
    }

    private IEnumerator PauseGameAnimation() {                      // Pause game
        pause.SetActive(true);                                      // Enable pause menu
        pauseAnim.Play("Load");                                     // Play pause menu load animation
        yield return new WaitForSeconds(0.25f);
    }

    private IEnumerator UnPauseGameAnimation() {                    // Unpause game
        pauseAnim.Play("Deload");                                   // Play pause menu deload animation
        yield return new WaitForSeconds(0.25f);
        pause.SetActive(false);                                     // Disable pause menu

        Cursor.lockState = CursorLockMode.Locked;                   // Lock cursor

        playerCamera.enabled = true;                                // Enable abilities
        playerMovement.enabled = true;
        playerInventory.enabled = true;
        inGameGUI.SetActive(true);                                  // Enable in-game GUI
    }

    public void QuitInGame() {                                      // Go back to main menu
        gameSaveData.SaveTheGame();
    }
}
