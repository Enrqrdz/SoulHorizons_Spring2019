using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Attacks/Boomerang")]
[RequireComponent(typeof(AudioSource))]

public class atk_Boomerang : AttackData
{
    private AudioSource PlayCardSFX;
    public AudioClip BoomerangSFX;

    public override Vector2Int BeginAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        return new Vector2Int(xPos, yPos);
    }
    public override ActiveAttack BeginAttack(ActiveAttack activeAtk)
    {
        activeAtk.particle = Instantiate(particles, Grid.Instance.GetWorldLocation(activeAtk.position),Quaternion.identity);
        PlayCardSFX = GameObject.Find("DeckManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = BoomerangSFX;
        PlayCardSFX.Play();
        activeAtk.attack.maxIncrementRange = ((Grid.Instance.columnSizeMax * 2) + Grid.Instance.rowSizeMax - 2); 
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
        
        if(activeAtk.currentIncrement < Grid.Instance.columnSizeMax - 1)
        {
            xPos++;
            return new Vector2Int(xPos, yPos); 
        }
        else if(activeAtk.currentIncrement < (Grid.Instance.columnSizeMax - 1 + Grid.Instance.rowSizeMax - 1))
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
        activeAttack.particle.transform.position = Vector3.Lerp(activeAttack.particle.transform.position, Grid.Instance.GetWorldLocation(activeAttack.lastPosition.x, activeAttack.lastPosition.y) + activeAttack.attack.particlesOffset, (particleSpeed) * Time.deltaTime);
        activeAttack.particle.transform.Rotate(0, 0, 30,Space.Self);
        activeAttack.particle.sortingOrder = -activeAttack.position.y;
    }
}
