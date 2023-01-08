using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LoadUniversalSettings : MonoBehaviour
{

    [Header("Settings")]                                                                    // Settings UI
    [SerializeField] private Slider brightness;                                             // Brightness setting slider
    [SerializeField] private Slider gamma;                                                  // Gamma setting slider
    [SerializeField] private Slider contrast;                                               // Contrast setting slider
    [SerializeField] private Toggle vignette;                                               // Enable/disable vignette
    [SerializeField] private Toggle bloom;                                                  // Enable/disable bloom
    [SerializeField] private Toggle antiAliasing;                                           // Enable/disable anti-aliasing
    [SerializeField] private Slider volume;                                                 // Volume setting slider

    [Header("Global Volume")]
    public Volume globalVolume;

    [Header("Camera")]
    public UniversalAdditionalCameraData mainCamera;

    [Header("Brightness")]
    public Image brightnessOverlay;

    private void Start() {

    }

    public void LoadSettings() {
        OptionsData optionsData = new OptionsData();

        Debug.Log("LoadUniversalSettings " + optionsData.volumeValueSetting);
        // -- SET SETTING MENU VALUES --
        brightness.value = optionsData.brightnessValueSetting;                                  // Set setting panel brightness
        gamma.value = optionsData.gammaValueSetting;                                            // Set setting panel gamma
        contrast.value = optionsData.contrastValueSetting;                                      // Set setting panel ontrast
        vignette.isOn = optionsData.vignetteValueSetting;                                       // Set setting panel vignette
        bloom.isOn = optionsData.bloomValueSetting;                                             // Set setting panel bloom
        antiAliasing.isOn = optionsData.antiAliasingValueSetting;                               // Set setting panel anti-aliasing
        volume.value = optionsData.volumeValueSetting;                                          // Set setting panel volume

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

        AudioListener.volume = optionsData.volumeValueSetting;                                                // Volume
    }

}
