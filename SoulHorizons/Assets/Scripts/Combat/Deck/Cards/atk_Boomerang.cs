using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Attacks/Boomerang")]
[RequireComponent(typeof(AudioSource))]

public class atk_Boomerang : Attack
{
    private AudioSource PlayCardSFX;
    public AudioClip BoomerangSFX;

    public override Vector2Int BeginAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        return new Vector2Int(xPos, yPos);
    }
    public override ActiveAttack BeginAttack(ActiveAttack activeAtk)
    {
        activeAtk.particle = Instantiate(particles, scr_Grid.GridController.GetWorldLocation(activeAtk.pos),Quaternion.identity);
        PlayCardSFX = GameObject.Find("DeckManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = BoomerangSFX;
        PlayCardSFX.Play();
        activeAtk._attack.maxIncrements = ((scr_Grid.GridController.xSizeMax * 2) + scr_Grid.GridController.ySizeMax - 2); 
        return activeAtk;
    }

    public override bool CheckCondition(scr_Entity _ent)
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
        
        if(activeAtk.currentIncrement < scr_Grid.GridController.xSizeMax - 1)
        {
            xPos++;
            return new Vector2Int(xPos, yPos); 
        }
        else if(activeAtk.currentIncrement < (scr_Grid.GridController.xSizeMax - 1 + scr_Grid.GridController.ySizeMax - 1))
        {
            yPos++;
            return new Vector2Int(xPos, yPos); 
        }
        else
        {
            xPos--;
            return new Vector2Int(xPos, yPos); 
        }
    }

    public override void ProgressEffects(ActiveAttack activeAttack)
    {
        activeAttack.particle.transform.position = Vector3.Lerp(activeAttack.particle.transform.position, scr_Grid.GridController.GetWorldLocation(activeAttack.lastPos.x, activeAttack.lastPos.y) + activeAttack._attack.particlesOffset, (particleSpeed) * Time.deltaTime);
        activeAttack.particle.transform.Rotate(0, 0, 30,Space.Self);
        activeAttack.particle.sortingOrder = -activeAttack.pos.y;
    }
}
