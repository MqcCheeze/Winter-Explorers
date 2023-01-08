using UnityEngine;
using System.Collections;

public class MapPreview : MonoBehaviour {

	public Renderer textureRender;                              // Rendering the texture of the preview texture
	public MeshFilter meshFilter;                               // Mesh filter
	public MeshRenderer meshRenderer;                           // Rendering the mesh
    public enum DrawMode {NoiseMap, Mesh, FallOffMap};          // Draw modes: NoiseMap generates a noise map | Mesh generates a mesh with a texture | FallOffMap generates a fall off map
    public DrawMode drawMode;

    public MeshSettings meshSettings;                           // Call MeshSettings to access different mesh settings
    public HeightMapSettings heightMapSettings;                 // Call HeightMapSettings to access different height map values

    public Material terrainMaterial;                            // Material used to colour the terrain

    [Range(0, MeshSettings.numSupportedLODs - 1)]               // The number of editor LOD settings is equal to the ones that are supported
    public int editorPreviewLOD;                                // Level of detail of the preview mesh in the editor

    public bool autoUpdate;                                     // Whether the preview mesh should auto-update in the editor

    public void DrawMapInEditor() {                             // Drawing height map, mesh, and fall off map in the editor
        HeightMap heightMap = HeightMapGenerator.GenerateHeightMap(meshSettings.numVertsPerLine, meshSettings.numVertsPerLine, heightMapSettings, Vector2.zero);

        if (drawMode == DrawMode.NoiseMap) {
            DrawTexture(TextureGenerator.TextureFromHeightMap(heightMap));                                                                                  // Draw the noise map with the perlin noise as a texture
        } else if (drawMode == DrawMode.Mesh) {
            DrawMesh(MeshGenerator.GenerateTerrainMesh(heightMap.values, meshSettings, editorPreviewLOD));                                                  // Draw the mesh using the mesh settings at the LOD in the editor
        } else if (drawMode == DrawMode.FallOffMap) {
            DrawTexture(TextureGenerator.TextureFromHeightMap(new HeightMap(FallOffGenerator.GenerateFallOffMap(meshSettings.numVertsPerLine), 0, 1)));     // Draw the fall off map with the fall off map as the texture
        }
    }

    public void DrawTexture(Texture2D texture) {                                                        // Draw the texture onto the preview texture
        textureRender.sharedMaterial.mainTexture = texture;                                             // Set the object's material texture to the newly made texture
        textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height) / 10f;       // Set the scale of the object to the size of the texture divided by 10

        textureRender.gameObject.SetActive(true);                                                       // Enable preview textureobject
        meshFilter.gameObject.SetActive(false);                                                         // Disable preview mesh object
    }

    public void DrawMesh(MeshData meshData) {
        meshFilter.sharedMesh = meshData.CreateMesh();

        textureRender.gameObject.SetActive(false);                                                      // Disable preview textureobject
        meshFilter.gameObject.SetActive(true);                                                          // Enable preview mesh object
    }

    void OnValuesUpdated() {
        if (!Application.isPlaying) {                                                                   // If the game isn't being executed
            DrawMapInEditor();                                                                          // Then draw the map in the editor

        }
    }

    void OnValidate() {                                                                                 // If the value of any variable changes in the editor, update it to visualise the new change
        if (meshSettings != null) {
            meshSettings.OnValuesUpdated -= OnValuesUpdated; 
            meshSettings.OnValuesUpdated += OnValuesUpdated;

        }
        if (heightMapSettings != null) {
            heightMapSettings.OnValuesUpdated -= OnValuesUpdated;
            heightMapSettings.OnValuesUpdated += OnValuesUpdated;

        }
    }

    

}
