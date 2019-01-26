using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : ScriptableObject {

    public float incrementTime;
    public int maxIncrements = 1;
    public int damage;
    [Header("Where the attack is coming from")]
    public EntityType type; //Player, Enemy, or Obstacle
    public bool piercing;

    public SpriteRenderer particle;
    public Vector3 particlesOffset;
    public float particleSpeed = 1;

    public virtual Vector2Int BeginAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        return new Vector2Int(); 
    }

    public virtual ActiveAttack BeginAttack(ActiveAttack activeAtk)
    {
        return activeAtk; 
    }

    public virtual Vector2Int ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        return new Vector2Int(); 
    }

    public virtual bool CheckCondition(scr_Entity _ent)
    {
        return false; 
    }
    
    public abstract void LaunchEffects(ActiveAttack activeAttack);

    public abstract void ProgressEffects(ActiveAttack activeAttack);

    public abstract void ImpactEffects(int xPos = -1, int yPos = -1);

    public abstract void EndEffects(ActiveAttack activeAttack);
}
