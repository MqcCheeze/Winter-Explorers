using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    [SerializeField] private PlayerCrafting inventory;
    [SerializeField] private GameObject item;
    [SerializeField] private RawImage itemPreviewImage;
    [SerializeField] private RawImage itemImage;

    public void SelectItemToCraft() {
        inventory.itemToCraft = item;
        itemPreviewImage.gameObject.SetActive(true);
        itemPreviewImage.texture = itemImage.texture;
    }
}