using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Cleave")]
public class atk_Cleave : Attack {

    public override Vector2Int BeginAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        scr_Grid.GridController.PrimeNextTile(xPos, yPos);
        scr_Grid.GridController.PrimeNextTile(xPos, yPos + 1);
        scr_Grid.GridController.PrimeNextTile(xPos, yPos + 2);
        return new Vector2Int(xPos, yPos); 
    }
    public override Vector2Int ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        return Cleave_ProgressAttack(xPos, yPos, activeAtk);
    }

    Vector2Int Cleave_ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        scr_Grid.GridController.ActivateTile(xPos, yPos);
        return new Vector2Int(xPos, yPos + 1);
    }

	//--Effects Methods--
    public override void LaunchEffects(ActiveAttack activeAttack)
    {
		activeAttack.particle = Instantiate(particles, scr_Grid.GridController.GetWorldLocation(activeAttack.position.x, activeAttack.position.y) + particlesOffset, particles.transform.rotation);
        activeAttack.particle.sortingOrder = -activeAttack.position.y;
    }

    public override void ImpactEffects(int xPos = -1, int yPos = -1)
    {

    }

    public override void ProgressEffects(ActiveAttack activeAttack)
    {
        //rotate the scythe
		activeAttack.particle.transform.Rotate(Vector3.forward, 10f);
    }
    public override void EndEffects(ActiveAttack activeAttack)
    {
        
    }
}
