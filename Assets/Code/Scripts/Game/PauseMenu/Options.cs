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
    private OptionsData optionsData = new();                                                // Call options data struct

    [Header("Animations")]                                                                  // Animations
    [SerializeField] private Animation pauseAnim;                                           // pause menu animation

    [Header("Panels")]                                                                      // Panels
    [SerializeField] private GameObject pause;                                              // Pause menu panel
    [SerializeField] private GameObject settings;                                           // Settings menu panel
    
    [Header("Settings")]                                                                    // Settings UI
    [SerializeField] private Slider brightness;                                             // Brightness setting slider
    [SerializeField] private Slider gamma;                                                  // Gamma setting slider
    [SerializeField] private Slider contrast;                                               // Contrast setting slider
    [SerializeField] private Toggle vignette;                                               // Enable/disable vignette
    [SerializeField] private Toggle bloom;                                                  // Enable/disable bloom
    [SerializeField] private Toggle antiAliasing;                                           // Enable/disable anti-aliasing
    [SerializeField] private Slider volume;                                                 // Volume setting slider

    [Header("Global Volume")]
    [SerializeField] private Volume globalVolume;                                           // Global volume setting                               

    [Header("Camera")]
    [SerializeField] private UniversalAdditionalCameraData mainCamera;                      // Anti-aliasing setting

    [Header("Brightness")]
    [SerializeField] private Image brightnessOverlay;                                       // Brightness setting

    private AudioSource clickSound;                                                         // Save button click sound effect

    private void Start() {
        clickSound = GetComponent<AudioSource>();
    }

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

        globalVolume.profile.TryGet<LiftGammaGain>(out LiftGammaGain gammaSetting);         // Gamma
        gammaSetting.gamma.value = new Vector4(1f, 1f, 1f, gamma.value);

        globalVolume.profile.TryGet<ColorAdjustments>(out ColorAdjustments contrastSetting);// Contrast
        contrastSetting.contrast.value = contrast.value;

        globalVolume.profile.TryGet<Vignette>(out Vignette vignetteSetting);                // Vignette
        vignetteSetting.active = vignette.isOn;

        globalVolume.profile.TryGet<Bloom>(out Bloom bloomSetting);                         // Bloom
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
        clickSound.Play();
        yield return new WaitForSeconds(0.20f);
        settings.SetActive(false);                                                          // Close settings menu panel
        pauseAnim.Play("Load");                                                             // Play menu deload animation
        yield return new WaitForSeconds(0.25f);
        PauseMenu.pauseState = PauseMenu.PauseState.Paused;                                 // Set state to in pause menu
    }

    public void ResetOptions() {                                                            // Reset settings
        clickSound.Play();

        brightness.value = 0f;
        gamma.value = 0.3f;
        contrast.value = 35f;
        vignette.isOn = true;
        bloom.isOn = false;
        antiAliasing.isOn = false;
        volume.value = 0.5f;
    }
}
