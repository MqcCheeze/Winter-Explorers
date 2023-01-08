using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] backgroundMusic;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate() {
        if (!audioSource.isPlaying) {
            PlayMusic();
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            PlayMusic();
        }    
    }

    private void PlayMusic() {
        audioSource.clip = backgroundMusic[Random.Range(0, backgroundMusic.Length)];
        audioSource.Play();
    }
}
