using UnityEngine;
using System.Collections;

public static class Noise {

	public enum NormalizeMode {Local, Global};																				// Different modes: Local | Global

	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, NoiseSettings settings, Vector2 sampleCentre) {	// Method takes the width and height with the noise settings (e.g. seed etc...) and the sample centre
		float[,] noiseMap = new float[mapWidth,mapHeight];

		System.Random prng = new System.Random(NoiseSettings.seed);
		Vector2[] octaveOffsets = new Vector2[settings.octaves];

		float maxPossibleHeight = 0;
        float amplitude = 1;
        float frequency = 1;

        for (int i = 0; i < settings.octaves; i++) {																		// For every octave 
			float offsetX = prng.Next(-100000, 100000) + NoiseSettings.offset.x + sampleCentre.x;							// Randomise the offset's x
			float offsetY = prng.Next(-100000, 100000) - NoiseSettings.offset.y - sampleCentre.y;							// Randomise the offset's y
			octaveOffsets[i] = new Vector2(offsetX, offsetY);																// Store the offset in a vector 2 in the octave offset array for later use below

			maxPossibleHeight += amplitude;
			amplitude *= settings.persistance;
		}

		float maxLocalNoiseHeight = float.MinValue;
		float minLocalNoiseHeight = float.MaxValue;

		float halfWidth = mapWidth / 2f;																					// Find half the width of the map mesh size
		float halfHeight = mapHeight / 2f;																					// Find half the height of the map mesh size


		for (int y = 0; y < mapHeight; y++) {																				// For every point in the map
			for (int x = 0; x < mapWidth; x++) {
				amplitude = 1;
				frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < settings.octaves; i++) {																// For every octave
                    float sampleX = (x - halfWidth + octaveOffsets[i].x) / settings.scale * frequency;						// Sample the x value at the point in the octave i
                    float sampleY = (y - halfHeight + octaveOffsets[i].y) / settings.scale * frequency;						// Sample the y value at the point in the octave i

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;										// Get the perlin noise value at the position
					noiseHeight += perlinValue * amplitude;																	// Set the noise height to the perlin value multiplied by the amplitude of the terrain

					amplitude *= settings.persistance;
					frequency *= settings.lacunarity;
				}

				if (noiseHeight > maxLocalNoiseHeight) {																	// If the noise height is above the maximum height
					maxLocalNoiseHeight = noiseHeight;																		// Then update the maximum height to the new value

				}
				if (noiseHeight < minLocalNoiseHeight) {																	// If the noise height is below the minimum height
                    minLocalNoiseHeight = noiseHeight;																		// Then update the minimum height to the new value

                }
				noiseMap[x, y] = noiseHeight;																				// Set the x and y values of the noiseMap to the noise height created

				if (settings.normalizeMode == NormalizeMode.Global) {														// If the normalize mode is Global
                    float normalizedHeight = (noiseMap[x, y] + 1) / (maxPossibleHeight / 0.9f);
                    noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);										// Clamp x, y  values to the normalized height between 0 and the maximum height

                }
            }
		}
		if (settings.normalizeMode == NormalizeMode.Local) {																// If the normalize mode is Local
            for (int y = 0; y < mapHeight; y++) {																			// For every point in the map
				for (int x = 0; x < mapWidth; x++) {
					noiseMap[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);			// Return a value between the minimum and maximum height using nosieMap[x,  y] as the input

				}
			}
		}
		return noiseMap;																									// Return the float array noiseMap

	}

}

[System.Serializable]
public class NoiseSettings {
    public Noise.NormalizeMode normalizeMode;

    public float scale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public static int seed;
    public static Vector2 offset;

	public void ValidateValues() {																							// Validate values are valid
		scale = Mathf.Max(scale, 0.01f);																					// Minimum scale value is 0.01
		octaves = Mathf.Max(octaves, 1);																					// Minimum number of octaves is 1
		lacunarity = Mathf.Max(lacunarity, 1);																				// Minimum lacunarity is 1
		persistance = Mathf.Clamp01(persistance);																			// Clamp value of persistance between 0 and 1

	}

}
