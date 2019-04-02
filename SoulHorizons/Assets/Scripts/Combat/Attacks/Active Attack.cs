using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActiveAttack
{
    public AttackData attack;
    public Vector2Int position;
    public Vector2Int lastPosition;
    public float lastAttackTime;
    public int currentIncrement = 0;

    public Entity entity;
    public Entity entityHit = null; //contains a reference to the entity that the attack hit
    public bool entityIsHit = false;    //set to true if the attack hits an entity

    public SpriteRenderer particle;     // use if only one particle 
    public SpriteRenderer[] particles;  //use for multiple particles 

    public ActiveAttack(AttackData atk, int x, int y, Entity ent)
    {
        particles = new SpriteRenderer[10];
        attack = atk;
        position.x = x;
        position.y = y;
        entity = ent;
        lastPosition.x = x;
        lastPosition.y = y;

        lastAttackTime = Time.time - attack.incrementTime;
    }

    public ActiveAttack()
    {
        attack = null;
        position = new Vector2Int();
        lastAttackTime = 0;
        currentIncrement = 0;
    }

    public bool CanAttackContinue()
    {
        if (lastAttackTime + attack.incrementTime <= Time.time)
        {
            return true;
        }
        return false;
    }

    public void Clone(ActiveAttack atk)
    {
        attack = atk.attack;
        position = atk.position;
        lastAttackTime = atk.lastAttackTime;
        currentIncrement = atk.currentIncrement;
        lastPosition = atk.lastPosition;
        entity = atk.entity;
        entityIsHit = atk.entityIsHit;
        particle = atk.particle;
        particles = atk.particles;

    }
}