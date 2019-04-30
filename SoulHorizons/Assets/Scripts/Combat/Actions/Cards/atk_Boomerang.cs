using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Attacks/Boomerang")]
[RequireComponent(typeof(AudioSource))]

public class atk_Boomerang : AttackData
{
    private AudioSource PlayCardSFX;
    public AudioClip BoomerangSFX;
    Entity player;

    public int boomerangForwardDistance = 3;
    int playerX;
    int playerY;
    bool backwards;

    public override Vector2Int BeginAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        return new Vector2Int(xPos, yPos);
    }
    public override ActiveAttack BeginAttack(ActiveAttack activeAtk)
    {
        activeAtk.particle = Instantiate(particles, scr_Grid.GridController.GetWorldLocation(activeAtk.position),Quaternion.identity);

        player = ObjectReference.Instance.PlayerEntity;
        playerX = player._gridPos.x;
        playerY = player._gridPos.y;
        backwards = false;

        if (PlayCardSFX == null)
        {
            PlayCardSFX = ObjectReference.Instance.ActionManager;
        }

        PlayCardSFX.clip = BoomerangSFX;
        PlayCardSFX.Play();
        activeAtk.attack.maxIncrementRange = ((scr_Grid.GridController.columnSizeMax * 2) + scr_Grid.GridController.rowSizeMax - 2); 
        return activeAtk;
    }

    public override bool CheckCondition(Entity _ent)
    {
        return false;
    }

    public override void EndEffects(ActiveAttack activeAttack)
    {
       
    }

    public override void ImpactEffects(int xPos = -1, int yPos = -1)
    {
    }

    public override void LaunchEffects(ActiveAttack activeAttack)
    {
            
    }

    public override Vector2Int ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        int tempX = xPos - playerX;
        int tempY = yPos - playerY;
        Debug.Log(backwards);
        if (tempX <= boomerangForwardDistance - 1 && backwards == false)
        {
            backwards = false;
            scr_Grid.GridController.PrimeNextTile(xPos + 1, yPos);
            scr_Grid.GridController.ActivateTile(xPos, yPos);
            return new Vector2Int(xPos + 1, yPos);
        }
        else if (tempX == boomerangForwardDistance && yPos < player._gridPos.y && backwards == false)
        {
            backwards = true;
            scr_Grid.GridController.PrimeNextTile(xPos, yPos + 1);
            scr_Grid.GridController.ActivateTile(xPos, yPos);
            return new Vector2Int(xPos, yPos + 1);
        }
        else if (tempX == boomerangForwardDistance && yPos > player._gridPos.y && backwards == false)
        {
            backwards = true;
            scr_Grid.GridController.PrimeNextTile(xPos, yPos - 1);
            scr_Grid.GridController.ActivateTile(xPos, yPos);
            return new Vector2Int(xPos, yPos - 1);
        }
        else
        {
            backwards = true;
            scr_Grid.GridController.PrimeNextTile(xPos - 1, yPos);
            scr_Grid.GridController.ActivateTile(xPos, yPos);
            return new Vector2Int(xPos - 1, yPos);
        }
    }

    public override void ProgressEffects(ActiveAttack activeAttack)
    {
        activeAttack.particle.transform.position = Vector3.Lerp(activeAttack.particle.transform.position, scr_Grid.GridController.GetWorldLocation(activeAttack.lastPosition.x, activeAttack.lastPosition.y) + activeAttack.attack.particlesOffset, (particleSpeed) * Time.deltaTime);
        activeAttack.particle.transform.Rotate(0, 0, 30,Space.Self);
        activeAttack.particle.sortingOrder = -activeAttack.position.y;
    }
}


/*
        if(activeAtk.currentIncrement < scr_Grid.GridController.columnSizeMax - 1)
        {
            xPos++;
            return new Vector2Int(xPos, yPos); 
        }
        else if(activeAtk.currentIncrement < (scr_Grid.GridController.columnSizeMax - 1 + scr_Grid.GridController.rowSizeMax - 1))
        {
            yPos++;
            return new Vector2Int(xPos, yPos); 
        }
        else
        {
            xPos--;
            return new Vector2Int(xPos, yPos); 
        } */
