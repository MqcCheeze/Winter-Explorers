using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBreak : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;

    [SerializeField] private float breakRange;                                                              // Range of the break ray
    [SerializeField] private LayerMask breakLayer;                                                          // The layers the break ray will hit
    [SerializeField] private GameObject breakHint;                                                          // Break hint

    [SerializeField] private GameObject currentObject;                                                      // The current object
    private PlayerInventory playerInventory;

    void Start() {
        eventSystem.PlayerInteractions += EventSystem_PlayerInteractions;
        playerInventory = GetComponent<PlayerInventory>();                                                  // Get the player inventory
    }

    void FixedUpdate() {
        BreakRay();                                                                                         // Break raycast
    }
    private void EventSystem_PlayerInteractions(object sender, EventSystem.KeyPressed e) {
        if (e.keyPressed == "lMouse") {
            RaycastHit hitInfo;
            if (Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out hitInfo, breakRange, breakLayer)) {
                currentObject = hitInfo.collider.gameObject;                                                // Swap current object with newer object
                Break();                                                                                    // Break the object                                          
            }
            
        }
    }

    private void BreakRay() {                                                                               // Shoot out a ray infront of player to detetect any objects
        RaycastHit hitInfo;
        if (Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out hitInfo, breakRange, breakLayer)) {
            breakHint.SetActive(true);                                                                      // Show the break hint                                                   
        } else {
            breakHint.SetActive(false);                                                                     // Hide the break hint
        }
    }

    private void Break() {                                                                                  // Break object the player is looking at
        
        switch (currentObject.tag) {
            case "Tree":
                try {
                    if (playerInventory.currentItem.CompareTag("Axe")) {
                        Debug.Log($"{currentObject.name} broken");
                        Destroy(currentObject);
                    }
                } catch {

                }
                break;
            case "Rock":
                try {
                    if (playerInventory.currentItem.CompareTag("Pickaxe")) {
                        Debug.Log($"{currentObject.name} broken");
                        Destroy(currentObject);
                    }
                } catch {

                }
                break;
        }
    }
}
