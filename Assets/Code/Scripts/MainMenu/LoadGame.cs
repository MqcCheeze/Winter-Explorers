using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    [Header("Animations")]                                  // Animations
    [SerializeField] private Animation mainMenuAnim;        // Main menu animations

    private AudioSource clickSound;

    private void Start() {
        clickSound = GetComponent<AudioSource>();
    }

    public void LoadSavedGame() {                           // Load existing world in the save file
        StartCoroutine(LoadSavedGameAnimation());
    }

    private IEnumerator LoadSavedGameAnimation() {
        clickSound.Play();
        mainMenuAnim.Play("Deload");                        // Play the main menu deload animation
        yield return new WaitForSeconds(0.3f);
        GameSaveData.isNewSeed = false;                     // Say that the world isn't a new one
        SceneManager.LoadScene("Game");                     // Load game scene
    }
}
