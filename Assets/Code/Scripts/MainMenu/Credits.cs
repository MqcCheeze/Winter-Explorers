using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [Header("Panels")]                                      // Panels
    [SerializeField] private GameObject mainMenu;               // Menu panel
    [SerializeField] private GameObject credits;            // Credits panel

    [Header("Animations")]                                  // Animations
    [SerializeField] private Animation mainMenuAnim;        // Main menu animation
    [SerializeField] private Animation creditsAnim;         // Credits menu animation
    

    public void LoadCredits() {                             // Load credits menu
        StartCoroutine(LoadCreditsAnimation());
    }

    private IEnumerator LoadCreditsAnimation() {
        mainMenuAnim.Play("Deload");                        // Play main menu deload animation
        yield return new WaitForSeconds(0.25f);
        mainMenu.SetActive(false);                          // Hide main menu panel
        credits.SetActive(true);                            // Show credits menu panel
        creditsAnim.Play("Load");                           // Play credits menu load animation
    }

    public void CloseCredits() {                            // Load main menu panel
        StartCoroutine(LoadMenuAnimation());
    }

    private IEnumerator LoadMenuAnimation() {
        creditsAnim.Play("Deload");                         // Play credits menu deload animation
        yield return new WaitForSeconds(0.25f);
        credits.SetActive(false);                           // Hide credits menu panel
        mainMenu.SetActive(true);                           // Show main menu panel
        mainMenuAnim.Play("Load");                          // Play main menu load animation
    }
}
