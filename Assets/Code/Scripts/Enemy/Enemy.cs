using System;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform playerTransform;
    private Vector3 velocity = new Vector3(0, -2, 0);
    private CharacterController characterController;

    [SerializeField] private SpawnEnemies spawnEnemies;
    private float originalHealth;
    public float health;
    private float speed;

    [SerializeField] private TMP_Text score;

    private void Start() {

        health = originalHealth = RandomWithBias(10f, 20f, 3f);
        Debug.Log(originalHealth);
        speed = RandomWithBias(0.05f, 0.1f, 3f);

        score = GameObject.FindGameObjectWithTag("Points").GetComponent<TMP_Text>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        spawnEnemies = GameObject.FindGameObjectWithTag("SpawnEnemies").GetComponent<SpawnEnemies>();
        characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate() {

        characterController.Move(velocity);

        this.transform.position = Vector3.MoveTowards(this.transform.position, playerTransform.position + new Vector3(0, 1, 0), speed);
        this.transform.LookAt(playerTransform);

        if (health <= 0) {
            spawnEnemies.enemyCount--;
            Destroy(this.gameObject);

            score.text = Convert.ToString(Convert.ToInt32(Convert.ToInt32(score.text) + Mathf.RoundToInt(originalHealth / 5)));
        }
    }

    private float RandomWithBias(float low, float high, float bias) { // higher bias values = average closer to "low"
        float rand = UnityEngine.Random.Range(0, 2);                  // lower bias values = average closer to "high"
        rand = Mathf.Pow(rand, bias);                                 // bias of 1.0 equal chance
        return low + (high - low) * rand;
    }
}
