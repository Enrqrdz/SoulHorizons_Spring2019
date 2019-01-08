using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_AttackController : MonoBehaviour {

    public ActiveAttack[] activeAttacks = new ActiveAttack[10];
    public int numberOfActiveAttacks = 0; 
    public static scr_AttackController attackController;
    public scr_Pause pauseReference;

    private void Awake()
    {
        attackController = this; 
    }

    public void AddNewAttack(Attack _attack,int xPos, int yPos, scr_Entity ent)
    {
        activeAttacks[numberOfActiveAttacks] = new ActiveAttack(_attack, xPos, yPos, ent);
        activeAttacks[numberOfActiveAttacks]._attack.BeginAttack(xPos, yPos, activeAttacks[numberOfActiveAttacks]);
        activeAttacks[numberOfActiveAttacks].Clone(activeAttacks[numberOfActiveAttacks]._attack.BeginAttack(activeAttacks[numberOfActiveAttacks]));
        
        /*
        activeAttacks[numberOfActiveAttacks].particle = Instantiate(_attack.particles, scr_Grid.GridController.GetWorldLocation(xPos,yPos)+_attack.particlesOffset, Quaternion.identity);
        activeAttacks[numberOfActiveAttacks].particle.sortingOrder = -yPos; 
         */

        //Start effects for when the attack is created
        if (_attack == null)
        {
            Debug.Log("AttackController: attack is null");
        }
        if (activeAttacks[numberOfActiveAttacks] == null)
        {
            Debug.Log("AttackController: attack is null");
        }
        _attack.LaunchEffects(activeAttacks[numberOfActiveAttacks]);
        numberOfActiveAttacks++;
        
    }

    void Update()
    {
        for (int x = 0; x < numberOfActiveAttacks; x++)
        {
            if (activeAttacks[x].CanAttackContinue())
            {
                if (activeAttacks[x].currentIncrement > activeAttacks[x]._attack.maxIncrements)
                {
                    RemoveFromArray(x);
                    return;
                }
                else if (!activeAttacks[x]._attack.piercing && activeAttacks[x].entityIsHit)
                {
                    RemoveFromArray(x);
                    return;
                }
                else if (scr_Grid.GridController.LocationOnGrid(activeAttacks[x].pos.x, activeAttacks[x].pos.y) == false)
                {
                    //Debug.Log("location off grid " + activeAttacks[x]._attack.name); 
                    RemoveFromArray(x);
                    return;
                }

                if (activeAttacks[x].currentIncrement != 0)
                {
                    scr_Grid.GridController.DeactivateTile(activeAttacks[x].lastPos.x, activeAttacks[x].lastPos.y);
                }
                activeAttacks[x].lastPos = activeAttacks[x].pos;
                activeAttacks[x].Clone(scr_Grid.GridController.AttackPosition(activeAttacks[x]));
                activeAttacks[x].pos = activeAttacks[x]._attack.ProgressAttack(activeAttacks[x].pos.x, activeAttacks[x].pos.y, activeAttacks[x]);
                activeAttacks[x].lastAttackTime = Time.time;
                activeAttacks[x].currentIncrement++;

            }
            //activeAttacks[x].particle.transform.position = Vector3.Lerp(activeAttacks[x].particle.transform.position, scr_Grid.GridController.GetWorldLocation(activeAttacks[x].lastPos.x,activeAttacks[x].lastPos.y) + activeAttacks[x]._attack.particlesOffset, (4.5f) * Time.deltaTime);
            //Replaced this lerp by passing the particle to progress attack above and letting the attack object determine particle behavior - Colin
            activeAttacks[x]._attack.ProgressEffects(activeAttacks[x]);
        }
    }
    void RemoveFromArray(int index)
    {
        //Attack end effects
        activeAttacks[index]._attack.EndEffects(activeAttacks[index]);

        scr_Grid.GridController.DeactivateTile(activeAttacks[index].lastPos.x, activeAttacks[index].lastPos.y);
        scr_Grid.GridController.DeactivateTile(activeAttacks[index].pos.x, activeAttacks[index].pos.y);
        scr_Grid.GridController.DePrimeTile(activeAttacks[index].pos.x, activeAttacks[index].pos.y);
        Destroy(activeAttacks[index].particle);
        for (int x = index; x < numberOfActiveAttacks; x++)
        {
            if (x + 1 < activeAttacks.Length && activeAttacks[x + 1]._attack != null)
            {
                activeAttacks[x].Clone(activeAttacks[x + 1]);
            }
            else
            {
                activeAttacks[x] = new ActiveAttack();
            }
        }
        numberOfActiveAttacks--; 
    }

    public Attack AttackType(Vector2Int pos)
    {
        for (int x = 0; x < numberOfActiveAttacks; x++)
        {
            if (activeAttacks[x].pos == pos)
            {
                return activeAttacks[x]._attack;
            }
        }
        
        return null; 
        
    }

    public Attack MoveIntoAttackCheck(Vector2Int pos, scr_Entity entity)
    {
        for (int x = 0; x < numberOfActiveAttacks; x++)
        {
            if (activeAttacks[x].lastPos == pos)
            {
                if (activeAttacks[x].entity.type != entity.type)
                {
                    Attack atk = activeAttacks[x]._attack;
                    activeAttacks[x].entityIsHit = true;
                    return atk;
                }
            }
        }

        return null;
    }

}

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
        if(lastAttackTime + _attack.incrementSpeed <= Time.time)
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
