using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HeightMapSettings : UpdatableData {

    public NoiseSettings noiseSettings;                                 // Access the variables from class NoiseSettings

    public bool useFallOff;                                             // Enable/disable falloff option

    public float heightMultiplier;                                      // How high the 
    public AnimationCurve heightCurve;                                  // Manipulate the height of the terrain

    public float minHeight {
        get {
            return heightMultiplier * heightCurve.Evaluate(0);          // Minimum height of the terrain
        }
    }

    public float maxHeight {
        get {
            return heightMultiplier * heightCurve.Evaluate(1);          // Max height of the terrain
        }
    }

#if UNITY_EDITOR

    protected override void OnValidate() {
        noiseSettings.ValidateValues();
        base.OnValidate();

    }

    #endif
}
