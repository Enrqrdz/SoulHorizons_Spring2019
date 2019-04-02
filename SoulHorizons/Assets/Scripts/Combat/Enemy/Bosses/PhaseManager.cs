using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Phase { Idle, Stage1, Stage2, Stage3, Stage4, Transition, Death }

public class PhaseManager : MonoBehaviour
{
    public static PhaseManager Instance { get; private set; }
    private Phase phaseInScene;

    private scr_Tile[,] grid;

    private void Start()
    {
        if(scr_Grid.GridController.grid != null)
        {
            SetStartingGrid();
        }

    }

    private void SetStartingGrid()
    {
        int rowSize = scr_Grid.GridController.rowSizeMax;
        int columnSize = scr_Grid.GridController.columnSizeMax;

        grid = new scr_Tile[columnSize, rowSize];

        for (int i = 0; i < rowSize; i++)
        {
            for (int j = 0; j < columnSize; j++)
            {
                grid[i, j] = scr_Grid.GridController.grid[i, j];
            }
        }
    }

    private void Update()
    {
        switch (phaseInScene)
        {
            case Phase.Idle:
                //TODO: Play Boss Intro Animation
                break;
            case Phase.Transition:
                //TODO: Boss Transition
                break;
            case Phase.Stage1:
                //TODO: First stage of fight
                break;
            case Phase.Stage2:
                //TODO: Second stage of fight (if needed)
                break;
            case Phase.Stage3:
                //TODO: Third stage of fight (if needed)
                break;
            case Phase.Stage4:
                //TODO: Fourth stage of fight (if needed)
                break;
            case Phase.Death:
                //TODO: Death of boss
                break;
            default:
                break;
        }
    }

    public void PhaseInScene(Phase newPhase)
    {
        phaseInScene = newPhase;
    }

    public void ResetPhase()
    {
    }
}
