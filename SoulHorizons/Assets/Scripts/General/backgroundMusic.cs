using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]


public class backgroundMusic : MonoBehaviour {

    AudioSource Music, Buttons;
    public AudioClip music, button_SFX;
    public AudioClip[] buttons_SFX;

    void Start () {
        AudioSource[] Audio_Sources = GetComponents<AudioSource>();
        Music = Audio_Sources[0];
        Buttons = Audio_Sources[1];
        Music.clip = music;
        Music.Play();
    }

    public void ButtonSFX ()
    {
        int index = Random.Range(0, buttons_SFX.Length);
        button_SFX = buttons_SFX[index];
        Buttons.clip = button_SFX;
        Buttons.Play();
    }
	
}