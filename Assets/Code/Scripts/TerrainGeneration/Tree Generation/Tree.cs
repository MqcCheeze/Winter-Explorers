using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public List<GameObject> trees;
    public float rayDistance;
    public LayerMask groundLayer;
    public float radius;
    public Vector2 regionSize;
    public int rejectionSamples;

    public Vector3 spawnTrees;

    List<Vector2> points;

    public bool alreadySpawnedTrees;

    private Vector3 treeSpawning = new Vector3(0, 0.125f, 0);
    private Vector3 objectSpawning = new Vector3(0, 0.5f, 0);

    private void Start() {
        StartCoroutine(SpawnOnLoad());
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.U)) {
            SpawnTrees();
        }
    }

    public void SpawnTrees() {
        if (!alreadySpawnedTrees) {
            points = PoissonDiscSamp.GeneratePoints(radius, regionSize, rejectionSamples);
            if (Physics.Raycast(new Vector3(this.transform.position.x, 100, this.transform.position.z), Vector3.down, 100f, groundLayer)) {
                alreadySpawnedTrees = true;
            }
            foreach (Vector2 point in points) {
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(point.x + this.transform.position.x, 100, point.y + this.transform.position.z), Vector3.down, out hit, rayDistance, groundLayer)) {
                    GameObject objectToSpawn = trees[Random.Range(0, trees.Count - 1)];
                    for (int i = 0; i < trees.Count - 1; i++) {
                        if (trees[i] == objectToSpawn && i < 3) {
                            Instantiate(objectToSpawn, hit.point - treeSpawning, Quaternion.identity, this.transform);
                        } else if (trees[i] == objectToSpawn && i >= 3 && i < trees.Count) {
                            Instantiate(objectToSpawn, hit.point + objectSpawning, Quaternion.identity, this.transform);
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
