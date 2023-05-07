using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{

    public event EventHandler<KeyPressed> PlayerInteractions;

    public class KeyPressed {
        public string keyPressed;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            Debug.Log("E pressed");
            PlayerInteractions?.Invoke(this, new KeyPressed {
                keyPressed = "e"
            });
        }
        if (Input.GetKeyDown(KeyCode.Tab)) {
            Debug.Log("Tab pressed");
            PlayerInteractions?.Invoke(this, new KeyPressed {
                keyPressed = "tab"
            });
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Debug.Log("Escape pressed");
            PlayerInteractions?.Invoke(this, new KeyPressed {
                keyPressed = "escape"
            });
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            Debug.Log("Q pressed");
            PlayerInteractions?.Invoke(this, new KeyPressed {
                keyPressed = "q"
            });
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
            Debug.Log("Scrolled back");
            PlayerInteractions?.Invoke(this, new KeyPressed {
                keyPressed = "mouseBack"
            });
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
            Debug.Log("Scrolled forward");
            PlayerInteractions?.Invoke(this, new KeyPressed {
                keyPressed = "mouseForward"
            });
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            Debug.Log("Left shift pressed");
            PlayerInteractions?.Invoke(this, new KeyPressed {
                keyPressed = "lShift"
            });
        }
        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            Debug.Log("Left control pressed");
            PlayerInteractions?.Invoke(this, new KeyPressed {
                keyPressed = "lControl"
            });
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Space pressed");
            PlayerInteractions?.Invoke(this, new KeyPressed {
                keyPressed = "space"
            });
        }
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("LMB pressed");
            PlayerInteractions?.Invoke(this, new KeyPressed {
                keyPressed = "lMouse"
            });
        }
        if (Input.GetMouseButtonDown(1)) {
            Debug.Log("RMB pressed");
            PlayerInteractions?.Invoke(this, new KeyPressed {
                keyPressed = "rMouse"
            });
        }
    }
}