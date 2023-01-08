using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class NewGame : MonoBehaviour
{
    [Header("Panels")]                          // Panels
    public GameObject menu;                     // Main menu panel
    public GameObject newGame;                  // New game panel

    [Header("Animations")]
    public Animation menuAnim;
    public Animation newGameAnim;

    public TMP_InputField seedInput;

    public GameObject errorMessage;

    public void Back() {
        StartCoroutine(LoadMenuAnimation());
    }

    private IEnumerator LoadMenuAnimation() {
        newGameAnim.Play("Deload");                 // Play new game deload animation
        yield return new WaitForSeconds(0.25f);
        newGame.SetActive(false);                   // Hide new game panel
        menu.SetActive(true);                       // Show menu panel
        menuAnim.Play("Load");                      // Play menu load animation
    }

    public void CreateNewWorld() {
        try {
            GameSaveData.isNewSeed = true;                          // Say that the world is a new one
            GameSaveData.seed = Convert.ToInt32(seedInput.text);    // Set the seed to the one inputted
            SceneManager.LoadScene("Game");                         // Load game scene
        }
        catch {
            errorMessage.SetActive(true);
        }
    }
}
