using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Drawing;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class TerrainChunk {

    const float colliderGenerationDistanceThreshhold = 20;                                     // The distance the player has to be for the game to generate a collider mesh for the chunk in front

    public event System.Action<TerrainChunk, bool> onVisibilityChanged;

    public Vector2 coord;

    public GameObject meshObject;                                                               // The chunk
    Vector2 sampleCentre;
    Bounds bounds;

    MeshRenderer meshRenderer;                                                                  // Mesh's texture
    MeshFilter meshFilter;                                                                      // Mesh's mesh
    MeshCollider meshCollider;                                                                  // Mesh's collider

    LODInfo[] detailLevels;
    LODMesh[] lodMeshes;
    int colliderLODIndex;                                                                       // The LOD of the mesh's collider

    HeightMap heightMap;                                                                        // Height map
    bool heightMapReceived;                                                                     // Check if the height map is received
    int previousLODIndex = -1;                                                                  // Previous LOD index
    bool hasSetCollider;                                                                        // Check if the mesh has a collider
    float maxViewDistance;                                                                      // Maximum render distance

    HeightMapSettings heightMapSettings;                                                        // Get the height map settings
    MeshSettings meshSettings;                                                                  // Get the mesh settings
    Transform player;                                                                           // Get the player's position

    public TerrainChunk(Vector2 coord, HeightMapSettings heightMapSettings, MeshSettings meshSettings, LODInfo[] detailLevels, int colliderLODIndex, Transform parent, Transform player, Material material) {   // Terrain chunk
        this.coord = coord;                                                                     // Set coord to value received
        this.detailLevels = detailLevels;                                                       // Set detail levels to values received
        this.colliderLODIndex = colliderLODIndex;                                               // Set collider LOD index to value received
        this.heightMapSettings = heightMapSettings;                                             // Set the height map settings to values received
        this.meshSettings = meshSettings;                                                       // Set the mesh settings to values received
        this.player = player;                                                                   // Set the player position to value received

        sampleCentre = coord * meshSettings.meshWorldSize / meshSettings.meshScale;
        Vector2 position = coord * meshSettings.meshWorldSize;
        bounds = new Bounds(position, Vector2.one * meshSettings.meshWorldSize);                // Create a bound the size of the mesh 


        meshObject = new GameObject("Terrain Chunk");                                           // Create a new GameObject called "Terrain Chunk"
        meshObject.layer = 3;                                                                   // Set the chunk's layer to 3 (ground layer) so the player and other objects can interact with it correctly
        meshRenderer = meshObject.AddComponent<MeshRenderer>();                                 // Add a mesh renderer so the player can see the chunk mesh
        meshFilter = meshObject.AddComponent<MeshFilter>();                                     // Add a mesh filter to get the mesh of the chunk
        meshCollider = meshObject.AddComponent<MeshCollider>();                                 // Adds a collider so any object can collide with the mesh
        meshRenderer.material = material;                                                       // Set the mesh renderer's material to the terrain material

        meshObject.transform.position = new Vector3(position.x, 0, position.y);                 // Set the position of the mesh to the position specified
        meshObject.transform.parent = parent;                                                   // Set the object as a child of parent gameobject MapGenerator

        SetVisible(false);                                                                      // Set the GameObject to be disabled

        lodMeshes = new LODMesh[detailLevels.Length];
        for (int i = 0; i < detailLevels.Length; i++) {                                         // For every detail level
            lodMeshes[i] = new LODMesh(detailLevels[i].lod);
            lodMeshes[i].updateCallback += UpdateTerrainChunk;                                  // Update the terrain chunk
            if (i == colliderLODIndex) {
                lodMeshes[i].updateCallback += UpdateCollisionMesh;                             // Update the terrain chunk's collider mesh

            }
        }
        maxViewDistance = detailLevels[detailLevels.Length - 1].visibleDistanceThreshold;       // Set the max view distance to the visible distance threshold to prevent chunks from being generated outside the visible distance

    }

    public void Load() {
        ThreadedDataRequester.RequestData(() => HeightMapGenerator.GenerateHeightMap(meshSettings.numVertsPerLine, meshSettings.numVertsPerLine, heightMapSettings, sampleCentre), OnHeightMapReceived); // Request data and generate a height map


    }

    void OnHeightMapReceived(object heightMapObject) {
        this.heightMap = (HeightMap)heightMapObject;                                                    // Set the heightMap variable to the one received
        heightMapReceived = true;

        UpdateTerrainChunk();                                                                           // Update the terrain chunk
    }

    Vector2 playerPosition {
        get {
            return new Vector2(player.position.x, player.position.z);                                   // Return the player's x and z coords to be used as x and y for creating, loading and unloading chunks

        }
    }

    public void UpdateTerrainChunk() {
        if (heightMapReceived) {                                                                        // If it has the height map
            float playerDistanceFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(playerPosition));       // Calculate the distance from the player to the nearest edge of a chunk

            bool wasVisible = IsVisible();
            bool visible = playerDistanceFromNearestEdge <= maxViewDistance;                            // visible is if the player distance from the nearest edge is less or equal to the maximum view distance

            if (visible) { 
                int lodIndex = 0;

                for (int i = 0; i < detailLevels.Length - 1; i++) {                                     // For every detail level
                    if (playerDistanceFromNearestEdge > detailLevels[i].visibleDistanceThreshold) {     // If the player distance is more than the visible distance threshhold
                        lodIndex = i + 1;                                                               // Increase the lodIndex
                    } else {
                        break;                                                                          // If it isn't then skip
                    }
                }

                if (lodIndex != previousLODIndex) {                                                     // If the lod index is not the previous one
                    LODMesh lodMesh = lodMeshes[lodIndex];                                              // Set the ledMesh to the lodMesh in the lodMeshes array at the LOD index
                    if (lodMesh.hasMesh) {                                                              // If the lodMesh has a mesh
                        previousLODIndex = lodIndex;                                                    // Set the previous index to the current index
                        meshFilter.mesh = lodMesh.mesh;                                                 // Set the chunk's mesh to the mesh of lodMesh
                    } else if (!lodMesh.hasRequestedMesh) {                                             // If it hasn't requested a mesh
                        lodMesh.RequestMesh(heightMap, meshSettings);                                   // Request a mesh with the height map and mesh settings
                    }
                }
            }
            if (wasVisible != visible) {                                                                // If the chunk visibility isn't the same as the visible bool
                SetVisible(visible);                                                                    // Make it the same

                if (onVisibilityChanged != null) {                                                      // If the visibility has changed
                    onVisibilityChanged(this, visible);                                                 // Change the visibility of this chunk to the value of visible

                }
            }
        }
    }

    public void UpdateCollisionMesh() {                                                                                         // Update the chunk's collision mesh
        if (!hasSetCollider) {                                                                                                  // If the mesh doesn't have a collider
            float sqrDistanceFromPlayerToEdge = bounds.SqrDistance(playerPosition);                                             // Get the squared distance from the player to the edge of the chunk

            if (sqrDistanceFromPlayerToEdge < detailLevels[colliderLODIndex].sqrVisibleDistanceThreshold) {                     // If the squared distance is less than the squared visible distance threshold
                if (!lodMeshes[colliderLODIndex].hasRequestedMesh) {                                                            // And if the index in lodMeshes hasn't requrested a mesh
                    lodMeshes[colliderLODIndex].RequestMesh(heightMap, meshSettings);                                           // Request a mesh with the height map and mesh settings

                }
            }
                                                                                                                                // Otherwise    
            if (sqrDistanceFromPlayerToEdge < colliderGenerationDistanceThreshhold * colliderGenerationDistanceThreshhold) {    // If the player is within the distance to the edge
                if (lodMeshes[colliderLODIndex].hasMesh) {                                                                      // And if the mesh has a mesh
                    meshCollider.sharedMesh = lodMeshes[colliderLODIndex].mesh;                                                 // Set the mesh's collider to the mesh at index colliderLODIndex in lodMeshes array
                    hasSetCollider = true;                                                                                      // Say that the chunk has a collider

                }
            }
        }
    }

    public void SetVisible(bool visible) {                                                                                                      // Set the chunk's visibility
        meshObject.SetActive(visible);
    }

    public bool IsVisible() {                                                                                                                   // Check is the chunk visible
        return meshObject.activeSelf;                                                                                                           // Return the bool value if the chunk game object is active in the scene
    }
}

class LODMesh {
    public Mesh mesh;                                                                                                                           // The mesh
    public bool hasRequestedMesh;                                                                                                               // Check if the chunk has requested for a mesh
    public bool hasMesh;                                                                                                                        // Check if the chunk has a mesh
    int lod;                                                                                                                                    // The level of detail for the mesh
    public event System.Action updateCallback;

    public LODMesh(int lod) {                                                                                                                   // Set this class' lod to the lod inputted
        this.lod = lod;
    }

    void OnMeshDataReceived(object meshDataObject) {
        mesh = ((MeshData)meshDataObject).CreateMesh();                                                                                         // Create a mesh
        hasMesh = true;                                                                                                                         // Say that the chunk has a mesh

        updateCallback();
    }

    public void RequestMesh(HeightMap heightMap, MeshSettings meshSettings) {                                                                   // Request the mesh with the height map and the mesh settings
        hasRequestedMesh = true;                                                                                                                // Say that the mesh has been requested
        ThreadedDataRequester.RequestData(() => MeshGenerator.GenerateTerrainMesh(heightMap.values, meshSettings, lod), OnMeshDataReceived);    // Request for the chunk data by generating the terrain mesh

    }
}
