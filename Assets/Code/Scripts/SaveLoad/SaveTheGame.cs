using SaveLoad;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveTheGame : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform playerTransform;                                         // Player transform
    [SerializeField] private CharacterController charController;                                // CharacterController component
    [SerializeField] private PlayerMovement playerMovement;                                     // Player movement class

    [Header("Settings")]                                                                        // Settings UI
    [SerializeField] private Options options;                                                   // Set settings sliders/toggles
    [SerializeField] private Slider brightness;                                                 // Brightness setting slider
    [SerializeField] private Slider gamma;                                                      // Gamma setting slider
    [SerializeField] private Slider contrast;                                                   // Contrast setting slider
    [SerializeField] private Toggle vignette;                                                   // Enable/disable vignette
    [SerializeField] private Toggle bloom;                                                      // Enable/disable bloom
    [SerializeField] private Toggle antiAliasing;                                               // Enable/disable anti-aliasing
    [SerializeField] private Slider volume;                                                     // Volume setting slider

    [Header("Global Volume")]                                                                   // Global volume setting
    public Volume globalVolume;

    [Header("Camera")]                                                                          // Anti-aliasing setting
    public UniversalAdditionalCameraData mainCamera;

    [Header("Brightness")]                                                                      // Brightness setting
    public Image brightnessOverlay;

    private PlayerData playerData = new();                                                      // Call player data struct
    private OptionsData optionsData = new();                                                    // Call options data struct
    private WorldData worldData = new();                                                        // Call world data struct

    public void Save() {
                    // -- PLAYER --
        playerData.playerPosition = playerTransform.position;                                   // Saving position
        playerData.playerRotation = playerTransform.rotation;                                   // Saving rotation
        playerData.sneaking = playerMovement.isSneaking;                                        // Saving sneak

                    // -- OPTIONS -- 
        optionsData.brightnessValueSetting = brightness.value;                                  // Save all options to the options data struct
        optionsData.gammaValueSetting = gamma.value;
        optionsData.contrastValueSetting = contrast.value;
        optionsData.vignetteValueSetting = vignette.isOn;
        optionsData.bloomValueSetting = bloom.isOn;
        optionsData.antiAliasingValueSetting = antiAliasing.isOn;
        optionsData.volumeValueSetting = volume.value;

                    // -- WORLD --
        worldData.seed = NoiseSettings.seed;
        worldData.offset = NoiseSettings.offset;

        SaveGameManager.currentSaveData.playerData = playerData;                                // Set the player save data to variable
        SaveGameManager.currentSaveData.optionsData = optionsData;                              // Set the options save data to variable
        SaveGameManager.currentSaveData.worldData = worldData;                                  // Set the world save data to variable

        SaveGameManager.Save();                                                                 // Save values
        StartCoroutine(LoadMenu());
    }
    public IEnumerator LoadMenu() {

        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("MainMenu");                                                     // Load main menu scene
    }
}
