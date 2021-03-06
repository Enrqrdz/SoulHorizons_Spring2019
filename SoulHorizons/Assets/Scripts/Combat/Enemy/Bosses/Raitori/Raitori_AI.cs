﻿using System;
using System.Collections;
using UnityEngine;


public class Raitori_AI : scr_EntityAI
{
    public Raitori_Properties Raitori;

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

    //Attacks
    private int playerPositionX = 1;
    private int playerPositionY = 1;
    private int tornadoNumber;
    private int gustGaleColumnNumber;
    private Vector2Int RendCenter;

    //Audio
    AudioSource Attack_SFX;
    public AudioClip[] attacks_SFX;
    private AudioClip attack_SFX;

    private void Start()
    {
        SetInitialVariables();
        SetPossibleHeadPositions();
        SetZigZagPattern();
        SetTransitionNumber();
        SetTilesOccupied();
    }

    private void SetInitialVariables()
    {
        xRange = scr_Grid.GridController.columnSizeMax - entity.width; //8 - 2
        yRange = scr_Grid.GridController.rowSizeMax - entity.height;   //4 - 3
        xPosition = entity._gridPos.x;
        yPosition = entity._gridPos.y;
        currentHeadPosition = new Vector2Int(xPosition, yPosition);
    }

    private void SetPossibleHeadPositions()
    {
        //All possible positions for Raitori
        possibleHeadPositions = new[] {
                                        new Vector2Int(xRange - 2, yRange), new Vector2Int(xRange - 1, yRange), new Vector2Int(xRange, yRange),
                                        new Vector2Int(xRange - 2, yRange - 1), new Vector2Int(xRange - 1, yRange - 1), new Vector2Int(xRange, yRange - 1)
                                      };
    }

    private void SetZigZagPattern()
    {
        //Zig-Zag Movement Pattern for Raitori
        zigZagPattern = new[] {
                                possibleHeadPositions[0], possibleHeadPositions[4], possibleHeadPositions[2],
                                possibleHeadPositions[5], possibleHeadPositions[1], possibleHeadPositions[3]
                              };
    }

