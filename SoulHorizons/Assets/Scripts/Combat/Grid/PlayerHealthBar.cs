using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : HealthBar
{
    public override void OnStart()
    {
        targetEntity = ObjectReference.Instance.Player.GetComponentInChildren<Entity>();

        if(targetEntity == null)
        {
            Invoke("OnStart", .5f);
        }
    }
}
