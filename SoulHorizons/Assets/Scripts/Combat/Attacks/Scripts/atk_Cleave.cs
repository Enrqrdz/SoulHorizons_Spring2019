using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Cleave")]
public class atk_Cleave : AttackData
{
    Entity player;
    int playerX;
    int playerY;

    public override Vector2Int BeginAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        }
        playerX = player._gridPos.x;
        playerY = player._gridPos.y;

        return new Vector2Int(xPos, yPos); 
    }
    public override Vector2Int ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
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
