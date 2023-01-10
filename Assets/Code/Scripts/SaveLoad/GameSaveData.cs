using UnityEngine;

            // -- DATA TO STORE --
[System.Serializable]
public struct PlayerData {                                                  // Player data to save
    public Vector3 playerPosition;                                          // Player's position as of saving
    public Quaternion playerRotation;                                       // Player's rotation as of saving
    public bool sneaking;                                                   // Player's sneak state as of saving
}

[System.Serializable]
public struct OptionsData {                                                 // Options data to save
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
    public int seed;                                                        // The world seed
    public Vector2 offset;                                                  // The world seed offset
}