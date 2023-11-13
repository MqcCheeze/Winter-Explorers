using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    private int health = 10;

    PlayerMovement playerMovement;

    private void Start() {
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void Attacked(Vector3 velocity) {

        health -= 1;
        playerMovement.Knockback(velocity);

        if (health <= 0) {

            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(0);

        }
    }
}
