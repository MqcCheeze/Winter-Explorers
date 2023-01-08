using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class MeshSettings : UpdatableData {
    public const int numSupportedLODs = 5;                                                                  // Num of supported levels of details 

    public const int numSupportedChunkSizes = 9;                                                            // Max chunk sizes
    public const int numSupportedFlatShadedChunkSizes = 3;                                                  // Num of supported chunk sizes (first 3 from supportedChunkSizes)
    public static readonly int[] supportedChunkSizes = {48, 72, 96, 120, 144, 168, 192, 216, 240};          // Num of supported chunk sizes (not flat shaded)

    public float meshScale;                                                                                 // Scale of the mesh (scale up or down to better fit player size)

    public bool useFlatShading;                                                                             // Use flat shading?

    [Range(0, numSupportedChunkSizes - 1)]
    public int chunkSizeIndex; 

    [Range(0, numSupportedFlatShadedChunkSizes - 1)]
    public int flatShadedChunkSizeIndex;

    public int numVertsPerLine {                                                                            // Number of vertices per row at LOD 0 (includes 2 extra vertices expluded from final mesh), used for calculating normals
        get {
            return supportedChunkSizes[(useFlatShading)? flatShadedChunkSizeIndex : chunkSizeIndex] + 5;

        }
    }

    public float meshWorldSize {                                                                            // Count number of vertices in a row and multiply it by mesh scale to get the width/height of the mesh world size
        get {
            return (numVertsPerLine - 3) * meshScale;

        }
    
    }
}
