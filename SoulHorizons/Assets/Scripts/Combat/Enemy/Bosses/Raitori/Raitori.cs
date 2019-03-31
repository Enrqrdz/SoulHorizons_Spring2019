using System;
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
    private int xPosition;
    private int yPosition;
    private int xRange;
    private int yRange;
    private int transitionNumber;
    private Vector2Int[] possibleHeadPositions;
    private Vector2Int[] zigZagPattern;
    private Vector2Int currentHeadPosition;

    //Audio
    AudioSource Attack_SFX;
    public AudioClip[] attacks_SFX;
    private AudioClip attack_SFX;

    private void Start()
    {
        maxHealth = entity._health.hp;
        xRange = scr_Grid.GridController.columnSizeMax - width; //8 - 2
        yRange = scr_Grid.GridController.rowSizeMax - height;   //4 - 3
        xPosition = entity._gridPos.x;
        yPosition = entity._gridPos.y;
        currentHeadPosition = new Vector2Int(xPosition, yPosition);

        entity.SetLargeTransform(currentHeadPosition, width, height);

        //All possible positions for Raitori
        possibleHeadPositions = new[] {
                                        new Vector2Int(xRange - 2, yRange), new Vector2Int(xRange - 1, yRange), new Vector2Int(xRange, yRange),
                                        new Vector2Int(xRange - 2, yRange - 1), new Vector2Int(xRange - 1, yRange - 1), new Vector2Int(xRange, yRange - 1)
                                      };

        //Zig-Zag Movement Pattern for Raitori
        zigZagPattern = new[] {
                                possibleHeadPositions[0], possibleHeadPositions[4], possibleHeadPositions[2],
                                possibleHeadPositions[5], possibleHeadPositions[1], possibleHeadPositions[3]
                              };

        //Raitori origin must be on one of the designated positions
        for (int i = 0; i < zigZagPattern.Length; i++)
        {
            if (currentHeadPosition == zigZagPattern[i])
            {
                transitionNumber = i;
                Debug.Log("\nTransition number: " + i);
                Debug.Log("\nX: " + currentHeadPosition.x + "\tY: " + currentHeadPosition.y);
                break;
            }
            else if (i == zigZagPattern.Length - 1)
            {
                //Arbitrary preference in position
                currentHeadPosition = zigZagPattern[1];
                transitionNumber = 1;
            }
        }
    }

    public override void UpdateAI()
    {
        SetTilesOccupied();
        if (canMove)
        {
            StartCoroutine(Movement());
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
                    int xPosition = (int)zigZagPattern[transitionNumber].x + i;
                    int yPosition = (int)zigZagPattern[transitionNumber].y + j;
                    scr_Grid.GridController.SetTileOccupied(true, xPosition, yPosition, this.entity);
                }
            }
        }
        catch(Exception e)
        {
            Debug.Log(e);
            Debug.Log("Raitori position is off!");
            Debug.Log("Transition Number: " + transitionNumber);
        }
    }

    private IEnumerator Movement()
    {
        canMove = false;
        yield return new WaitForSecondsRealtime(movementInterval);
        Move();
        canMove = true;
    }

    public override void Move()
    {
        transitionNumber += 1;
        transitionNumber %= zigZagPattern.Length;
        xPosition = (int)zigZagPattern[transitionNumber].x;
        yPosition = (int)zigZagPattern[transitionNumber].y;
        currentHeadPosition = new Vector2Int(xPosition, yPosition);

        if (scr_Grid.GridController.ReturnTerritory(xPosition, yPosition).name == entity.entityTerritory.name)
        {
            entity.SetLargeTransform(currentHeadPosition, width, height);
        }
        else
        {
            transitionNumber += 1;  //This will effectively skip two zig-zag positions before the next check
        }
    }

    public override void Die()
    {
        entity.Death();
    }
}
