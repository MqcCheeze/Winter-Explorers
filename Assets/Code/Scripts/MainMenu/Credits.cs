using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [Header("Animations")]
    public Animation menuAnim;
    public Animation creditsAnim;
    public GameObject menu;                         // Menu panel
    public GameObject credits;                      // Credits panel

    public void CloseCredits() {
        StartCoroutine(LoadMenuAnimation());
    }


    private IEnumerator LoadMenuAnimation() {
        creditsAnim.Play("Deload");
        yield return new WaitForSeconds(0.25f);
        credits.SetActive(false);                   // Hide credits panel
        menu.SetActive(true);                       // Show menu panel
        menuAnim.Play("Load");
    }
}
