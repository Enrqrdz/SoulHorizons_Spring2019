using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Attacks/SwordCross")]
[RequireComponent(typeof(AudioSource))]
public class atk_SwordCross : AttackData
{
    private AudioSource PlayCardSFX;
    public AudioClip SwordSFX;
    int playerX;
    int playerY;
    int startX = 0;
    int startY = 0;

    int incrementTimeModifier = 1;

    public override Vector2Int BeginAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        playerX = ObjectReference.Instance.PlayerEntity._gridPos.x;
        playerY = ObjectReference.Instance.PlayerEntity._gridPos.y;        

        if (yPos > playerY)
        {
            int temp = xPos - playerX;
            if (temp == 1)
            {
                activeAtk.particle = Instantiate(particles, scr_Grid.GridController.GetWorldLocation(activeAtk.position), Quaternion.Euler(new Vector3(0, 0, 315)));
            }
            else
            {
                activeAtk.particle = Instantiate(particles, scr_Grid.GridController.GetWorldLocation(activeAtk.position), Quaternion.Euler(new Vector3(0, 0, 225)));
            }
        }
        else
        {
            int temp = xPos - playerX;
            if (temp == 1)
            {
                activeAtk.particle = Instantiate(particles, scr_Grid.GridController.GetWorldLocation(activeAtk.position), Quaternion.Euler(new Vector3(0, 0, 45)));
            }
            else
            {
                activeAtk.particle = Instantiate(particles, scr_Grid.GridController.GetWorldLocation(activeAtk.position), Quaternion.Euler(new Vector3(0, 0, 135)));
            }
        }
        
        startX = xPos;
        startY = yPos;

        return new Vector2Int(xPos, yPos);
    }
    public override ActiveAttack BeginAttack(ActiveAttack activeAtk)
    {
        
        if (PlayCardSFX == null)
        {
            PlayCardSFX = ObjectReference.Instance.ActionManager;
        }

        PlayCardSFX.clip = SwordSFX;
        PlayCardSFX.Play();
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
        int temp = xPos - playerX;
        if (yPos > playerY)
        {         
            if (temp == 1)
            {
                scr_Grid.GridController.ActivateTile(xPos, yPos);
                scr_Grid.GridController.PrimeNextTile(xPos + 1, yPos - 1);
                return new Vector2Int(xPos + 1, yPos - 1);
            }
            else
            {
                scr_Grid.GridController.ActivateTile(xPos, yPos);
                scr_Grid.GridController.PrimeNextTile(xPos - 1, yPos - 1);               
                return new Vector2Int(xPos - 1, yPos - 1);
            }
        }
        else if (yPos < playerY)
        {
            if (temp == 1)
            {
                scr_Grid.GridController.ActivateTile(xPos, yPos);
                scr_Grid.GridController.PrimeNextTile(xPos + 1, yPos + 1);
                return new Vector2Int(xPos + 1, yPos + 1);
            }
            else
            {
                scr_Grid.GridController.ActivateTile(xPos, yPos);
                scr_Grid.GridController.PrimeNextTile(xPos - 1, yPos + 1);
                return new Vector2Int(xPos - 1, yPos + 1);
            }
        }
        else
        {
            scr_Grid.GridController.ActivateTile(xPos, yPos);
            scr_Grid.GridController.PrimeNextTile(xPos, yPos);
            return new Vector2Int(xPos, yPos);
        }
    }

    public override void ProgressEffects(ActiveAttack activeAttack)
    {
        activeAttack.particle.transform.position = Vector3.Lerp(activeAttack.particle.transform.position, scr_Grid.GridController.GetWorldLocation(activeAttack.lastPosition.x, activeAttack.lastPosition.y) + activeAttack.attack.particlesOffset, (particleSpeed) * Time.deltaTime);
    }
}
