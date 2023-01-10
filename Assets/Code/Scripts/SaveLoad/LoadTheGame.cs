using SaveLoad;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadTheGame : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform playerTransform;                                                                 // Player transform
    [SerializeField] private CharacterController charController;                                                        // CharacterController component
    [SerializeField] private PlayerMovement playerMovement;                                                             // Player movement class

    [Header("Settings")]                                                                                                // Settings UI
    [SerializeField] private Slider brightness;                                                                         // Brightness setting slider
    [SerializeField] private Slider gamma;                                                                              // Gamma setting slider
    [SerializeField] private Slider contrast;                                                                           // Contrast setting slider
    [SerializeField] private Toggle vignette;                                                                           // Enable/disable vignette
    [SerializeField] private Toggle bloom;                                                                              // Enable/disable bloom
    [SerializeField] private Toggle antiAliasing;                                                                       // Enable/disable anti-aliasing
    [SerializeField] private Slider volume;                                                                             // Volume setting slider

    [Header("Global Volume")]                                                                                           // Settings
    public Volume globalVolume;

    [Header("Camera")]                                                                                                  // Settings
    public UniversalAdditionalCameraData mainCamera;

    [Header("Brightness")]                                                                                              // Settings
    public Image brightnessOverlay;

    

    public static bool isNewSeed;
    public static int seed;

    private PlayerData playerData = new();                                                                              // Call struct
    private OptionsData optionsData = new();                                                                            // Call struct
    private WorldData worldData = new();                                                                                // Call struct

    private void Awake() {
        Load();                                                                                                         // Load data
    }

    private void Load() {
        LoadWorldData();                                                                                                // Load the world data
        LoadSettingsData();                                                                                             // Load settings   
        LoadCharacterData();                                                                                            // Load all other saved data
    }

    private void LoadWorldData() {
        if (isNewSeed) {                                                                                                // If a new world is requested
            if (seed == 0) {                                                                                            // If the seed input box is left blank
                NoiseSettings.seed = Random.Range(0, 2147483647);                                                       // Set seed between 0 and 32 bit int limit
                NoiseSettings.offset = new Vector2(Random.Range(-100000, 100000), Random.Range(-100000, 100000));       // Set a random overall map offset
            } else {                                                                                                    // If the player has inputted a seed
                NoiseSettings.seed = seed;                                                                              // Set the seed to the seed specified
                NoiseSettings.offset = Vector2.zero;                                                                    // Set the offset to 0
            }
        } else {                                                                                                        // If the existing world is requested
            SaveGameManager.Load();                                                                                     // Load values
            worldData = SaveGameManager.currentSaveData.worldData;                                                      // Store the world data to the WorldData struct
            NoiseSettings.seed = worldData.seed;                                                                        // Set the seed to the one saved in the world data struct
            NoiseSettings.offset = worldData.offset;                                                                    // Set the offset to the one saved in the world data struct
        }
    }

    private void LoadSettingsData() {                                                                                   // Load settings
        SaveGameManager.Load();
        optionsData = SaveGameManager.currentSaveData.optionsData;                                                      // Store option data to the OptionsData struct

        // -- SET SETTING MENU VALUES --
        brightness.value = optionsData.brightnessValueSetting;                                                          // Set setting panel brightness
        gamma.value = optionsData.gammaValueSetting;                                                                    // Set setting panel gamma
        contrast.value = optionsData.contrastValueSetting;                                                              // Set setting panel ontrast
        vignette.isOn = optionsData.vignetteValueSetting;                                                               // Set setting panel vignette
        bloom.isOn = optionsData.bloomValueSetting;                                                                     // Set setting panel bloom
        antiAliasing.isOn = optionsData.antiAliasingValueSetting;                                                       // Set setting panel anti-aliasing
        volume.value = optionsData.volumeValueSetting;                                                                  // Set setting panel volume

        // -- SET SETTINGS --
        Color overlayAmount = brightnessOverlay.color;                                                                  // Brightness
        overlayAmount.a = brightness.value;
        brightnessOverlay.color = overlayAmount;

        globalVolume.profile.TryGet<LiftGammaGain>(out LiftGammaGain gammaSetting);                                     // Gamma
        gammaSetting.gamma.value = new Vector4(1f, 1f, 1f, gamma.value);

        globalVolume.profile.TryGet<ColorAdjustments>(out ColorAdjustments contrastSetting);                            // Contrast
        contrastSetting.contrast.value = contrast.value;

        globalVolume.profile.TryGet<Vignette>(out Vignette vignetteSetting);                                            // Vignette
        vignetteSetting.active = vignette.isOn;

        globalVolume.profile.TryGet<Bloom>(out Bloom bloomSetting);                                                     // Bloom
        bloomSetting.active = bloom.isOn;


        if (antiAliasing.isOn) {                                                                                        // Anti-aliasing
            mainCamera.antialiasing = AntialiasingMode.FastApproximateAntialiasing;                                     // Turn anti-aliasing on
        } else if (!antiAliasing.isOn) {
            mainCamera.antialiasing = AntialiasingMode.None;                                                            // Turn anti-aliasing off
        }

        AudioListener.volume = optionsData.volumeValueSetting;                                                          // Volume
    }


    public void LoadCharacterData() {                                                                                   // Load all other properties
        SaveGameManager.Load();                                                                                         // Load values

        charController.enabled = false;                                                                                 // Disable character controller to control player

        playerData = SaveGameManager.currentSaveData.playerData;                                                        // Store player data to the PlayerData struct

        if (isNewSeed) {
            playerTransform.SetPositionAndRotation(new Vector3(0f, 80f, 0f), Quaternion.Euler(0.0f, 0.0f, 0.0f));       // Set player pos and rot
        } else {
            playerTransform.SetPositionAndRotation(playerData.playerPosition, playerData.playerRotation);               // Set player pos and rot
        }

        if (playerData.sneaking) {                                                                                      // Sneak if the player is sneaking when saved
            playerMovement.StartSneak();
        }
        charController.enabled = true;                                                                                  // Enable character controller once finished
    }
}
