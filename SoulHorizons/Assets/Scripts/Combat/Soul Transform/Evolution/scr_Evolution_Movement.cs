using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class scr_Evolution_Movement : scr_PlayerMovement {
    //Note: This is the same as normal movement right now, but I think we would have issues if an extra copy of the same script was added to the player object by the soul manager

    public void Start()
    {
        //fill the entity reference
        entity = gameObject.GetComponent<scr_Entity>();
    }
}
