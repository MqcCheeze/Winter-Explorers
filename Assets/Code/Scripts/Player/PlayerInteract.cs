using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactRange;                                                               // Range of the interaction ray
    [SerializeField] private LayerMask interactLayer;                                                           // The layers the interact ray will hit
    [SerializeField] private GameObject interactionHint;                                                        // Interaction hint

    [SerializeField] private GameObject currentObject;                                                          // The current object
    private GameObject player;

    private void Start() {
        player = this.gameObject;                                                                               // Set the player gameobject variable
    }

    void Update() {
        InteractRay();                                                                                          // Interact up raycast
    }

    private void InteractRay() {                                                                                // Shoot out a ray infront of player to detetect any objects
        RaycastHit hitInfo;
        if (Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out hitInfo, interactRange, interactLayer)) {
            interactionHint.SetActive(true);                                                                    // Show the interaction hint                                                   
            if (Input.GetKeyDown(KeyCode.E)) {                                                                  // If player presses the pick up key (E)
                currentObject = hitInfo.collider.gameObject;                                                    // Swap current item with newer item
                Interact();                                                                                     // Pick up the object
            }
        } else {
            interactionHint.SetActive(false);                                                                   // Hide the interactio hint
        }
    }

    private void Interact() {                                                                                   // Interactions
        /*
        if (currentObject.CompareTag("Chest")) {                                                                // If the object is a chest
            currentObject.GetComponent<Chest>().ChestInteract(player);                                          // Get the chest script and interact with the chest on the player
        }
        */
        switch (currentObject.tag) {
            case "Chest":
                currentObject.GetComponent<Chest>().ChestInteract(player);                                      // Get the chest script and interact with the chest on the player
                break;
        }
    }


}
