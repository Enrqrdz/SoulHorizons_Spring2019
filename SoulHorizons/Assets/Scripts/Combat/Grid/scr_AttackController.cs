using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_AttackController : MonoBehaviour {

    public ActiveAttack[] activeAttacks = new ActiveAttack[20];     //Max number of active attacks is 20
    public int numberOfActiveAttacks = 0;
    public static scr_AttackController attackController;
    public scr_Pause pauseReference;

    private void Awake()
    {
        InitializeAttackController();
    }

    void Update()
    {
        IterateThroughActiveAttacks();
    }

    private void InitializeAttackController()
    {
        attackController = this;
    }

    private void IterateThroughActiveAttacks()
    {
        for (int i = 0; i < numberOfActiveAttacks; i++)
        {
            bool attackIsAtMaxRange = activeAttacks[i].currentIncrement > activeAttacks[i].attack.maxIncrementRange;
            bool attackIsOnFinalTarget = !activeAttacks[i].attack.hasPiercing && activeAttacks[i].entityIsHit;
            bool attackIsOnGrid = scr_Grid.GridController.LocationOnGrid(activeAttacks[i].position.x, activeAttacks[i].position.y) == false;
            bool attackHasMoved = activeAttacks[i].currentIncrement != 0;

            if (activeAttacks[i].CanAttackContinue())
            {
                if (attackIsAtMaxRange || attackIsOnFinalTarget || attackIsOnGrid)
                {
                    RemoveFromArray(i);
                    return;
                }

                if (attackHasMoved)
                {
                    scr_Grid.GridController.DeactivateTile(activeAttacks[i].lastPosition.x, activeAttacks[i].lastPosition.y);
                }

                activeAttacks[i].lastPosition = activeAttacks[i].position;
                activeAttacks[i].Clone(scr_Grid.GridController.AttackPosition(activeAttacks[i]));
                activeAttacks[i].position = activeAttacks[i].attack.ProgressAttack(activeAttacks[i].position.x, activeAttacks[i].position.y, activeAttacks[i]);
                activeAttacks[i].lastAttackTime = Time.time;
                activeAttacks[i].currentIncrement++;
            }
            activeAttacks[i].attack.ProgressEffects(activeAttacks[i]);
        }
    }

    public void AddNewAttack(AttackData _attack, int xPos, int yPos, scr_Entity ent)
    {
        activeAttacks[numberOfActiveAttacks] = new ActiveAttack(_attack, xPos, yPos, ent);
        activeAttacks[numberOfActiveAttacks].attack.BeginAttack(xPos, yPos, activeAttacks[numberOfActiveAttacks]);
        activeAttacks[numberOfActiveAttacks].Clone(activeAttacks[numberOfActiveAttacks].attack.BeginAttack(activeAttacks[numberOfActiveAttacks]));

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


    void RemoveFromArray(int index)
    {
        //Attack end effects
        activeAttacks[index].attack.EndEffects(activeAttacks[index]);

        scr_Grid.GridController.DeactivateTile(activeAttacks[index].lastPosition.x, activeAttacks[index].lastPosition.y);
        scr_Grid.GridController.DeactivateTile(activeAttacks[index].position.x, activeAttacks[index].position.y);
        scr_Grid.GridController.DePrimeTile(activeAttacks[index].position.x, activeAttacks[index].position.y);
        Destroy(activeAttacks[index].particle);
        for (int x = index; x < numberOfActiveAttacks; x++)
        {
            if (x + 1 < activeAttacks.Length && activeAttacks[x + 1].attack != null)
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

    public AttackData AttackType(Vector2Int pos)
    {
        for (int x = 0; x < numberOfActiveAttacks; x++)
        {
            if (activeAttacks[x].position == pos)
            {
                return activeAttacks[x].attack;
            }
        }
        
        return null; 
        
    }

    public AttackData MoveIntoAttackCheck(Vector2Int pos, scr_Entity entity)
    {
        for (int x = 0; x < numberOfActiveAttacks; x++)
        {
            if (activeAttacks[x].lastPosition == pos)
            {
                if (activeAttacks[x].entity.type != entity.type)
                {
                    AttackData atk = activeAttacks[x].attack;
                    activeAttacks[x].entityIsHit = true;
                    return atk;
                }
            }
        }

        return null;
    }
}