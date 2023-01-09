using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Continue : MonoBehaviour
{
    [Header("Panels")]                                              // Panels
    [SerializeField] private GameObject pause;                      // Pause panel
    [SerializeField] private GameObject inGameGUI;                  // In-game GUI panel

    [Header("Pause Menu Stuff")]                                    // Pause menu stuff
    [SerializeField] private PlayerCamera playerCamera;             // Camera toggle ability to look around while paused
    [SerializeField] private PlayerMovement playerMovement;         // Movement toggle ability to move around
    [SerializeField] private PlayerInventory playerInventory;       // Inventory toggle ability to interact

    [Header("Animations")]                                          // Animations
    [SerializeField] private Animation pauseAnim;                   // Pause menu animation

    public void ContinueGame() {
        PauseMenu.pauseState = PauseMenu.PauseState.Unpaused;       // Set the game's state to unpaused
        StartCoroutine(ContinueGameAnimation());

        Cursor.lockState = CursorLockMode.Locked;                   // Lock cursor

        playerCamera.enabled = true;                                // Enable abilities
        playerMovement.enabled = true;
        playerInventory.enabled = true;
        inGameGUI.SetActive(true);                                  // Enable in-game GUI
    }

    private IEnumerator ContinueGameAnimation() {
        pauseAnim.Play("Deload");                                   // Play pause menu deload animation
        yield return new WaitForSeconds(0.25f);
        pause.SetActive(false);                                     // Disable pause menu
    }
}
