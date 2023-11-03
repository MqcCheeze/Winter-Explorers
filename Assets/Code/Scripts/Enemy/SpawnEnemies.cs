using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] private GameObject enemy;


    private void Start() {

        StartCoroutine(Spawn());
    }

    IEnumerator Spawn() {

        yield return new WaitForSeconds(Random.Range(5, 11));
        for (int i = 0; i < Random.Range(5, 11); i++) {

            Vector3 spawnPos;

            do {

                spawnPos = new Vector3(Random.Range(-40, 40), 100, Random.Range(-40, 40));
            } while (spawnPos.x > -5 && spawnPos.x < 5 && spawnPos.z > -5 && spawnPos.z < 5);

            RaycastHit hit;
            if (Physics.Raycast(spawnPos, Vector3.down, out hit)) {

                spawnPos = hit.point + new Vector3(0, 1, 0);
            }
            
            Instantiate(enemy, spawnPos, Quaternion.identity);
        }

        StartCoroutine(Spawn());
    }
}
