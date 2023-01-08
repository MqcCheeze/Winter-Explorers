using SaveLoad;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSaveData : MonoBehaviour
{
                // -- PLAYER --
    [Header("Player")]
    private Transform playerTransform;                                                      // Player transform
    private CharacterController charController;                                             // CharacterController component
    private PlayerMovement playerMovement;                                                  // Player movement class
    private PlayerInventory playerInventory;

    PlayerData playerData = new PlayerData();                                               // Call struct
    OptionsData optionsData = new OptionsData();                                            // Call struct
    WorldData worldData = new WorldData();                                                  // Call struct
                
                // -- OPTIONS --
    [Header("Settings")]                                                                    // Settings UI
    [SerializeField] private Slider brightness;                                             // Brightness setting slider
    [SerializeField] private Slider gamma;                                                  // Gamma setting slider
    [SerializeField] private Slider contrast;                                               // Contrast setting slider
    [SerializeField] private Toggle vignette;                                               // Enable/disable vignette
    [SerializeField] private Toggle bloom;                                                  // Enable/disable bloom
    [SerializeField] private Toggle antiAliasing;                                           // Enable/disable anti-aliasing
    [SerializeField] private Slider volume;                                                 // Volume setting slider

    [Header("Global Volume")]                                                               // Settings
    public Volume globalVolume;

    [Header("Camera")]                                                                      // Settings
    public UniversalAdditionalCameraData mainCamera;

    [Header("Brightness")]                                                                  // Settings
    public Image brightnessOverlay;


    public Options options;                                                                 // Set settings sliders/toggles

    public static bool isNewSeed;
    public static int seed;

    private void Awake() {
        playerTransform = GetComponent<Transform>();                                        // Player transform
        charController = GetComponent<CharacterController>();                               // Enable/disable characater controller while loading game
        playerMovement = GetComponent<PlayerMovement>();                                    // Save and load abilities
        playerInventory = GetComponent<PlayerInventory>();
        LoadWorldSettings();                                                                // Load the world data
        StartCoroutine(LoadTheGame());                                                      // Load all other saved data
    }

                // -- SAVING AND GOING TO MENU --
    public void SaveTheGame() {
                    // -- PLAYER --
        playerData.playerPosition = playerTransform.position;                               // Saving position
        playerData.playerRotation = playerTransform.rotation;                               // Saving rotation
        playerData.sneaking = playerMovement.isSneaking;                                    // Saving sneak

                    // -- OPTIONS --
        optionsData.brightnessValueSetting = options.brightness.value;                      // Set the option slider and toggle values
        optionsData.gammaValueSetting = options.gamma.value;
        optionsData.contrastValueSetting = options.contrast.value;
        optionsData.vignetteValueSetting = options.vignette.isOn;
        optionsData.bloomValueSetting = options.bloom.isOn;
        optionsData.antiAliasingValueSetting = options.antiAliasing.isOn;
        optionsData.volumeValueSetting = options.volume.value;

                    // -- WORLD --
        worldData.seed = NoiseSettings.seed;
        worldData.offset = NoiseSettings.offset;

        SaveGameManager.currentSaveData.playerData = playerData;                            // Set the player save data to variable
        SaveGameManager.currentSaveData.optionsData = optionsData;                          // Set the options save data to variable
        SaveGameManager.currentSaveData.worldData = worldData;                              // Set the world save data to variable

        SaveGameManager.Save();                                                             // Save values
        StartCoroutine(LoadMenu());
    }
    public IEnumerator LoadMenu() {
        
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("MainMenu");                                                 // Load main menu scene
    }

                // -- LOAD AND START GAME --
    public void LoadSettings() {                                                            // Load settings

                    // -- SET SETTING MENU VALUES --
        brightness.value = optionsData.brightnessValueSetting;                              // Set setting panel brightness
        gamma.value = optionsData.gammaValueSetting;                                        // Set setting panel gamma
        contrast.value = optionsData.contrastValueSetting;                                  // Set setting panel ontrast
        vignette.isOn = optionsData.vignetteValueSetting;                                   // Set setting panel vignette
        bloom.isOn = optionsData.bloomValueSetting;                                         // Set setting panel bloom
        antiAliasing.isOn = optionsData.antiAliasingValueSetting;                           // Set setting panel anti-aliasing
        volume.value = optionsData.volumeValueSetting;                                      // Set setting panel volume

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

        AudioListener.volume = optionsData.volumeValueSetting;                              // Volume
    }

    public void LoadWorldSettings() {
        SaveGameManager.Load();                                                                                         // Load values

        if (isNewSeed) {                                                                                                // If a new world is requested
            if (seed == 0) {                                                                                            // If the seed input box is left blank
                NoiseSettings.seed = Random.Range(0, 2147483647);                                                       // Set seed between 0 and 32 bit int limit
                NoiseSettings.offset = new Vector2(Random.Range(-100000, 100000), Random.Range(-100000, 100000));       // Set a random overall map offset
            } else {                                                                                                    // If the player has inputted a seed
                NoiseSettings.seed = seed;                                                                              // Set the seed to the seed specified
                NoiseSettings.offset = Vector2.zero;                                                                    // Set the offset to 0
            }
        } else {                                                                                                        // If the existing world is requested
            worldData = SaveGameManager.currentSaveData.worldData;                                                      // Store the world data to the WorldData struct
            NoiseSettings.seed = worldData.seed;                                                                        // Set the seed to the one saved in the world data struct
            NoiseSettings.offset = worldData.offset;                                                                    // Set the offset to the one saved in the world data struct
        }
    }

    public IEnumerator LoadTheGame() {                                      // Load all other properties
        SaveGameManager.Load();                                             // Load values

        yield return new WaitForSeconds(0.0001f);

        charController.enabled = false;                                     // Disable character controller to control player

        playerData = SaveGameManager.currentSaveData.playerData;            // Store player data to the PlayerData struct
        optionsData = SaveGameManager.currentSaveData.optionsData;          // Store option data to the OptionsData struct
        
        if (isNewSeed) {
            playerTransform.position = new Vector3(0f, 80f, 0f);            // Set players position
            playerTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);     // Set players rotation
        } else {
            playerTransform.position = playerData.playerPosition;           // Set players position
            playerTransform.rotation = playerData.playerRotation;           // Set players rotation
        }

        if (playerData.sneaking) {                                          // Sneak if the player is sneaking when saved
            playerMovement.StartSneak();
        }

        LoadSettings();                                                     // Load settings    

        charController.enabled = true;                                      // Enable character controller once finished
 
    }
}

            // -- DATA TO STORE --
[System.Serializable]
public struct PlayerData {                                                  // Player data to save

                // -- PLAYER --
    public Vector3 playerPosition;                                          // Player's position as of saving
    public Quaternion playerRotation;                                       // Player's rotation as of saving
    public bool sneaking;                                                   // Player's sneak state as of saving
}

[System.Serializable]
public struct OptionsData {                                                 // Options data to save

                // -- OPTIONS --
    public float brightnessValueSetting;                                    // Brightness
    public float gammaValueSetting;                                         // Gamma
    public float contrastValueSetting;                                      // Contrast
    public bool vignetteValueSetting;                                       // Vignette
    public bool bloomValueSetting;                                          // Bloom
    public bool antiAliasingValueSetting;                                   // Anti-aliasing
    public float volumeValueSetting;                                        // Volume
}

[System.Serializable]
public struct WorldData {                                                   // World data to save

                // -- WORLD --
    public int seed;                                                        // The world seed
    public Vector2 offset;                                                  // The world seed offset
}