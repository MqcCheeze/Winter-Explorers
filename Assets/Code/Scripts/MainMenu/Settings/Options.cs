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
    public OptionsData optionsData = new OptionsData();                                     // Call options data struct

    [Header("Animations")]                                                                  // Animations
    [SerializeField] private Animation pauseAnim;                                           // pause menu animation

    [Header("Panels")]                                                                      // Panels
    [SerializeField] private GameObject pause;                                              // Pause menu panel
    [SerializeField] private GameObject settings;                                           // Settings menu panel
    
    [Header("Settings")]                                                                    // Settings UI
    public Slider brightness;                                                               // Brightness setting slider
    public Slider gamma;                                                                    // Gamma setting slider
    public Slider contrast;                                                                 // Contrast setting slider
    public Toggle vignette;                                                                 // Enable/disable vignette
    public Toggle bloom;                                                                    // Enable/disable bloom
    public Toggle antiAliasing;                                                             // Enable/disable anti-aliasing
    public Slider volume;                                                                   // Volume setting slider

    [Header("Global Volume")]
    [SerializeField] private Volume globalVolume;

    [Header("Camera")]
    [SerializeField] private UniversalAdditionalCameraData mainCamera;

    [Header("Brightness")]
    [SerializeField] private Image brightnessOverlay;
  
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

        LiftGammaGain gammaSetting;
        globalVolume.profile.TryGet<LiftGammaGain>(out gammaSetting);                       // Gamma
        gammaSetting.gamma.value = new Vector4(1f, 1f, 1f, gamma.value);

        ColorAdjustments contrastSetting;
        globalVolume.profile.TryGet<ColorAdjustments>(out contrastSetting);                 // Contrast
        contrastSetting.contrast.value = contrast.value;

        Vignette vignetteSetting;
        globalVolume.profile.TryGet<Vignette>(out vignetteSetting);                         // Vignette
        vignetteSetting.active = vignette.isOn;

        Bloom bloomSetting;
        globalVolume.profile.TryGet<Bloom>(out bloomSetting);                               // Bloom
        bloomSetting.active = bloom.isOn;

        switch (antiAliasing.isOn) {                                                        // Anti aliasing
            case true:
                mainCamera.antialiasing = AntialiasingMode.FastApproximateAntialiasing;     // Turn anti-aliasing on
                break;
            case false:
                mainCamera.antialiasing = AntialiasingMode.None;                            // Turn anti-aliasing off
                break;
        }
        AudioListener.volume = volume.value;                                                // Volume

        StartCoroutine(PauseGameAnimation());                                               // Go back to paused menu                                     
    }

    private IEnumerator PauseGameAnimation() {
        settings.SetActive(false);                                                          // Close settings menu panel
        pauseAnim.Play("Load");                                                             // Play menu deload animation
        yield return new WaitForSeconds(0.25f);
        PauseMenu.pauseState = PauseMenu.PauseState.Paused;                                 // Set state to in pause menu
    }

    public void ResetOptions() {                                                            // Reset settings
        brightness.value = 0f;
        gamma.value = 0.3f;
        contrast.value = 35f;
        vignette.isOn = true;
        bloom.isOn = false;
        antiAliasing.isOn = false;
        volume.value = 0.5f;
    }
}
