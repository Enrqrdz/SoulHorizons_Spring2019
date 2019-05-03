using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]


public class RegionAudioManager : MonoBehaviour
{
    bool isInventoryClosed = false;
    AudioSource Music, Buttons;
    public AudioClip music, inventoryOpenSFX, inventoryAddSFX, inventoryCloseSFX;

    void Start()
    {
        AudioSource[] Audio_Sources = GetComponents<AudioSource>();
        Music = Audio_Sources[0];
        Buttons = Audio_Sources[1];
        Music.clip = music;
        Music.Play();
    }

    public void InventoryOpenCloseSFX()
    {
        isInventoryClosed = !isInventoryClosed;
        if (isInventoryClosed == true)
        {
            Buttons.clip = inventoryOpenSFX;
            Buttons.Play();
        }
        if (isInventoryClosed == false)
        {
            Buttons.clip = inventoryCloseSFX;
            Buttons.Play();
        }
    }

    public void InventoryAddSFX()
    {
        Buttons.clip = inventoryAddSFX;
        Buttons.Play();
    }
}