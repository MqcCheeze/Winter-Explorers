using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBreak : MonoBehaviour
{
    [SerializeField] private float breakRange;                                                              // Range of the break ray
    [SerializeField] private LayerMask breakLayer;                                                          // The layers the break ray will hit
    [SerializeField] private GameObject breakHint;                                                          // Break hint

    [SerializeField] private GameObject currentObject;                                                      // The current object
    private GameObject player;
    private PlayerInventory playerInventory;

    void Start() {
        player = this.gameObject;                                                                           // Set the player gameobject variable
    }

    void Update() {
        BreakRay();                                                                                         // Break raycast
    }

    private void BreakRay() {                                                                               // Shoot out a ray infront of player to detetect any objects
        RaycastHit hitInfo;
        if (Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out hitInfo, breakRange, breakLayer)) {
            breakHint.SetActive(true);                                                                      // Show the break hint                                                   
            if (Input.GetMouseButtonDown(0)) {                                                              // If player presses the break button (LMB)
                currentObject = hitInfo.collider.gameObject;                                                // Swap current object with newer object
                Break();                                                                                    // Break the object
            }
        } else {
            breakHint.SetActive(false);                                                                     // Hide the break hint
        }
    }

    private void Break() {                                                                                  // Break object the player is looking at
        playerInventory = player.GetComponent<PlayerInventory>();                                           // Get the player inventory
        switch (currentObject.tag) {
            case "Tree":
                Debug.Log($"{currentObject.name} broken");
                Destroy(currentObject);
                break;
            case "Rock":
                Debug.Log($"{currentObject.name} broken");
                Destroy(currentObject);
                break;
        }
    }
}
