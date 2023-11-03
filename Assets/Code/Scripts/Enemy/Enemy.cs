using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform playerTransform;
    private Vector3 velocity = new Vector3(0, -2, 0);
    private CharacterController characterController;

    private void Start() {

        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate() {

        characterController.Move(velocity);

        this.transform.position = Vector3.MoveTowards(this.transform.position, playerTransform.position + new Vector3(0, 1, 0), 0.05f);
    }
}
