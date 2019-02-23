using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Attacks/PlayerBasic")]
public class atk_PlayerBasic : AttackData {

    


    public override Vector2Int ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        return LinearForward_ProgressAttack(xPos,yPos, activeAtk); 
    }

    Vector2Int LinearForward_ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        //move particles
        //ProgressEffects(xPos, yPos, activeAtk.lastPos.x, activeAtk.lastPos.y, activeParticle);

        scr_Grid.GridController.PrimeNextTile(xPos + 1,yPos);
        scr_Grid.GridController.ActivateTile(xPos, yPos); 
        return new Vector2Int(xPos + 1, yPos); 
    }
    public override bool CheckCondition(Entity _ent)
    {

        //return Random.Range(0, 100) < chanceToAttack;
       
            return true;
            //TODO charging attack 
        
        
    }

    //--Effects Methods--
    public override void LaunchEffects(ActiveAttack activeAttack)
    {
        activeAttack.particle = Instantiate(particles, scr_Grid.GridController.GetWorldLocation(activeAttack.position.x, activeAttack.position.y) + particlesOffset, Quaternion.identity);
        activeAttack.particle.sortingOrder = -activeAttack.position.y;
    }

    public override void ProgressEffects(ActiveAttack activeAttack)
    {
        activeAttack.particle.transform.position = Vector3.Lerp(activeAttack.particle.transform.position, scr_Grid.GridController.GetWorldLocation(activeAttack.lastPosition.x,activeAttack.lastPosition.y) + activeAttack.attack.particlesOffset, (4.5f) * Time.deltaTime);
    }

    public override void ImpactEffects(int xPos = -1, int yPos = -1)
    {

    }

    public override void EndEffects(ActiveAttack activeAttack)
    {

    }
}
