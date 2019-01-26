using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActiveAttack
{
    public Attack _attack;
    public Vector2Int position;
    public Vector2Int lastPosition;
    public float lastAttackTime;
    public int currentIncrement = 0;
    public scr_Entity SourceEntity;
    public bool isEntityHit = false; //set to true if the attack hits an entity
    public scr_Entity TargetEntity = null; //contains a reference to the entity that the attack hit
    public SpriteRenderer particle; // use if only one particle 
    public List<SpriteRenderer> particles = new List<SpriteRenderer>();

    public ActiveAttack(Attack atk, int x, int y, scr_Entity ent)
    {
        _attack = atk;
        position.x = x;
        position.y = y;
        SourceEntity = ent;
        lastPosition.x = x;
        lastPosition.y = y;

        lastAttackTime = Time.time - _attack.incrementTime;
    }

    public ActiveAttack()
    {
        _attack = null;
        position = new Vector2Int();
        lastAttackTime = 0;
        currentIncrement = 0;
    }

    public bool CanAttackContinue()
    {
        if (lastAttackTime + _attack.incrementTime <= Time.time)
        {
            return true;
        }
        return false;
    }

    public void Clone(ActiveAttack atk)
    {
        _attack = atk._attack;
        position = atk.position;
        lastAttackTime = atk.lastAttackTime;
        currentIncrement = atk.currentIncrement;
        lastPosition = atk.lastPosition;
        SourceEntity = atk.SourceEntity;
        isEntityHit = atk.isEntityHit;
        particle = atk.particle;
    }
}
