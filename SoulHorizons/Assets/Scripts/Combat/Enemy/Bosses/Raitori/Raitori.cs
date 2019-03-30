using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Raitori Boss Designed for 4x8 grid
public class Raitori : scr_EntityAI
{
    //Properties
    [SerializeField]
    [Tooltip("Width in number of tiles")]
    private int width = 2;
    [SerializeField]
    [Tooltip("Height in number of tiles")]
    private int height = 3;
    private int maxHealth;
    private int currentHealth;

    //Attacks

    //Movement
    public float movementInterval;
    private bool canMove = true;

    private int transitionNumber;
    private Vector2[] possibleHeadPositions;
    private Vector2[] zigZagPattern;

    //Audio
    AudioSource Attack_SFX;
    public AudioClip[] attacks_SFX;
    private AudioClip attack_SFX;
    

    void Start()
    {
        maxHealth = entity._health.hp;

        int xRange = scr_Grid.GridController.columnSizeMax - width; //8 - 2
        int yRange = scr_Grid.GridController.rowSizeMax - height;   //4 - 3

        //All possible positions for Raitori
        possibleHeadPositions = new[] { new Vector2(xRange - 2, yRange), new Vector2(xRange - 1, yRange), new Vector2(xRange, yRange),
                            new Vector2(xRange - 2, yRange - 1), new Vector2(xRange - 1, yRange - 1), new Vector2(xRange, yRange - 1)};

        //Zig-Zag Movement Pattern for Raitori
        zigZagPattern = new[] {possibleHeadPositions[0], possibleHeadPositions[4], possibleHeadPositions[2],
                                possibleHeadPositions[5], possibleHeadPositions[1], possibleHeadPositions[3]};
    }

    public override void UpdateAI()
    {
        SetTilesOccupied();
        if (canMove)
        {
            Debug.Log("Raitori can move");
            StartCoroutine(MovementClock());
        }
    }

    private void SetTilesOccupied()
    {
        try
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    scr_Grid.GridController.SetTileOccupied(true, entity._gridPos.x + i, entity._gridPos.y + j, this.entity);
                }
            }
        }
        catch
        {
            Debug.Log("Raitori position is off!");
        }
    }

    IEnumerator MovementClock()
    {
        if (canMove)
        {
            canMove = false;
            yield return new WaitForSecondsRealtime(movementInterval);
            Move();
            canMove = true;
        }
    }

    public override void Move()
    {
        int xPosition = entity._gridPos.x;
        int yPosition = entity._gridPos.y;

        try
        {
            //Width x Height number of tiles
            //currentPosition[0] will be used as 'origin'
            Vector2[] currentPosition = new[] { new Vector2(xPosition, yPosition), new Vector2(xPosition + 1, yPosition),
                                            new Vector2(xPosition, yPosition - 1), new Vector2(xPosition + 1, yPosition - 1),
                                            new Vector2(xPosition, yPosition - 2), new Vector2(xPosition + 1, yPosition - 2),};
            //Raitori origin must be on one of the designated positions
            for (int i = 0; i < zigZagPattern.Length; i++)
            {
                if (currentPosition[0] == zigZagPattern[i])
                {
                    transitionNumber = i;
                    break;
                }
                else if(i == zigZagPattern.Length - 1)
                {
                    //Arbitrary preference in position
                    currentPosition[0] = zigZagPattern[1];
                    transitionNumber = 1;
                }
            }

            transitionNumber %= zigZagPattern.Length;

            if (scr_Grid.GridController.CheckIfOccupied(xPosition, yPosition) == false)
            {
                transitionNumber += 1;
                if (scr_Grid.GridController.ReturnTerritory(xPosition, yPosition).name == entity.entityTerritory.name)
                {
                    entity.SetTransform((int)zigZagPattern[transitionNumber].x, (int)zigZagPattern[transitionNumber].y);
                }
                else
                {
                    transitionNumber += 1;  //This will effectively skip two zig-zag positions before the next check
                }
            }
        }

        catch
        {
            Debug.Log("xPosition or yPosition out of range!");
        }

    }

    public override void Die()
    {
        entity.Death();
    }
}
