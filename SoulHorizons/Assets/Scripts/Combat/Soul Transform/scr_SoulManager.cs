﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(AudioSource))]

/// <summary>
/// Update the UI, take in user input, and manage information on the available soul transforms
/// </summary>
public class scr_SoulManager : MonoBehaviour {

    public List<SoulTransform> soulTransforms = new List<SoulTransform>();
    public  List<Button> buttons = new List<Button>();
    private SoulTransform currentTransform = null;
    private bool transformed = false;
    private Entity player;
    public ActionManager deckManager;
    Animator anim; //animator to control soul transform animations

    private IDictionary<Element, int> soulCharges = new Dictionary<Element, int>(); 
    private IDictionary<Element, Button> elementButtons = new Dictionary<Element, Button>();
    //TODO: need to get references to the UI buttons so they can be updated with sprites and animations can occur when they get chaged

    //--Art assets--
    public Sprite[] earth_soulOrb = new Sprite[4]; //an array of the different sprites for the soul orb based on charge

    AudioSource Transform_SFX;
    public AudioClip transform_SFX;

    void Start () {
        //find the player
        GameObject p = ObjectReference.Instance.Player;
        player = p.GetComponent<Entity>();
        anim = ObjectReference.Instance.Player.GetComponentInChildren<Animator>();

        //TODO: Set all the button sprites according to the soul transforms given

        //add an on click to the button that triggers the transform
        int i = 0;
        foreach (SoulTransform item in soulTransforms)
        {
            //add the transformation code to the button onclick event
            buttons[i].onClick.AddListener(delegate {Transformation(item); });

            //add the components in the soulTransform to the player
             MonoBehaviour[] scripts = item.scriptHolder.GetComponents<MonoBehaviour>();
             foreach (MonoBehaviour script in scripts)
             {
                 MonoBehaviour s = CopyComponent<MonoBehaviour>(script, player.gameObject); //copy the values from the prefab; needed for particle references
                 s.enabled = false;
             }

            //add the button to the dictionary with the transform's element as the key
            elementButtons[item.element] = buttons[i];

            i++;
        }
        //when this is done, each button should have an on click event which triggers one of the transforms
        
        //initialize all of the charges to 0; this loop goes through all of the elements in the Element enum
        foreach (Element e in (Element[])System.Enum.GetValues(typeof(Element)))
        {
            soulCharges[e] = 0;
        }

        //Temporary Start off with Charge
        soulCharges[Element.Earth] = 100;
        elementButtons[Element.Earth].GetComponent<Image>().sprite = earth_soulOrb[3];

    }
	
	void Update () {
		UserInput();
	}

    /// <summary>
    /// Get keyboard input
    /// </summary>
    private void UserInput()
    {   
        //this is used only for keyboard input
        //TODO: need to take charge into account once we start charging these
        switch (InputManager.K_SoulFusion())
        {
            case 1:
                buttons[0].onClick.Invoke();
                break;
            case 2:
                buttons[1].onClick.Invoke();
                break;
            case 3:
                buttons[2].onClick.Invoke();
                break;
            default:
                break; //returned 0, so none were pressed
        }
    }

    /// <summary>
    /// Replace the default list of transforms with passed-in arguments. Use this on start when we start using save data to set the transforms
    /// </summary>
    /// <param name="transforms"></param>
    public void SetSoulTransforms(params SoulTransform[] transforms)
    {
        soulTransforms = new List<SoulTransform>(transforms.Length);
    }

