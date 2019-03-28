using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Raitori Boss Designed for 4x8 grid
public class Raitori : scr_EntityAI
{
    [Tooltip("Damage required before first phase transition")]
    public int phase1requirement = 100;

    [Tooltip("Damage required before second phase transition")]
    public int phase2requirement = 100;

    [SerializeField]
    [Tooltip("Width in number of tiles")]
    private int width = 2;
    [SerializeField]
    [Tooltip("Height in number of tiles")]
    private int height = 3;

    public Phase currentPhase;
    private int transitionNumber;
    private Vector2[] possibleHeadPositions;
    private Vector2[] zigZagPattern;

    private int maxHealth;
    private int currentHealth;

    AudioSource Attack_SFX;
    public AudioClip[] attacks_SFX;
    private AudioClip attack_SFX;
    

    void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Attack_SFX = SFX_Sources[1];
        maxHealth = entity._health.hp;

        int xRange = scr_Grid.GridController.rowSizeMax - width;
        int yRange = scr_Grid.GridController.columnSizeMax - height;

        //All possible positions for Raitori
        possibleHeadPositions = new[] { new Vector2(xRange - 2, yRange), new Vector2(xRange - 1, yRange), new Vector2(xRange, yRange),
                            new Vector2(xRange - 2, yRange - 1), new Vector2(xRange - 1, yRange - 1), new Vector2(xRange, yRange - 1)};
    }

    public override void Move()
    {
        int xPosition = entity._gridPos.x;
        int yPosition = entity._gridPos.y;
        Vector2[] currentPosition = new[] { new Vector2(xPosition, yPosition), new Vector2(xPosition + 1, yPosition),
                                            new Vector2(xPosition, yPosition - 1), new Vector2(xPosition + 1, yPosition - 1),
                                            new Vector2(xPosition, yPosition - 2), new Vector2(xPosition + 1, yPosition - 2),};

        //Zig-Zag Movement Pattern for Raitori
        zigZagPattern = new[] {possibleHeadPositions[0], possibleHeadPositions[4], possibleHeadPositions[2],
                                possibleHeadPositions[5], possibleHeadPositions[1], possibleHeadPositions[3]};

        for (int i = 0; i < zigZagPattern.Length; i++)
        {
            if (currentPosition[0] == zigZagPattern[i])
            {
                transitionNumber = i;
                break;
            }
            else
            {
                //Designated position from designers
                currentPosition[0] = zigZagPattern[1];
                transitionNumber = 1;
            }
        }

        try
        {
            if (scr_Grid.GridController.CheckIfOccupied(xPosition, yPosition) == false)
            {
                transitionNumber += 1;

                if (scr_Grid.GridController.ReturnTerritory(xPosition, yPosition).name == entity.entityTerritory.name)
                {
                    entity.SetTransform((int)zigZagPattern[transitionNumber].x, (int)zigZagPattern[transitionNumber].y);
                }
                else
                {
                    transitionNumber += 2;
                }
            }
        }

        catch
        {

        }

    }

    public override void UpdateAI()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                scr_Grid.GridController.SetTileOccupied(true, entity._gridPos.x + i, entity._gridPos.y + j, this.entity);
            }
        }  
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }
}
