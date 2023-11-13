using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject enemyList;
    public int enemyCount;

    private void Start() {

        StartCoroutine(Spawn());
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            Vector3 spawnPos;


            do {

                spawnPos = new Vector3(Random.Range(-50, 50), 100, Random.Range(-50, 50));
            } while ((spawnPos.x > -10 && spawnPos.x < 10) || (spawnPos.z > -10 && spawnPos.z < 10));

            RaycastHit hit;
            if (Physics.Raycast(spawnPos, Vector3.down, out hit)) {

                spawnPos = hit.point + new Vector3(0, 1, 0);
            }

            Instantiate(enemy, spawnPos, Quaternion.identity, enemyList.transform);
            enemyCount++;
        }
    }

    IEnumerator Spawn() {

        yield return new WaitForSeconds(Random.Range(20, 41));
        for (int i = 0; i < Random.Range(5, 11); i++) {

            Vector3 spawnPos;

            if (enemyCount < 30) {

                do {

                    spawnPos = new Vector3(Random.Range(-50, 50), 100, Random.Range(-50, 50));
                } while ((spawnPos.x > -10 && spawnPos.x < 10) || (spawnPos.z > -10 && spawnPos.z < 10));

                RaycastHit hit;
                if (Physics.Raycast(spawnPos, Vector3.down, out hit)) {

                    spawnPos = hit.point + new Vector3(0, 1, 0);
                }

                Instantiate(enemy, spawnPos, Quaternion.identity, enemyList.transform);
                enemyCount++;
            }
        }

        StartCoroutine(Spawn());
    }
}
