using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActiveAttack
{
    public Attack _attack;
    public Vector2Int pos;
    public Vector2Int lastPos;
    public float lastAttackTime;
    public int currentIncrement = 0;
    public scr_Entity entity;
    public bool entityIsHit = false; //set to true if the attack hits an entity
    public scr_Entity entityHit = null; //contains a reference to the entity that the attack hit
    public SpriteRenderer particle; // use if only one particle 
    public SpriteRenderer[] particles; //use for multiple particles 

    public ActiveAttack(Attack atk, int x, int y, scr_Entity ent)
    {
        particles = new SpriteRenderer[5];
        _attack = atk;
        pos.x = x;
        pos.y = y;
        entity = ent;
        lastPos.x = x;
        lastPos.y = y;

        lastAttackTime = Time.time - _attack.incrementSpeed;
    }

    public ActiveAttack()
    {
        _attack = null;
        pos = new Vector2Int();
        lastAttackTime = 0;
        currentIncrement = 0;
    }

    public bool CanAttackContinue()
    {
        if (lastAttackTime + _attack.incrementSpeed <= Time.time)
        {
            return true;
        }
        return false;
    }

    public void Clone(ActiveAttack atk)
    {
        _attack = atk._attack;
        pos = atk.pos;
        lastAttackTime = atk.lastAttackTime;
        currentIncrement = atk.currentIncrement;
        lastPos = atk.lastPos;
        entity = atk.entity;
        entityIsHit = atk.entityIsHit;
        particle = atk.particle;
        particles = atk.particles;

    }
}