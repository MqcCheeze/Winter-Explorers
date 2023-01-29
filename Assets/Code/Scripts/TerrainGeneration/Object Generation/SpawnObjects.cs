using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    public List<GameObject> objects;
    public float rayDistance;
    public LayerMask groundLayer;
    public float radius;
    public Vector2 regionSize;
    public int rejectionSamples;

    List<Vector2> points;

    public bool alreadySpawnedTrees;

    private Vector3 treeSpawning = new Vector3(0f, -0.125f, 0f);
    private Vector3 rockSpawning = new Vector3(0f, -0.2f, 0f);
    private Vector3 objectSpawning = new Vector3(0f, 0.5f, 0f);

    private void Start() {
        StartCoroutine(SpawnOnLoad());
    }

    public void SpawnObjectsInWorld() {
        if (!alreadySpawnedTrees) {
            points = PoissonDiscSamp.GeneratePoints(radius, regionSize, rejectionSamples);
            if (Physics.Raycast(new Vector3(this.transform.position.x, 100, this.transform.position.z), Vector3.down, 100f)) {
                alreadySpawnedTrees = true;
            }
            foreach (Vector2 point in points) {
                Vector3 position = new Vector3(point.x + this.transform.position.x, 100, point.y + this.transform.position.z);
                RaycastHit hit;
                if (Physics.Raycast(position, Vector3.down, out hit, rayDistance, groundLayer)) {
                    GameObject objectToSpawn = objects[Random.Range(0, objects.Count - 1)];
                    switch (objectToSpawn.tag) {
                        case "Tree":
                            Instantiate(objectToSpawn, hit.point + treeSpawning, Quaternion.identity, this.transform);
                            break;
                        case "Rock":
                            Instantiate(objectToSpawn, hit.point + rockSpawning, Quaternion.identity, this.transform);
                            break;
                        default:
                            Instantiate(objectToSpawn, hit.point + objectSpawning, Quaternion.identity, this.transform);
                            break;
                    }
                }
            }
        }
    }

    private IEnumerator SpawnOnLoad() {
        yield return new WaitForSeconds(1f);
        SpawnObjectsInWorld();
        StartCoroutine(RunTimeSpawn());
    }

    private IEnumerator RunTimeSpawn() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(5, 15));
            Debug.ClearDeveloperConsole();
            SpawnObjectsInWorld();
            Debug.Log("Attempted to spawn objects");
        }
    }
}
