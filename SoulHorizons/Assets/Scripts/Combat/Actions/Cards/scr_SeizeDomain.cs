using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Cards/SeizeDomain")]
[RequireComponent(typeof(AudioSource))]

public class scr_SeizeDomain : ActionData
{
    public float duration;
    private AudioSource PlayCardSFX;
    public AudioClip SeizeDomainSFX;
    private int seizedColumnNumber;


    public override void Activate()
    {    
        PlayCardSFX = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = SeizeDomainSFX;
        PlayCardSFX.Play();
        DomainManager.Instance.Activate(duration);

    }

    

    public override void Project()
    {
        for(int i = 0; i < DomainManager.Instance.numberOfRows; i++)
        {
            seizedColumnNumber = DomainManager.Instance.columnToBeSeized;

            scr_Grid.GridController.grid[seizedColumnNumber, i].Highlight();
        }
        
    }

    public override void DeProject()
    {
        for (int i = 0; i < DomainManager.Instance.numberOfRows; i++)
        { 
            scr_Grid.GridController.grid[seizedColumnNumber, i].DeHighlight();
        }
    }
}
