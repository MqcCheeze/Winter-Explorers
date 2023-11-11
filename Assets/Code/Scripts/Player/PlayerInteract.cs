using System;
using System.Collections;
using TMPro;
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
        if (e.keyPressed == "e" || e.keyPressed == "lMouse") {
            RaycastHit hitInfo;
            if (Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out hitInfo, interactRange)) {
                currentObject = hitInfo.collider.gameObject;                                                // Swap current item with newer item
                Interact(e);                                                                                 
            }
        } else if (e.keyPressed == "rMouse") {
            StartCoroutine(Use());
        }
    }

    private void InteractRay() {                                                                                // Shoot out a ray infront of player to detetect any objects
        RaycastHit hitInfo;
        if (Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out hitInfo, interactRange, interactLayer)) {
            interactionHint.SetActive(true);                                                                    // Show the interaction hint    
        } else {
            interactionHint.SetActive(false);                                                                   // Hide the interactio hint
        }
    }

    private void Interact(EventSystem.KeyPressed e) {                                                        // Interactions
        if (e.keyPressed == "e") {
            switch (currentObject.tag) {
                case "Chest":
                    currentObject.GetComponent<Chest>().ChestInteract(player);                               // Get the chest script and interact with the chest on the player
                    break;
            }
        } else if (e.keyPressed == "lMouse") {
            switch (currentObject.tag) {
                case "Enemy":
                    AttackEnemy();
                    break;
            }
        }
    }

    private void AttackEnemy() {
        if (PlayerInventory.currentItem.CompareTag("Sword")) {
            currentObject.GetComponent<Enemy>().health -= 2.5f;
        } else if (PlayerInventory.currentItem.CompareTag("Axe")) {
            currentObject.GetComponent<Enemy>().health -= 2;
        } else if (PlayerInventory.currentItem.CompareTag("Pickaxe")) {
            currentObject.GetComponent<Enemy>().health -= 1.5f;
        } else {
            currentObject.GetComponent<Enemy>().health -= 1;
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
