using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class scr_EntityAI : MonoBehaviour {

    public Entity entity;
    public Animator anim;

    public abstract void Move();

    public abstract void UpdateAI();

    public abstract void Die();

    public void PrimeAttackTiles (AttackData attack, int xPos, int yPos)
    {
        int num = 0; ;
        for (int i = 0; i < attack.maxIncrementRange; i++)
        {
            
            if (scr_Grid.GridController.GetEntityAtPosition(xPos - 1 - i, yPos) == null || (scr_Grid.GridController.GetEntityAtPosition(xPos - 1 - i, yPos).type == EntityType.Player))
            {
                scr_Grid.GridController.PrimeNextTile(xPos - 1 - i, yPos);
                num = xPos - 1 - i;
            }
            else
            {
                StartCoroutine(DePrimeAttackTiles(attack, num, yPos));
                break;
            }
        }
    }

    private IEnumerator DePrimeAttackTiles (AttackData attack, int startPoint, int yPos)
    {
        Debug.Log("We Deprimin Bois");
        yield return new WaitForSeconds(attack.incrementTime * scr_Grid.GridController.rowSizeMax/2);
        for(int i = 0; i < attack.maxIncrementRange; i++)
        {
            scr_Grid.GridController.DePrimeTile(startPoint + i, yPos);
        }
    }
 
}
