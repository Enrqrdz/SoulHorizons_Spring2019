using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]


public class backgroundMusic : MonoBehaviour {

    AudioSource Music, Buttons;
    public AudioClip music, buttonsSFX;

    void Start () {
        AudioSource[] Audio_Sources = GetComponents<AudioSource>();
        Music = Audio_Sources[0];
        Buttons = Audio_Sources[1];
        Music.clip = music;
        Music.Play();
    }

    public void ButtonSFX ()
    {
        Buttons.clip = buttonsSFX;
        Buttons.Play();
    }
	
}