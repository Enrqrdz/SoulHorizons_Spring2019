using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(AudioSource))]

public class scr_PlayerMovement : scr_EntityAI
{
    int inputX;
    int inputY;
    bool axisPressed = false; //used to get "OnJoystickDown"

    public override void Move()
    {

    }

    public override void UpdateAI()
    {
        MovementCheck();
    }
    public override void Die()
    {
    }

    void Start()
    {
    }

    void MovementCheck()
    {
        int _x = entity._gridPos.x;
        int _y = entity._gridPos.y;

        if(inputX != InputManager.MainHorizontal())
        {
            inputX = InputManager.MainHorizontal();
            _x += InputManager.MainHorizontal();
            axisPressed = true;
        }

        if (inputY != InputManager.MainVertical())
        {
            inputY = InputManager.MainVertical();
            _y += InputManager.MainVertical();
            axisPressed = true;
        }

        if (InputManager.MainHorizontal() == 0 && InputManager.MainVertical() == 0)
        {
            axisPressed = false;
        }

        if (scr_Grid.GridController.LocationOnGrid(_x, _y) &&  scr_Grid.GridController.ReturnTerritory(_x,_y).name == entity.entityTerritory.name)
        {
            entity.SetTransform(_x, _y);
        }
    }
}

