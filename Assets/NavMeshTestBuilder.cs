using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshTestBuilder : MonoBehaviour
{
    NavMeshSurface surface;                                 // The surface the nav mesh surface is going to build

    void Start()
    {
        surface = GetComponent<NavMeshSurface>();           // Get the NavMeshSurface component
        StartCoroutine(BuildNavMeshSurface());              // Start building the nav mesh surface
    }

    private IEnumerator BuildNavMeshSurface() {             // IEnumerator function to build nav mesh surface continously
        while (true) {                                      // Infinite loop, stop with StopCoroutine(BuildNavMeshSurface()) 
            yield return new WaitForSeconds(1f);            // Wait a second
            surface.BuildNavMesh();                         // Build the surface mesh
        }
    }
}
