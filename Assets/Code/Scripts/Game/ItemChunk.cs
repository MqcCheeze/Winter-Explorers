using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChunk : MonoBehaviour
{
    public Transform parentChunk;

    private void Awake() {
        parentChunk = transform.parent;
        if (parentChunk.CompareTag("Chest")) {
            parentChunk = parentChunk.transform.parent;
        }
    }
}
