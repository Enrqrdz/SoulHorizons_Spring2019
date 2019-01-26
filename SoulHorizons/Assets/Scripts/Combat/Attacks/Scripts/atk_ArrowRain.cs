using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Attacks/ArrowRain")]
public class atk_ArrowRain : Attack {

    public int incrementWaitTime = 2; 

    public override Vector2Int BeginAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        scr_Grid.GridController.PrimeNextTile(xPos, yPos);
        scr_Grid.GridController.PrimeNextTile(xPos, yPos - 1);
        scr_Grid.GridController.PrimeNextTile(xPos, yPos - 2);
        scr_Grid.GridController.PrimeNextTile(xPos, yPos - 3);
        return new Vector2Int(xPos, yPos); 
    }
    public override Vector2Int ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        return ArrowRain_ProgressAttack(xPos, yPos, activeAtk);
    }

    Vector2Int ArrowRain_ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        if(activeAtk.currentIncrement < incrementWaitTime)
        {
            return new Vector2Int(xPos, yPos); 
        }

        scr_Grid.GridController.ActivateTile(xPos, yPos);
        return new Vector2Int(xPos, yPos - 1);
    }
    public override bool CheckCondition(scr_Entity _ent)
    {
        return true; 
    }

     //--Effects Methods--
    public override void LaunchEffects(ActiveAttack activeAttack)
    {
        activeAttack.particle = Instantiate(particle, scr_Grid.GridController.GetWorldLocation(activeAttack.position.x, activeAttack.position.y) + (new Vector3(0, 2.5f, 0)) + particlesOffset, Quaternion.identity);
        activeAttack.particle.sortingOrder = -activeAttack.position.y;
    }


    public override void ProgressEffects(ActiveAttack activeAttack)
    {
        if(activeAttack.currentIncrement < incrementWaitTime)
        {
            return; 
        }
        if (activeAttack.currentIncrement == incrementWaitTime)
        {
            activeAttack.particle.transform.position = scr_Grid.GridController.GetWorldLocation(activeAttack.position.x, activeAttack.position.y) + particlesOffset; 
        }
        activeAttack.particle.transform.position = Vector3.Lerp(activeAttack.particle.transform.position, scr_Grid.GridController.GetWorldLocation(activeAttack.lastPosition.x, activeAttack.lastPosition.y) + activeAttack._attack.particlesOffset, (4.5f) * Time.deltaTime);
    }

      public override void ImpactEffects(int xPos = -1, int yPos = -1)
    {

    }

    public override void EndEffects(ActiveAttack activeAttack)
    {

    }
}