    private void SetTransitionNumber()
    {
        //Raitori origin must be on one of the designated positions
        for (int i = 0; i < zigZagPattern.Length; i++)
        {
            if (currentHeadPosition == zigZagPattern[i])
            {
                transitionNumber = i;
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
        if (canMove)
        {
            StartCoroutine(Movement());
        }
        if (Raitori.stormStrikesIsActive == false)
        {
            StartCoroutine(StormStrikes());
        }
        if (Raitori.birdBashIsActive == false && (scr_Grid.GridController.ReturnTerritory(currentHeadPosition.x - 1, currentHeadPosition.y).name == TerrName.Player))
        {
            StartCoroutine(BirdBash());
        }
        if(Raitori.twinTornadoIsActive == false && (transitionNumber == 2 || transitionNumber == 3))
        {
            StartCoroutine(TwinTornado());
        }
        if (Raitori.gustGaleIsActive == false && (transitionNumber == 1 || transitionNumber == 4))
        {
            StartCoroutine(GustGale());
        }
        if(Raitori.featherRendIsActive == false && ((scr_Grid.GridController.ReturnTerritory(currentHeadPosition.x - 1, currentHeadPosition.y).name == TerrName.Player)) && Raitori.birdBashIsActive == true && (Raitori_Stages.Instance.currentPhase == Phase.Stage2 || Raitori_Stages.Instance.currentPhase == Phase.Stage3))
        {
            StartCoroutine(FeatherRend());
        }
    }

    private IEnumerator StormStrikes()
    {
        PrimeStormStrike();
        Raitori.stormStrikesIsActive = true;
        yield return new WaitForSeconds(Raitori.stormStrikeWindUpTime);
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Attack_SFX = SFX_Sources[0];
        attack_SFX = attacks_SFX[1];
        Attack_SFX.clip = attack_SFX;
        Attack_SFX.Play();
        StartStormStrike();
        yield return new WaitForSeconds(Raitori.StormStrikes.incrementTime + .15f);
        DePrimeStormStrike();
        yield return new WaitForSeconds(Raitori.stormStrikeCooldown);
        Raitori.stormStrikesIsActive = false;
    }

    private void PrimeStormStrike()
    {
        for (int i = 0; i < scr_Grid.GridController.activeEntities.Length; i++)
        {
            if (scr_Grid.GridController.activeEntities[i].type == EntityType.Player)
            {
                playerPositionX = scr_Grid.GridController.activeEntities[i]._gridPos.x;
                playerPositionY = scr_Grid.GridController.activeEntities[i]._gridPos.y;
                break;
            }
        }

        Raitori.stormStrikePhase = Raitori_Stages.Instance.currentPhase;

        switch (Raitori_Stages.Instance.currentPhase)
        {
            case Phase.Stage1:
                if (playerPositionX > 0)
                {
                    scr_Grid.GridController.PrimeNextTile(playerPositionX - 1, playerPositionY);
                }

                scr_Grid.GridController.PrimeNextTile(playerPositionX, playerPositionY);
                scr_Grid.GridController.PrimeNextTile(playerPositionX + 1, playerPositionY);
                break;

            case Phase.Stage2:
                if (playerPositionX > 0)
                {
                    scr_Grid.GridController.PrimeNextTile(playerPositionX - 1, playerPositionY);
                }
                if (playerPositionY > 0)
                {
                    scr_Grid.GridController.PrimeNextTile(playerPositionX, playerPositionY - 1);
                }
                if (playerPositionY < scr_Grid.GridController.rowSizeMax)
                {
                    scr_Grid.GridController.PrimeNextTile(playerPositionX, playerPositionY + 1);
                }

                scr_Grid.GridController.PrimeNextTile(playerPositionX, playerPositionY);
                scr_Grid.GridController.PrimeNextTile(playerPositionX + 1, playerPositionY);
                break;

            case Phase.Stage3:
                if (playerPositionX > 0)
                {
                    scr_Grid.GridController.PrimeNextTile(playerPositionX - 1, playerPositionY);
                    if (playerPositionY > 0)
                    {
                        scr_Grid.GridController.PrimeNextTile(playerPositionX - 1, playerPositionY - 1);
                        scr_Grid.GridController.PrimeNextTile(playerPositionX + 1, playerPositionY - 1);

                        if (playerPositionY < scr_Grid.GridController.rowSizeMax)
                        {
                            scr_Grid.GridController.PrimeNextTile(playerPositionX - 1, playerPositionY + 1);
                            scr_Grid.GridController.PrimeNextTile(playerPositionX + 1, playerPositionY + 1);
                        }
                    }
                }
                if (playerPositionY > 0)
                {
                    scr_Grid.GridController.PrimeNextTile(playerPositionX, playerPositionY - 1);
                }
                if (playerPositionY < scr_Grid.GridController.rowSizeMax)
                {
                    scr_Grid.GridController.PrimeNextTile(playerPositionX, playerPositionY + 1);
                }

                scr_Grid.GridController.PrimeNextTile(playerPositionX, playerPositionY);
                scr_Grid.GridController.PrimeNextTile(playerPositionX + 1, playerPositionY);
                break;
            default:
                break;
        }
    }

    private void StartStormStrike()
    {
        switch (Raitori_Stages.Instance.currentPhase)
        {
            case Phase.Stage1:
                if (playerPositionX > 0)
                {
                    AttackController.Instance.AddNewAttack(Raitori.StormStrikes, playerPositionX - 1, playerPositionY, entity);
                }

                AttackController.Instance.AddNewAttack(Raitori.StormStrikes, playerPositionX, playerPositionY, entity);
                AttackController.Instance.AddNewAttack(Raitori.StormStrikes, playerPositionX + 1, playerPositionY, entity);
                break;

            case Phase.Stage2:
                if (playerPositionX > 0)
                {
                    AttackController.Instance.AddNewAttack(Raitori.StormStrikes, playerPositionX - 1, playerPositionY, entity);
                }
                if (playerPositionY > 0)
                {
                    AttackController.Instance.AddNewAttack(Raitori.StormStrikes, playerPositionX, playerPositionY - 1, entity);
                }
                if (playerPositionY < scr_Grid.GridController.rowSizeMax)
                {
                    AttackController.Instance.AddNewAttack(Raitori.StormStrikes, playerPositionX, playerPositionY + 1, entity);
                }

                AttackController.Instance.AddNewAttack(Raitori.StormStrikes, playerPositionX, playerPositionY, entity);
                AttackController.Instance.AddNewAttack(Raitori.StormStrikes, playerPositionX + 1, playerPositionY, entity);
                break;

            case Phase.Stage3:
                if (playerPositionX > 0)
                {
                    AttackController.Instance.AddNewAttack(Raitori.StormStrikes, playerPositionX - 1, playerPositionY, entity);
                    if (playerPositionY > 0)
                    {
                        AttackController.Instance.AddNewAttack(Raitori.StormStrikes, playerPositionX - 1, playerPositionY - 1, entity);
                        AttackController.Instance.AddNewAttack(Raitori.StormStrikes, playerPositionX + 1, playerPositionY - 1, entity);

                        if (playerPositionY < scr_Grid.GridController.rowSizeMax)
                        {
                            AttackController.Instance.AddNewAttack(Raitori.StormStrikes, playerPositionX - 1, playerPositionY + 1, entity);
                            AttackController.Instance.AddNewAttack(Raitori.StormStrikes, playerPositionX + 1, playerPositionY + 1, entity);
                        }
                    }
                }
                if (playerPositionY > 0)
                {
                    AttackController.Instance.AddNewAttack(Raitori.StormStrikes, playerPositionX, playerPositionY - 1, entity);
                }
                if (playerPositionY < scr_Grid.GridController.rowSizeMax)
                {
                    AttackController.Instance.AddNewAttack(Raitori.StormStrikes, playerPositionX, playerPositionY + 1, entity);
                }

                AttackController.Instance.AddNewAttack(Raitori.StormStrikes, playerPositionX, playerPositionY, entity);
                AttackController.Instance.AddNewAttack(Raitori.StormStrikes, playerPositionX + 1, playerPositionY, entity);
                break;
            default:
                break;
        }
    }

    public void DePrimeStormStrike()
    {
        switch (Raitori.stormStrikePhase)
        {
            case Phase.Stage1:
                if (playerPositionX > 0)
                {
                    scr_Grid.GridController.DePrimeTile(playerPositionX - 1, playerPositionY);
                }

                scr_Grid.GridController.DePrimeTile(playerPositionX, playerPositionY);
                scr_Grid.GridController.DePrimeTile(playerPositionX + 1, playerPositionY);
                break;

            case Phase.Stage2:
                if (playerPositionX > 0)
                {
                    scr_Grid.GridController.DePrimeTile(playerPositionX - 1, playerPositionY);
                }
                if (playerPositionY > 0)
                {
                    scr_Grid.GridController.DePrimeTile(playerPositionX, playerPositionY - 1);
                }
                if (playerPositionY < scr_Grid.GridController.rowSizeMax)
                {
                    scr_Grid.GridController.DePrimeTile(playerPositionX, playerPositionY + 1);
                }

                scr_Grid.GridController.DePrimeTile(playerPositionX, playerPositionY);
                scr_Grid.GridController.DePrimeTile(playerPositionX + 1, playerPositionY);
                break;

            case Phase.Stage3:
                if (playerPositionX > 0)
                {
                    scr_Grid.GridController.DePrimeTile(playerPositionX - 1, playerPositionY);
                    if (playerPositionY > 0)
                    {
                        scr_Grid.GridController.DePrimeTile(playerPositionX - 1, playerPositionY - 1);
                        scr_Grid.GridController.DePrimeTile(playerPositionX + 1, playerPositionY - 1);

                        if (playerPositionY < scr_Grid.GridController.rowSizeMax)
                        {
                            scr_Grid.GridController.DePrimeTile(playerPositionX - 1, playerPositionY + 1);
                            scr_Grid.GridController.DePrimeTile(playerPositionX + 1, playerPositionY + 1);
                        }
                    }
                }
                if (playerPositionY > 0)
                {
                    scr_Grid.GridController.PrimeNextTile(playerPositionX, playerPositionY - 1);
                }
                if (playerPositionY < scr_Grid.GridController.rowSizeMax)
                {
                    scr_Grid.GridController.DePrimeTile(playerPositionX, playerPositionY + 1);
                }

                scr_Grid.GridController.DePrimeTile(playerPositionX, playerPositionY);
                scr_Grid.GridController.DePrimeTile(playerPositionX + 1, playerPositionY);
                break;
            default:
                break;
        }
    }

    private IEnumerator BirdBash()
    {
        PrimeBirdBash();
        Raitori.birdBashIsActive = true;
        yield return new WaitForSeconds(Raitori.birdBashWindUpTime);
        attack_SFX = attacks_SFX[0];
        Attack_SFX.clip = attack_SFX;
        Attack_SFX.Play();
        //anim.SetBool("Attack", true);
        StartBirdBash();
        yield return new WaitForSeconds(Raitori.birdBashCooldownTime);
        Raitori.birdBashIsActive = false;
    }

    private void PrimeBirdBash()
    {
        for (int i = 0; i < Raitori.BirdBash.maxIncrementRange; i++)
        {
            scr_Grid.GridController.PrimeNextTile(zigZagPattern[transitionNumber].x - 1, zigZagPattern[transitionNumber].y + i);
        }
    }

    private void StartBirdBash()
    {
        AttackController.Instance.AddNewAttack(Raitori.BirdBash, zigZagPattern[transitionNumber].x - 1, zigZagPattern[transitionNumber].y, entity);
    }

    private IEnumerator TwinTornado()
    {
        PrimeTwinTornado();
        Raitori.twinTornadoIsActive = true;
        yield return new WaitForSeconds(Raitori.twinTornadoWindUpTime);
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Attack_SFX = SFX_Sources[0];
        attack_SFX = attacks_SFX[0];
        Attack_SFX.clip = attack_SFX;
        Attack_SFX.Play();
        StartTwinTornado();
        yield return new WaitForSeconds(Raitori.twinTornadoCooldown);
        Raitori.twinTornadoIsActive = false;
    }

    private void PrimeTwinTornado()
    {
        tornadoNumber = transitionNumber;

        for (int i = 0; i < Raitori.TwinTornado.maxIncrementRange; i++)
        {
            scr_Grid.GridController.PrimeNextTile(zigZagPattern[tornadoNumber].x - 1 - i, zigZagPattern[tornadoNumber].y);
            scr_Grid.GridController.PrimeNextTile(zigZagPattern[transitionNumber].x - 1 - i, zigZagPattern[tornadoNumber].y + 2);
        }
    }

    private void StartTwinTornado()
    {
        AttackController.Instance.AddNewAttack(Raitori.TwinTornado, zigZagPattern[tornadoNumber].x - 1, zigZagPattern[tornadoNumber].y, entity);
        AttackController.Instance.AddNewAttack(Raitori.TwinTornado, zigZagPattern[tornadoNumber].x - 1, zigZagPattern[tornadoNumber].y + 2, entity);
    }

    private IEnumerator GustGale()
    {
        Raitori.gustGaleIsActive = true;
        PrimeGustGale();
        yield return new WaitForSeconds(Raitori.gustGaleWindUpTime);
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Attack_SFX = SFX_Sources[0];
        attack_SFX = attacks_SFX[0];
        Attack_SFX.clip = attack_SFX;
        Attack_SFX.Play();
        StartGustGale();
        yield return new WaitForSeconds(Raitori.GustGale.incrementTime * scr_Grid.GridController.rowSizeMax);
        DePrimeGustGale();
        yield return new WaitForSeconds(Raitori.gustGaleKnockUpTime);
        Knockup();
        yield return new WaitForSeconds(Raitori.gustGaleCooldownTime);
        Raitori.gustGaleIsActive = false;
    }

    private void PrimeGustGale()
    {
        //TODO 4/2/19: Make a function in scr_Grid to keep track of column territory numbers.
        gustGaleColumnNumber = UnityEngine.Random.Range(0,4);

        for (int i = 0; i < scr_Grid.GridController.rowSizeMax; i++)
        {
            scr_Grid.GridController.PrimeNextTile(gustGaleColumnNumber, i);
        }
    }

    private void StartGustGale()
    {
        AttackController.Instance.AddNewAttack(Raitori.GustGale, gustGaleColumnNumber, scr_Grid.GridController.rowSizeMax - 1, entity);
    }

    private void DePrimeGustGale()
    {
        for (int i = 0; i < scr_Grid.GridController.rowSizeMax; i++)
        {
            scr_Grid.GridController.DePrimeTile(gustGaleColumnNumber, i);
        }
    }

    private void Knockup()
    {
        for (int i = 0; i < scr_Grid.GridController.activeEntities.Length; i++)
        {
            if (scr_Grid.GridController.activeEntities[i].type == EntityType.Player)
            {
                if (scr_Grid.GridController.activeEntities[i].isStunned == true)
                {
                    scr_Grid.GridController.activeEntities[i].isStunned = false;
                    scr_Grid.GridController.activeEntities[i]._health.TakeDamage(Raitori.gustGaleFallDamage);
                    scr_Grid.GridController.activeEntities[i].spr.color = Color.white;
                    break;
                }
            }
        }
    }

    private IEnumerator FeatherRend()
    {
        Raitori.gustGaleIsActive = true;
        PrimeFeatherRend();
        yield return new WaitForSeconds(Raitori.featherRendWindUpTime);
        //int index = Random.Range(0, attacks_SFX.Length);
        //attack_SFX = attacks_SFX[index];
        //Attack_SFX.clip = attack_SFX;
        //Attack_SFX.Play();
        //anim.SetBool("Attack", true);
        StartFeatherRend();
        yield return new WaitForSeconds(Raitori.FeatherRend.incrementTime);
        DePrimeFeatherRend();
        yield return new WaitForSeconds(Raitori.featherRendCooldownTime);
        Raitori.featherRendIsActive = false;
    }

    private void PrimeFeatherRend()
    {
        RendCenter = new Vector2Int(currentHeadPosition.x - 2, currentHeadPosition.y + 0);
        scr_Grid.GridController.PrimeNextTile(RendCenter.x, RendCenter.y);          //Center
        scr_Grid.GridController.PrimeNextTile(RendCenter.x - 1, RendCenter.y + 1);  //TopLeft
        scr_Grid.GridController.PrimeNextTile(RendCenter.x - 1, RendCenter.y - 1);  //BottomLeft
        scr_Grid.GridController.PrimeNextTile(RendCenter.x + 1, RendCenter.y + 1);  //TopRight
        scr_Grid.GridController.PrimeNextTile(RendCenter.x + 1, RendCenter.y - 1);  //BottomRight
    }

    private void StartFeatherRend()
    {
        AttackController.Instance.AddNewAttack(Raitori.FeatherRend, RendCenter.x, RendCenter.y, entity);
        AttackController.Instance.AddNewAttack(Raitori.FeatherRend, RendCenter.x-1, RendCenter.y+1, entity);
        AttackController.Instance.AddNewAttack(Raitori.FeatherRend, RendCenter.x-1, RendCenter.y-1, entity);
        AttackController.Instance.AddNewAttack(Raitori.FeatherRend, RendCenter.x+1, RendCenter.y+1, entity);
        AttackController.Instance.AddNewAttack(Raitori.FeatherRend, RendCenter.x+1, RendCenter.y-1, entity);
    }

    private void DePrimeFeatherRend()
    {
        RendCenter = new Vector2Int(currentHeadPosition.x - 2, currentHeadPosition.y + 0);
        scr_Grid.GridController.DePrimeTile(RendCenter.x, RendCenter.y);
        scr_Grid.GridController.DePrimeTile(RendCenter.x - 1, RendCenter.y + 1);
        scr_Grid.GridController.DePrimeTile(RendCenter.x - 1, RendCenter.y - 1);
        scr_Grid.GridController.DePrimeTile(RendCenter.x + 1, RendCenter.y + 1);
        scr_Grid.GridController.DePrimeTile(RendCenter.x + 1, RendCenter.y - 1);
    }

    private void SetTilesOccupied()
    {
        try
        {
            for (int i = 0; i < entity.width; i++)
            {
                for (int j = 0; j < entity.height; j++)
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
        yield return new WaitForSeconds(movementInterval);
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
            entity.SetLargeTransform(currentHeadPosition);
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
