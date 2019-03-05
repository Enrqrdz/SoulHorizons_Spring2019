using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Test Comment to be able to see if this works
public class scr_CursorManager : MonoBehaviour
{
    void Update()
    {
        LockCursor();
        HideCursor();
    }


    private static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private static void HideCursor()
    {
        Cursor.visible = false;
    }
}
