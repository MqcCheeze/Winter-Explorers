using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnPlay : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);    // Hide the object when the scene starts
    }
}
