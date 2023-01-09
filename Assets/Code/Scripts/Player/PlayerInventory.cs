using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour {

    [SerializeField] private bool canPickUp;                                            // Enable/disable pickup ability
    private bool changedSlot;
    [SerializeField] private bool canScroll;

    [Header("Inventory")]
    public List<GameObject> inventory = new List<GameObject>();                         // Inventory
    [HideInInspector] private List<RawImage> hotbarSelector = new List<RawImage>();     // Select what item is being held
    [HideInInspector] public List<RawImage> itemImages = new List<RawImage>();          // Icons for images
    [SerializeField] private Transform hand;
    public ItemChunk itemChunk;

    [Header("Colours")]
    [SerializeField] private Color selected;                                            // Colour of selected inventory slot
    [SerializeField] private Color unselected;                                          // Colour of unselected inventory slot

    [Header("Current Item")]
    [SerializeField] private GameObject currentItem;                                    // Item currently held
    [SerializeField] private Rigidbody currentItemBody;                                 // Item body currently held
    [SerializeField] private int currentSlot;                                           // The slot the current item is in

    [Header("Previous Item")]
    [SerializeField] private GameObject previousItem;                                   // Previous item
    [SerializeField] private int previousSlot;                                          // Previous item's slot

    [Header("Pick up item")]
    [SerializeField] private bool holdingItem;                                          // Check if player is holding an item
    [SerializeField] private float pickUpRange;
    [SerializeField] private LayerMask pickUpLayer;                                     // Filter what objects can be picked up

    [SerializeField] private GameObject pickUpHint;                                     // Notify the player they can pick up the object they're looking at
    [SerializeField] private Animation inventoryFull;                                   // Notify the player when their inventory is full

    [Header("Drop item")]
    [SerializeField] private Vector3 dropPos;                                           // Drop location
    [SerializeField] private Transform pickUpObjectList;                                // Parent of the items the player can pick up

    private void FixedUpdate() {
        for (int i = 0; i < inventory.Count; i++) {
            itemImages[i].texture = inventory[i].GetComponent<RawImage>().texture;
        }
    }

    private void Update() {
        PickUpRay();                                                                    // Pick up raycast
        Scrolling();                                                                    // See if player is scrolling through inventory

        if (Input.GetKeyDown(KeyCode.Q)) {                                              // Drop if Q is pressed
            Drop();                                                 
        }

        if (changedSlot) {
            ChangeSlot();                                                               // Change slot
        }
    }

    private void PickUpRay() {                                                          // Shoot out a ray infront of player to detetect any objects
        RaycastHit hitInfo;
        if (Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out hitInfo, pickUpRange, pickUpLayer)) {
            pickUpHint.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E)) {                                                      // If player presses the pick up key (E)
                if (currentItem != null) {                                                          // If the player is holding an item
                    currentItem.GetComponent<Renderer>().enabled = false;                           // Disable rendering for the durrent item
                }

                previousItem = currentItem;                                                         // Set previous item to current item
                previousSlot = currentSlot;                                                         // Set previous slot to current slot

                currentItem = hitInfo.collider.gameObject;                                          // Swap current item with newer item
                PickUp();                                                                           // Pick up the object
            } else if (Input.GetMouseButtonDown(0)) {
                Destroy(hitInfo.transform.gameObject);
            }
        } else {
            pickUpHint.SetActive(false);
        }
    }

    private void PickUp() {                                                                         // Pick up the object
        if (inventory.Count < 9) {                                                                  // If inventory isn't full
            currentItem.transform.SetParent(hand);                                                  // Set object to a child of player's hand
            currentItem.transform.localPosition = Vector3.zero;                                     // Reset item's position
            currentItem.transform.localRotation = Quaternion.Euler(0f, 090f, 0f);                   // Rotate item
            currentItem.GetComponent<Collider>().enabled = false;                                   // Disable collider when in hand

            currentItemBody = currentItem.GetComponent<Rigidbody>();                                // Get the item's rigidbody
            currentItemBody.constraints = RigidbodyConstraints.FreezePosition;                      // Stop item from falling out of player's hand
            currentItemBody.freezeRotation = true;                                                  // Freeze item rotation

            holdingItem = true;

            inventory.Add(currentItem);                                                             // Add item to the inventory
            itemImages[inventory.Count - 1].texture = currentItem.GetComponent<RawImage>().texture; // Get the texture of the item
            itemImages[inventory.Count - 1].enabled = true;                                         // Enable the texture of the item

            currentSlot = inventory.Count - 1;                                                      // Set the current slot to the slot of the item picked up

            hotbarSelector[previousSlot].color = unselected;                                        // Unselect the old item
            hotbarSelector[currentSlot].color = selected;                                           // Indicate the new item being held

        } else if (inventory.Count >= 9) {                                                          // If the inventory is full
            currentItem = previousItem;                                                             // Set the item to the current item
            currentItem.GetComponent<Renderer>().enabled = true;                                    // Render the previous item

            currentSlot = previousSlot;                                                             // Set the slot to the previous item

            inventoryFull.Play();                                                                   // Play inventory full animation
        }
    }

    private void Drop() {
        if (holdingItem) {
            currentItem.transform.localPosition = dropPos;                                          // Set object position to drop position
            currentItem.transform.localRotation = Quaternion.Euler(0f, 090f, 0f);                   // Reset rotation
            itemChunk = currentItem.GetComponent<ItemChunk>();
            currentItem.transform.SetParent(itemChunk.parentChunk);                                      // Put object back into Objects list
            currentItem.GetComponent<Collider>().enabled = true;                                    // Enable collision

            currentItemBody.constraints = RigidbodyConstraints.None;                                // Disable constraints
            currentItemBody.freezeRotation = false;                                                 // Unfreeze rotation

            holdingItem = false;

            hotbarSelector[currentSlot].color = unselected;                                         // Deselect slot
            itemImages[currentSlot].texture = null;                                                 // Remove item icon from inventory

            inventory.Remove(currentItem);                                                          // Remove item from inventory

            itemImages[inventory.Count].enabled = false;

            if (currentSlot <= inventory.Count - 1) {
                changedSlot = true;

            } else if (currentSlot == inventory.Count && inventory.Count != 0) {                    // If there is more items
                currentSlot -= 1;
                changedSlot = true;

                hotbarSelector[currentSlot].color = selected;                                       // Select next item

            }
        }
        currentItem = null;                                                                         // Set the current item to nothing
        currentItemBody = null;                                                                     // Set the current item's body to nothing

    }

    private void Scrolling() {
        if (canScroll) {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) {                                          // Scroll down
                if (currentSlot > 0) {
                    previousSlot = currentSlot;
                    previousItem = currentItem;

                    currentSlot -= 1;                                                               // Change current slot to previous slot
                    changedSlot = true;
                } else if (currentSlot == 0 && inventory.Count == 1) {
                    changedSlot = true;
                }

            } else if (Input.GetAxis("Mouse ScrollWheel") < 0f) {                                   // Scroll up
                if (currentSlot < inventory.Count - 1) {
                    previousSlot = currentSlot;
                    previousItem = currentItem;

                    currentSlot += 1;                                                               // Change current slot to next slot
                    changedSlot = true;
                } else if (currentSlot == inventory.Count - 1 && inventory.Count == 1) {
                    changedSlot = true;
                }
            }

            hotbarSelector[previousSlot].color = unselected;
            hotbarSelector[currentSlot].color = selected;                                           // Indicate what slot is selected
        }
    }

    private void ChangeSlot() {
        changedSlot = false;                                                                        // So this function won't loop

        if (currentItem != null) {                                                                  // If player is currently holding an item
            currentItem.GetComponent<Renderer>().enabled = false;                                   // Don't render the current object
        }

        currentItem = inventory[currentSlot];                                                       // Swap the object for the newer current object
        currentItem.GetComponent<Renderer>().enabled = true;                                        // And render this new object

        currentItemBody = currentItem.GetComponent<Rigidbody>();                                    // Set the current item body to the new current item's body

        holdingItem = true;                                                                         // Confirm the player is holding an item
    }
}
