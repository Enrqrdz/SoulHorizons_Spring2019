using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class scr_EntityAI : MonoBehaviour {

    public scr_Entity entity;
    public Animator anim;

    public abstract void Move();

    public abstract void Attack();

    public abstract void UpdateAI();

    public abstract void Die();
}
