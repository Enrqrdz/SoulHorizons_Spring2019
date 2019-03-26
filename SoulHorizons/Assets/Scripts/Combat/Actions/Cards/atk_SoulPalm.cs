using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/SoulPalm")]
[RequireComponent(typeof(AudioSource))]

public class atk_SoulPalm : AttackData
{
    private AudioSource PlayCardSFX;
    public AudioClip PalmSFX;

    int playerX;
    int playerY;
    int counter;
    

    public override Vector2Int BeginAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        return new Vector2Int(xPos, yPos);
    }
    public override ActiveAttack BeginAttack(ActiveAttack activeAtk)
    {
        activeAtk.particle = Instantiate(particles, scr_Grid.GridController.GetWorldLocation(activeAtk.entity._gridPos.x, activeAtk.entity._gridPos.y) + new Vector3(0, 1.5f, 0), Quaternion.identity);

        playerX = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>()._gridPos.x;
        playerY = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>()._gridPos.y;
        counter = 1;

        if (PlayCardSFX == null)
        {
            PlayCardSFX = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        }

        PlayCardSFX.clip = PalmSFX;
        PlayCardSFX.Play();
        return activeAtk;
    }

    public override bool CheckCondition(Entity entity)
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
        
        Debug.Log(counter);
        if (counter == 1)
        {
            counter++;
            if (playerY == scr_Grid.GridController.rowSizeMax - 1)
            {
                maxIncrementRange = maxIncrementRange + 2;
                scr_Grid.GridController.ActivateTile(xPos, yPos);
                return new Vector2Int(xPos, yPos);
            }
            else
            {
                scr_Grid.GridController.ActivateTile(xPos, yPos + 1);
                return new Vector2Int(xPos, yPos + 1);
            }
        }

        else if (counter == 2)
        {
            counter++;
            scr_Grid.GridController.ActivateTile(xPos, yPos - 1);
            return new Vector2Int(xPos, yPos - 1);
        }

        else if (counter == 3)
        {
            counter++;
            if (playerY == 0)
            {
                counter = 5;
                maxIncrementRange = maxIncrementRange + 2;
                scr_Grid.GridController.ActivateTile(xPos + 1, yPos);
                return new Vector2Int(xPos + 1, yPos);
            }
            scr_Grid.GridController.ActivateTile(xPos, yPos - 1);
            return new Vector2Int(xPos, yPos - 1);
        }

        else if (counter == 4)
        {
            counter++;
            scr_Grid.GridController.ActivateTile(xPos + 1, yPos);
            return new Vector2Int(xPos + 1, yPos);
        }
        else if (counter == 5)
        {
            counter++;
            scr_Grid.GridController.ActivateTile(xPos, yPos + 1);
            return new Vector2Int(xPos , yPos+1);
        }
        else if (counter == 6)
        {
            counter++;
            scr_Grid.GridController.ActivateTile(xPos, yPos + 1);
            return new Vector2Int(xPos, yPos + 1);
        }
        else
        {
            return new Vector2Int(xPos, yPos);
        }

    }
    public override void ProgressEffects(ActiveAttack activeAttack)
    {
       activeAttack.particle.transform.position = Vector3.Lerp(activeAttack.particle.transform.position, scr_Grid.GridController.GetWorldLocation(activeAttack.lastPosition.x, activeAttack.lastPosition.y) + activeAttack.attack.particlesOffset, (particleSpeed) * Time.deltaTime);

    }
}
