using UnityEngine;

public static class HeightMapGenerator {

    static float[,] fallOffMap;
    public static HeightMap GenerateHeightMap(int width, int height, HeightMapSettings settings, Vector2 sampleCentre) {    // Takes in the width and height of the mesh, the settings and sample center as usable variables
        float[,] values = Noise.GenerateNoiseMap(width, height, settings.noiseSettings, sampleCentre);                      // Get the height of points in the NoiseMap

        AnimationCurve heightCurve_threadsafe = new AnimationCurve(settings.heightCurve.keys);                              // Height curve of the terrain

        float minValue = float.MaxValue;                                                                                    // Set the minimum height
        float maxValue = float.MinValue;                                                                                    // Set the maximum height

        if (settings.useFallOff) {                                                                                          // If using falloff generation setting
            if (fallOffMap == null) {
                fallOffMap = FallOffGenerator.GenerateFallOffMap(width);                                                    // Generate a falloff map with the width of the mesh 
            }
        }

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                values[i, j] *= heightCurve_threadsafe.Evaluate(values[i, j] - (settings.useFallOff ? fallOffMap[i, j] : 0)) * settings.heightMultiplier;   // If the falloff map is used, subtract the height of the falloff map

                if (values[i, j] > maxValue) {                                                                              // If the height exceeds the current maximum, update the new highest height
                    maxValue = values[i, j];

                }
                if (values[i, j] < minValue) {                                                                              // If the height is below the current minimum, update the new lowest height
                    minValue = values[i, j];

                }

            }
        }
        return new HeightMap(values, minValue, maxValue);                                                                   // Return the array of heights of points in the map

    }
}

public struct HeightMap {
    public readonly float[,] values;
    public readonly float minValue;
    public readonly float maxValue;

    public HeightMap(float[,] values, float minValue, float maxValue) {
        this.values = values;
        this.minValue = minValue;                                                                                           // Set the minimum height
        this.maxValue = maxValue;                                                                                           // Set the maximum height

    }
}