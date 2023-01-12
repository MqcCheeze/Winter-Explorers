using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Burst.CompilerServices;
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

    private Vector3 treeSpawning = new Vector3(0, 0.125f, 0);
    private Vector3 objectSpawning = new Vector3(0, 0.5f, 0);

    private float time;

    private void Start() {
        StartCoroutine(SpawnOnLoad());
    }
    private void FixedUpdate() {
        if (time < 10) {
            time += Time.deltaTime;
        } else {
            time = 0;
            SpawnTrees();
            Debug.Log("Attempted to spawn trees");
        }
    }

    public void SpawnTrees() {
        if (!alreadySpawnedTrees) {
            points = PoissonDiscSamp.GeneratePoints(radius, regionSize, rejectionSamples);
            if (Physics.Raycast(new Vector3(this.transform.position.x, 100, this.transform.position.z), Vector3.down, 100f)) {
                alreadySpawnedTrees = true;
            }
            foreach (Vector2 point in points) {
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(point.x + this.transform.position.x, 100, point.y + this.transform.position.z), Vector3.down, out hit, rayDistance, groundLayer)) {
                    int objectToSpawn = Random.Range(0, 13);
                    for (int i = 0; i < objects.Count - 1; i++) {
                        if (i == objectToSpawn && i <= 8) {
                            Instantiate(objects[i], hit.point - treeSpawning, Quaternion.identity, this.transform);
                        } else if (i == objectToSpawn && i >= 9) {
                            Instantiate(objects[i], hit.point + objectSpawning, Quaternion.identity, this.transform);
                        }
                    }
                }
            }
        }
    }

    private IEnumerator SpawnOnLoad() {
        yield return new WaitForSeconds(1f);
        SpawnTrees();

    }
}
