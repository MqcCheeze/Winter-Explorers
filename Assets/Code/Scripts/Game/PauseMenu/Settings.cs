using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [Header("Panels")]                                              // Panels
    [SerializeField] private GameObject settings;                   // Settings panel

    [Header("Animations")]                                          // Animations
    [SerializeField] private Animation pauseAnim;                   // Pause menu animation

    public void LoadSettings() {                                        // Open settings menu
        StartCoroutine(LoadSettingsAnimation());
    }

    private IEnumerator LoadSettingsAnimation() {                       // Load settings
        pauseAnim.Play("Deload");                                       // Play pause menu deload animation
        yield return new WaitForSeconds(0.25f);
        settings.SetActive(true);                                       // Enable settings menu
        PauseMenu.pauseState = PauseMenu.PauseState.InSettings;         // Set state to in settings
    }
}
