using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChunk : MonoBehaviour
{
    public Transform parentChunk;

    private void Start() {
        parentChunk = transform.parent;
    }
}
