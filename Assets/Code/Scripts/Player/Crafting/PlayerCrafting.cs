using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrafting : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private PauseMenu gui;
    [SerializeField] private GameObject player;
    private PlayerInventory playerInventory;

    public GameObject itemToCraft;
    public GameObject[] craftableItems;

    public void Craft() {
        GameObject craftedItem = Instantiate(itemToCraft, player.transform.position, Quaternion.identity);
        playerInventory = player.GetComponent<PlayerInventory>();
        playerInventory.PickUp(craftedItem);
        ItemChunk craftedItemChunk = craftedItem.GetComponent<ItemChunk>();
        RaycastHit hit;
        Physics.Raycast(craftedItem.transform.position, Vector3.down, out hit, 50f, groundLayer);
        craftedItemChunk.parentChunk = hit.transform;
    }

    public void Exit() {
        gui.Inventory();
    }
}
