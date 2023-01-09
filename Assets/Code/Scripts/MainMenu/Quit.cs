using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    [Header("Animations")]                                  // Animations
    [SerializeField] private Animation mainMenuAnim;        // Main menu animation

    private AudioSource clickSound;

    private void Start() {
        clickSound = GetComponent<AudioSource>();
    }

    public void QuitGame() {                                // Quit the game
        StartCoroutine(QuitGameAnimation());
    }

    private IEnumerator QuitGameAnimation() {
        clickSound.Play();
        mainMenuAnim.Play("Deload");                        // Play main menu deload animation
        yield return new WaitForSeconds(0.3f);
        Application.Quit();                                 // Close program
    }
}
