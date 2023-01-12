using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactRange;
    [SerializeField] private LayerMask interactLayer;

    [SerializeField] private GameObject currentObject;
    private GameObject player;

    private void Start() {
        player = this.gameObject;
    }

    void Update() {
        InteractRay();                                                                    // Pick up raycast
    }

    private void InteractRay() {                                                          // Shoot out a ray infront of player to detetect any objects
        RaycastHit hitInfo;
        if (Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out hitInfo, interactRange, interactLayer)) {
            //pickUpHint.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E)) {                                                      // If player presses the pick up key (E)
                currentObject = hitInfo.collider.gameObject;                                          // Swap current item with newer item
                Interact();                                                                           // Pick up the object
            } else if (Input.GetMouseButtonDown(0)) {
                Destroy(hitInfo.transform.gameObject);
            }
        } else {
            //pickUpHint.SetActive(false);
        }
    }

    private void Interact() {
        if (currentObject.CompareTag("Chest")) {
            currentObject.GetComponent<Chest>().ChestInteract(player);
        }
    }


}
