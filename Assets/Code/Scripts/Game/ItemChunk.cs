using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChunk : MonoBehaviour
{
    public Transform parentChunk;                           // The chunk the object is in

    private void Awake() {
        try {
            parentChunk = transform.parent;                     // Set the parent chunk to the item's parent
            if (parentChunk.CompareTag("Chest")) {              // If the item was in the chest
                parentChunk = parentChunk.transform.parent;     // Set the parent chunk to the parent of the chest
            }
        } catch {

        }
    }
}
