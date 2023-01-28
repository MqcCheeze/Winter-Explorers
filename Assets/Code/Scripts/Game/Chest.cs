using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animation chestAnim;                                                                        // Chest animations
    private bool isOpen;                                                                                // To check if the chest is open or closed
    [SerializeField] private List<GameObject> chestInventory = new List<GameObject>();                  // Objects the chest holds
    [SerializeField] private List<GameObject> obtainableItems = new List<GameObject>();                 // Number of items that will be generated in the chest

    [SerializeField] private PlayerInventory playerInventory;                                           // The player's inventory

    private Vector3 itemPos = new Vector3(0f, 1f, 1f);                                                  // Item position when the chest is opened

    private void Start() {
        float scale;
        chestAnim = GetComponent<Animation>();                                                          // Get the animations for the chest
        int numOfItems = Random.Range(1, 5);                                                            // Generate a random number for the amount of items the chest can generate
        for (int i = 0; i < numOfItems; i++) {
            GameObject instantiatedObject = Instantiate(obtainableItems[Random.Range(0, 3)], this.transform.position + itemPos, Quaternion.identity, this.transform);   // Instantiate object
            scale = Random.Range(0.1f, 1.5f);
            instantiatedObject.transform.localScale = new Vector3(scale, scale, scale);
            chestInventory.Add(instantiatedObject);                                                     // Add the instantiated object to the list of objects in the chest
            instantiatedObject.SetActive(false);                                                        // Set the object to false so the player cannot interact with it
        }
    }
    public void ChestInteract(GameObject player) {                                                      // Chest interaction
        switch (isOpen) {                                                                               // Is the chest open?
            case false:                                                                                 // If it isnt
                isOpen = true;                                                                          // Open it
                OpenChest(player); 
                break;
            case true:                                                                                  // If it is
                isOpen = false;                                                                         // Close it
                CloseChest(); 
                break;
        }
    }

    private void OpenChest(GameObject player) {                                                         // Open chest
        playerInventory = player.GetComponent<PlayerInventory>();                                       // Get the player inventory
        chestAnim.Play("ChestOpen");                                                                    // Play the open animation
        for (int i = 0; i < chestInventory.Count; i++) {                                                // loop through all items in the chest
            chestInventory[i].SetActive(true);                                                          // Set the objects to active so the player can interact with them
            playerInventory.PickUp(chestInventory[i]);                                                  // And try to pick the item up using the PickUp method in the PlayerInventory script
        }
        chestInventory.Clear();                                                                         // And clear the chest's inventory
    }

    private void CloseChest() {                                                                         // Close chest
        chestAnim.Play("ChestClose");                                                                   // Play the closing animation
    }
}
