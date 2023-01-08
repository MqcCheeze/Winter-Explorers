using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using SaveLoad;
using Unity.VisualScripting;

public class Buttons : MonoBehaviour
{
    
    [Header("Panels")]                          // Panels
    public GameObject menu;                     // Main menu panel
    public GameObject settings;                 // Settings panel
    public GameObject credits;                  // Credits panel
    public GameObject newGame;                  // New game panel
    public GameObject pause;                    // Pause panel
    public GameObject crosshair;                // Crosshair
    public GameObject inventory;                // Inventory
    public GameObject hints;                    // Hints

    [Header("Pause Menu Stuff")]                // Pause menu stuff
    [SerializeField] private bool isPaused;     // Check if the pause menu is open
    public static bool isInSettings;            // Check if in the settings menu
    public PlayerCamera playerCamera;           // Camera toggle ability to look around while paused
    public PlayerMovement playerMovement;       // Movement toggle ability to move around
    public PlayerInventory playerInventory;     // Inventory toggle ability to interact
    

    [Header("Saving/Loading")]
    public GameSaveData gameSaveData;

    [Header("Animations")]
    public Animation menuAnim;
    public Animation newGameAnim;
    public Animation creditsAnim;
    public Animation pauseAnim;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name == "Game") {     // Only show pause menu if in-game
            PauseGame();                                                                            // Pause game
        }
    }

                // -- MAIN MENU SPECIFIC BUTTON FUNCTIONS --

                // -- NEW GAME --
    public void LoadNewGame() {                 // Load a new world
        StartCoroutine(LoadNewGameAnimation());
    }

    private IEnumerator LoadNewGameAnimation() {
        menuAnim.Play("Deload");                // Play menu deload animation
        yield return new WaitForSeconds(0.25f);
        menu.SetActive(false);                  // Hide menu panel
        newGame.SetActive(true);                // Show new game panel
        newGameAnim.Play("Load");               // Play new game load animation
    }

    // -- LOAD GAME --
    public void LoadSavedGame() {               // Load existing world
        StartCoroutine(LoadSavedGameAnimation());
    }

    private IEnumerator LoadSavedGameAnimation() {
        menuAnim.Play("Deload");
        yield return new WaitForSeconds(0.3f);
        GameSaveData.isNewSeed = false;         // Say that the world isn't a new one
        SceneManager.LoadScene("Game");         // Load game scene
    }
                
                // -- OPTIONS --
    public void LoadOptions() {
        menu.SetActive(false);                  // Hide menu panel
        settings.SetActive(true);               // Show settings panel
    }
                // -- CREDITS --
    public void LoadCredits() {
        StartCoroutine(LoadCreditsAnimation());
    }

    private IEnumerator LoadCreditsAnimation() {
        menuAnim.Play("Deload");                // Play menu deload animation
        yield return new WaitForSeconds(0.25f);
        menu.SetActive(false);                  // Hide menu panel
        credits.SetActive(true);                // Show credits panel
        creditsAnim.Play("Load");               // Play credits load animation
    }

                // -- QUIT --
    public void QuitGame() {
        StartCoroutine(QuitGameAnimation());
    }

    private IEnumerator QuitGameAnimation() {
        menuAnim.Play("Deload");                // Play menu deload animation
        yield return new WaitForSeconds(0.3f);
        Application.Quit();                     // Close program
    }



    // -- IN-GAME SPECIFIC BUTTON FUNCTIONS --
    public void PauseGame() {
        if (!isPaused) {                                // If the game isn't paused
            isPaused = true;                            // Indicate game is paused
            StartCoroutine(PauseGameAnimation());       // Show pause menu
            
            Cursor.lockState = CursorLockMode.None;     // Unlock cursor

            playerCamera.enabled = false;               // Disable abilities
            playerMovement.enabled = false;
            playerInventory.enabled = false;
            crosshair.SetActive(false);
            inventory.SetActive(false);
            hints.SetActive(false);

        } else if (isPaused && !isInSettings) {         // If the game is paused
            isPaused = false;                           // Indicate game isn't paused
            StartCoroutine(UnPauseGameAnimation());     // Close pause menu and continue game
            
            Cursor.lockState = CursorLockMode.Locked;   // Lock cursor

            playerCamera.enabled = true;                // Enable abilities
            playerMovement.enabled = true;
            playerInventory.enabled = true;
            crosshair.SetActive(true);
            inventory.SetActive(true);
            hints.SetActive(true);

        }
    }

    private IEnumerator PauseGameAnimation() {
        pause.SetActive(true);                          // Open pause menu
        pauseAnim.Play("Load");                         // Play menu deload animation
        yield return new WaitForSeconds(0.25f);
    }

    private IEnumerator UnPauseGameAnimation() {
        pauseAnim.Play("Deload");                       // Play menu deload animation
        yield return new WaitForSeconds(0.25f);
        pause.SetActive(false);                         // Open pause menu
    }

    public void LoadOptionsInGame() {                   // Open options menu
        pause.SetActive(false);                         // Close pause panel
        settings.SetActive(true);                       // Open settings panel
        isInSettings = true;
    }

    public void QuitInGame() {                          // Go back to main menu
        gameSaveData.SaveTheGame();
    }
}