    /// <summary>
    /// This will be called by the deck manager when a card is cast to charge the corresponding element
    /// </summary>
    /// <param name="e"></param>
    /// <param name="amount"></param>
    public void ChargeSoulTransform(Element e, int amount)
    {
        if (amount > 0)
        {
            soulCharges[e] += amount;
            //TODO: trigger any UI effects that come with charging a soul transform
            if (soulCharges[e] >= 100)
            {
                soulCharges[e] = 100;
                //change the corresponding soul button to indicate that it is full
                if(e == Element.Earth) elementButtons[e].GetComponent<Image>().sprite = earth_soulOrb[3];
            }
            else if (soulCharges[e] >= 70)
            {
                if(e == Element.Earth) elementButtons[e].GetComponent<Image>().sprite = earth_soulOrb[2];
            }
            else if (soulCharges[e] >= 40)
            {
                if(e == Element.Earth) elementButtons[e].GetComponent<Image>().sprite = earth_soulOrb[1];
            }
            else
            {
                if(e == Element.Earth) elementButtons[e].GetComponent<Image>().sprite = earth_soulOrb[0];
            }
        }
    }

    /// <summary>
    /// This should be added to the corresponding button to listen for the onClick event.
    /// This method performs the transformation process with the given argument
    /// </summary>
    /// <param name="soul"></param>
    private void Transformation(SoulTransform soul)
    {
        //TODO: Should enable/disable the buttons based on this validation, so this method is only called under valid circumstances
        if (transformed || soulCharges[soul.element] < 100)
        {
            return; //don't transform if the player is already transformed or the element is not charged
        }
        AudioSource Transform_SFX = GetComponent<AudioSource>();
        Transform_SFX.clip = transform_SFX;
        Transform_SFX.Play();
        anim.SetBool("BearTransform", true);

        //disable the deck system
        deckManager.DisableBasicActions(true);


        //reduce the charge
        soulCharges[soul.element] -= 50; //reduce to 50%
        elementButtons[soul.element].GetComponent<Image>().sprite = earth_soulOrb[1];


        //disable the player attack and movement
        player.gameObject.GetComponent<scr_PlayerMovement>().enabled = false;


        MonoBehaviour[] scripts = soul.scriptHolder.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            MonoBehaviour s = (MonoBehaviour) player.gameObject.GetComponent(script.GetType());
            s.enabled = true;
        }

        //add the shield to the player
        player._health.shield += (player._health.max_hp * soul.GetShieldGain()) / 100; //increase shield by <shieldGain> % of max health

        currentTransform = soul;
        transformed = true;
        StartCoroutine(ShieldDrain());
    }

    /// <summary>
    /// Called when the player's shield runs out and they revert back to their default state
    /// </summary>
    public void EndTransformation()
    {
        Debug.Log("End Transformation Start");
        anim.SetBool("BearTransform", false);

        //enable the deck system
        deckManager.DisableBasicActions(false);

        MonoBehaviour[] scripts = currentTransform.scriptHolder.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            MonoBehaviour s = (MonoBehaviour) player.gameObject.GetComponent(script.GetType());
            s.enabled = false;
        }

        //enable the player default attack and movement
        player.gameObject.GetComponent<scr_PlayerMovement>().enabled = true;

        transformed = false;
        Debug.Log("End Transformation End");
    }

    /// <summary>
    /// Started when the player transforms, continues as long as the player is transformed and still has a shield
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShieldDrain()
    {
        while (transformed)
        {
            yield return new WaitForSeconds(1f);
            //decrement the shield using currentTransform
            player._health.shield -= currentTransform.GetShieldDrainRate();

            //end the transformation if the shield hits 0
            if (player._health.shield <= 0)
            {
                player._health.shield = 0;
                EndTransformation();
            }
        }
    }

    public void CopyClassValues(MonoBehaviour sourceComp, MonoBehaviour targetComp)
    {
        FieldInfo[] sourceFields = sourceComp.GetType().GetFields(BindingFlags.Public | 
                                                           BindingFlags.NonPublic | 
                                                           BindingFlags.Instance);
        int i = 0;
       for(i = 0; i < sourceFields.Length; i++)
        {
         var value = sourceFields[i].GetValue(sourceComp);
         sourceFields[i].SetValue(targetComp, value);
        }
     }

      T CopyComponent<T>(T original, GameObject destination) where T : Component
        {
            System.Type type = original.GetType();
            Component copy = destination.AddComponent(type);
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }
            return copy as T;
        }

}
