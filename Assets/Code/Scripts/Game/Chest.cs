using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animation chestAnim;
    private bool isOpen;
    [SerializeField] private List<GameObject> chestInventory = new List<GameObject>();
    [SerializeField] private List<GameObject> obtainableItems = new List<GameObject>();

    [SerializeField] private PlayerInventory playerInventory;

    private Vector3 itemPos = new Vector3(0f, 1f, 1f);

    private void Start() {
        chestAnim = GetComponent<Animation>();
        int numOfItems = Random.Range(1, 5);
        for (int i = 0; i < numOfItems; i++) {
            GameObject instantiatedObject = Instantiate(obtainableItems[Random.Range(0, 3)], this.transform.position + itemPos, Quaternion.identity, this.transform);
            chestInventory.Add(instantiatedObject);
            instantiatedObject.SetActive(false);
        }
    }
    public void ChestInteract(GameObject player) {
        switch (isOpen) {
            case false:
                isOpen = true;
                OpenChest(player); 
                break;
            case true:
                isOpen = false;
                CloseChest(); 
                break;
        }
    }


    private void OpenChest(GameObject player) {
        playerInventory = player.GetComponent<PlayerInventory>();
        chestAnim.Play("ChestOpen");
        for (int i = 0; i < chestInventory.Count; i++) {
            chestInventory[i].SetActive(true);
            playerInventory.PickUp(chestInventory[i]);
        }
        chestInventory.Clear();
    }

    private void CloseChest() {
        chestAnim.Play("ChestClose");
    }
}
