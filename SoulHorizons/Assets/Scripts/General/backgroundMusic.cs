using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]


public class backgroundMusic : MonoBehaviour {

    AudioSource Music;

    void Start () {
        AudioSource Music = GetComponent<AudioSource>();
        Music.Play();
    }
	
}
