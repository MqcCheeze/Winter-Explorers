using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public OptionsData optionsData = new OptionsData();

    public Animation pauseAnim;

    [Header("Panels")]                                                                      // Panels
    public GameObject menu;                                                                 // Main menu panel
    public GameObject pause;                                                                // Pause menu panel
    public GameObject settings;                                                             // Settings menu panel

    
    [Header("Settings")]                                                                    // Settings UI
    public Slider brightness;                                                               // Brightness setting slider
    public Slider gamma;                                                                    // Gamma setting slider
    public Slider contrast;                                                                 // Contrast setting slider
    public Toggle vignette;                                                                 // Enable/disable vignette
    public Toggle bloom;                                                                    // Enable/disable bloom
    public Toggle antiAliasing;                                                             // Enable/disable anti-aliasing
    public Slider volume;                                                                   // Volume setting slider

    [Header("Global Volume")]
    public Volume globalVolume;

    [Header("Camera")]
    public UniversalAdditionalCameraData mainCamera;

    [Header("Brightness")]
    public Image brightnessOverlay;
  
    public void SaveSettings() {                                                            // Set changed values to universal settings

        optionsData.brightnessValueSetting = brightness.value;
        optionsData.gammaValueSetting = gamma.value;
        optionsData.contrastValueSetting = contrast.value;
        optionsData.vignetteValueSetting = vignette.isOn;
        optionsData.bloomValueSetting = bloom.isOn;
        optionsData.antiAliasingValueSetting = antiAliasing.isOn;
        optionsData.volumeValueSetting = volume.value;

        // -- SET SETTINGS --
        Color overlayAmount = brightnessOverlay.color;                                      // Brightness
        overlayAmount.a = brightness.value;
        brightnessOverlay.color = overlayAmount;

        
        LiftGammaGain gammaSetting;                                                         // Gamma
        globalVolume.profile.TryGet<LiftGammaGain>(out gammaSetting);
        gammaSetting.gamma.value = new Vector4(1f, 1f, 1f, gamma.value);

        
        ColorAdjustments contrastSetting;                                                   // Contrast
        globalVolume.profile.TryGet<ColorAdjustments>(out contrastSetting);
        contrastSetting.contrast.value = contrast.value;

        
        Vignette vignetteSetting;                                                           // Vignette
        globalVolume.profile.TryGet<Vignette>(out vignetteSetting);
        vignetteSetting.active = vignette.isOn;

        
        Bloom bloomSetting;                                                                 // Bloom
        globalVolume.profile.TryGet<Bloom>(out bloomSetting);
        bloomSetting.active = bloom.isOn;

        
        if (antiAliasing.isOn) {                                                            // Anti-aliasing
            mainCamera.antialiasing = AntialiasingMode.FastApproximateAntialiasing;         // Turn anti-aliasing on
        } else if (!antiAliasing.isOn) {
            mainCamera.antialiasing = AntialiasingMode.None;                                // Turn anti-aliasing off
        }

        
        AudioListener.volume = volume.value;                                                // Volume

                    // -- GO BACK TO MENU --
        if (SceneManager.GetActiveScene().name == "MainMenu") {                             // If in title screen
            menu.SetActive(true);
        } else if (SceneManager.GetActiveScene().name == "Game") {                          // If in-game
            Buttons.isInSettings = false;
            settings.SetActive(false);                      // Close settings menu panel
            pause.SetActive(true);                          // Open pause menu
            pauseAnim.Play("Load");                         // Play menu deload animation
            //StartCoroutine(PauseGameAnimation());
           
        }                                                     
    }

    public void RevertOptions() {                                                           // Revert settings
        brightness.value = optionsData.brightnessValueSetting;
        gamma.value = optionsData.gammaValueSetting;
        contrast.value = optionsData.contrastValueSetting;
        vignette.isOn = optionsData.vignetteValueSetting;
        bloom.isOn = optionsData.bloomValueSetting;
        antiAliasing.isOn = optionsData.antiAliasingValueSetting;
        volume.value = optionsData.volumeValueSetting;
    }

    private IEnumerator PauseGameAnimation() {
        settings.SetActive(false);                      // Close settings menu panel
        pause.SetActive(true);                          // Open pause menu
        pauseAnim.Play("Load");                         // Play menu deload animation
        yield return new WaitForSeconds(0.25f);
        
    }
}
