using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class NewGame : MonoBehaviour
{
    [Header("Panels")]                                              // Panels
    [SerializeField] private GameObject mainMenu;                   // Main menu panel
    [SerializeField] private GameObject newGame;                    // New game menu panel

    [Header("Animations")]                                          // Animations
    [SerializeField] private Animation mainMenuAnim;                // Main menu animations
    [SerializeField] private Animation newGameAnim;                 // New game menu animations

    [SerializeField] private TMP_InputField seedInput;

    [SerializeField] private GameObject errorMessage;

    public void LoadNewGame() {                                     // Load a new world
        StartCoroutine(LoadNewGameAnimation());
    }

    private IEnumerator LoadNewGameAnimation() {
        mainMenuAnim.Play("Deload");                                // Play menu deload animation
        yield return new WaitForSeconds(0.25f);
        mainMenu.SetActive(false);                                  // Hide main menu panel
        newGame.SetActive(true);                                    // Show new game menu panel
        newGameAnim.Play("Load");                                   // Play new game menu load animation
    }

    public void Back() {                                            // Go back to main menu
        StartCoroutine(LoadMenuAnimation());
    }

    private IEnumerator LoadMenuAnimation() {
        newGameAnim.Play("Deload");                                 // Play new game menu deload animation
        yield return new WaitForSeconds(0.25f);
        newGame.SetActive(false);                                   // Hide new game menu panel
        mainMenu.SetActive(true);                                   // Show main menu panel
        mainMenuAnim.Play("Load");                                  // Play main menu load animation
    }

    public void CreateNewWorld() {                                  // Create a new world
        try {                                                       // Try to create a world
            GameSaveData.isNewSeed = true;                          // Say that the world is a new one
            GameSaveData.seed = Convert.ToInt32(seedInput.text);    // Set the seed to the one inputted
            SceneManager.LoadScene("Game");                         // Load game scene
        }
        catch {                                                     // If the number inputted in the seed text box exceeds the 32-bit integer limit
            errorMessage.SetActive(true);                           // Show the error message
        }
    }
}
