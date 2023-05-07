using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;

    [SerializeField] private float interactRange;                                                               // Range of the interaction ray
    [SerializeField] private LayerMask interactLayer;                                                           // The layers the interact ray will hit
    [SerializeField] private GameObject interactionHint;                                                        // Interaction hint

    [SerializeField] private GameObject currentObject;                                                          // The current object
    private GameObject player;
    private PlayerInventory playerInventory = new PlayerInventory();

    void Start() {
        eventSystem.PlayerInteractions += EventSystem_PlayerInteractions;

        player = this.gameObject;                                                                               // Set the player gameobject variable
    }

    void FixedUpdate() {
        InteractRay();                                                                                          // Interact raycast
    }


    private void EventSystem_PlayerInteractions(object sender, EventSystem.KeyPressed e) {
        if (e.keyPressed == "e") {
            RaycastHit hitInfo;
            if (Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out hitInfo, interactRange)) {
                currentObject = hitInfo.collider.gameObject;                                                // Swap current item with newer item
                Interact();                                                                                 // Pick up the object
            }
        } else if (e.keyPressed == "rMouse") {
            StartCoroutine(Use());
        }
    }

    private void InteractRay() {                                                                                // Shoot out a ray infront of player to detetect any objects
        RaycastHit hitInfo;
        if (Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out hitInfo, interactRange, interactLayer)) {
            interactionHint.SetActive(true);                                                                // Show the interaction hint    
        } else {
            interactionHint.SetActive(false);                                                                   // Hide the interactio hint
        }
    }

    private void Interact() {                                                                                   // Interactions
        switch (currentObject.tag) {
            case "Chest":
                currentObject.GetComponent<Chest>().ChestInteract(player);                                      // Get the chest script and interact with the chest on the player
                break;
        }
    }


    private IEnumerator Use() {
        currentObject = PlayerInventory.currentItem;
        if (currentObject.CompareTag("Bottle")) {
            playerInventory.Drop();
            yield return new WaitForSeconds(2f);
            Destroy(currentObject);
        }
    }
}
