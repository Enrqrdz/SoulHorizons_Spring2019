using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Attacks/Meteor")]
[RequireComponent(typeof(AudioSource))]

public class atk_Meteor : Attack {
    private AudioSource PlayCardSFX;
    public AudioClip MeteorSFX;

    public override Vector2Int BeginAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        PlayCardSFX = GameObject.Find("DeckManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = MeteorSFX;
        PlayCardSFX.Play();
        for (int i = 0; i < scr_Grid.GridController.ySizeMax; i++)
        {
            scr_Grid.GridController.PrimeNextTile(xPos, i);
            activeAtk.particles[i] = Instantiate(particles, scr_Grid.GridController.GetWorldLocation(activeAtk.entity._gridPos.x, activeAtk.entity._gridPos.y) + new Vector3(0,2.5f,0), Quaternion.Euler(new Vector3(0,0,33)));
        }
        return new Vector2Int(xPos, yPos); 
    }
    public override ActiveAttack BeginAttack(ActiveAttack activeAtk)
    {
        activeAtk.lastAttackTime += incrementTime;
        return activeAtk;
    }

    public override Vector2Int ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        return LinearForward_ProgressAttack(xPos, yPos, activeAtk);
    }

    Vector2Int LinearForward_ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        ///TODO: Make this a generic solution for scalable grids
        switch (activeAtk.currentIncrement)
        {
            case 0:
                scr_Grid.GridController.ActivateTile(xPos, yPos, activeAtk);
                return new Vector2Int(xPos, yPos + 1);

            case 1:
                scr_Grid.GridController.ActivateTile(xPos, yPos, activeAtk);
                return new Vector2Int(xPos, yPos - 2);

            case 2:
                scr_Grid.GridController.ActivateTile(xPos, yPos, activeAtk);
                break;
                //return new Vector2Int(xPos, yPos);
        }
        return new Vector2Int(xPos, yPos);
    }
    public override bool CheckCondition(scr_Entity _ent)
    {
        return true; 
    }

    public override void LaunchEffects(ActiveAttack activeAttack)
    {

    }

    public override void ProgressEffects(ActiveAttack activeAttack)
    {
        switch (activeAttack.currentIncrement)
        {
            case 0:
                activeAttack.particles[0].transform.position = Vector3.MoveTowards(activeAttack.particles[0].transform.position, scr_Grid.GridController.GetWorldLocation(activeAttack.position) + activeAttack.attack.particlesOffset, (18f) * Time.deltaTime);
                break; 

            case 1:
                activeAttack.particles[1].transform.position = Vector3.MoveTowards(activeAttack.particles[1].transform.position, scr_Grid.GridController.GetWorldLocation(activeAttack.position) + activeAttack.attack.particlesOffset, (18f) * Time.deltaTime);
                activeAttack.particles[0].gameObject.SetActive(false); 
                break;
            case 2:
                activeAttack.particles[2].transform.position = Vector3.MoveTowards(activeAttack.particles[2].transform.position, scr_Grid.GridController.GetWorldLocation(activeAttack.position) + activeAttack.attack.particlesOffset, (18f) * Time.deltaTime);
                activeAttack.particles[1].gameObject.SetActive(false);
                break;
            case 3:
                activeAttack.particles[2].gameObject.SetActive(false);
                break;
        }
    }

    public override void ImpactEffects(int xPos = -1, int yPos = -1)
    {

    }

    public override void EndEffects(ActiveAttack activeAttack)
    {
        Destroy(activeAttack.particles[0].gameObject);
        Destroy(activeAttack.particles[1].gameObject);
        Destroy(activeAttack.particles[2].gameObject);
    }
}
