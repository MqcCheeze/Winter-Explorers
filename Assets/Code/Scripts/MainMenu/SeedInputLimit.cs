using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SeedInputLimit : MonoBehaviour
{
    TMP_InputField seedInput;

    private void Start() {
        seedInput = GetComponent<TMP_InputField>();
    }

    private void FixedUpdate() {
        if (!string.IsNullOrEmpty(seedInput.text)) {
            if (Convert.ToInt64(seedInput.text) > Int32.MaxValue) {
                seedInput.text = "2147483647";
            }
        }
    }
}