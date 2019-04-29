using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_AnimController : MonoBehaviour {

    public Animator anim;
    //ANIMATION METHODS
    public void fadeIn()
    {
        if (anim != null)
        {
            anim.SetInteger("Movement", -1);
        }
    }

    public void backToIdle()
    {
        if (anim != null)
        {
            anim.SetInteger("Movement", 0);
        }
    }

    public void doneCast()
    {
        if (anim != null)
        {
            anim.SetBool("Cast", false);
        }
    }

    public void doneAttack()
    {
        if (anim != null)
        {
            anim.SetBool("Attack", false);
        }
    }

    public void doneAttack2()
    {
        if (anim != null)
        {
            anim.SetBool("Attack2", false);
        }
    }

    public void doneHit()
    {
        if (anim != null)
        {
            anim.SetBool("Hit", false);
        }
    }
}
