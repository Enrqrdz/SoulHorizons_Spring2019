using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Cards/SeizeDomain")]
[RequireComponent(typeof(AudioSource))]

public class scr_SeizeDomain : ActionData
{
    public float duration;
    //public Color newColor;
    private AudioSource PlayCardSFX;
    public AudioClip SeizeDomainSFX;
    public override void Activate()
    {    
        PlayCardSFX = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = SeizeDomainSFX;
        PlayCardSFX.Play();
        SeizeDomain.Instance.Activate(duration);
    }
}

public class SeizeDomain : MonoBehaviour
{
    public static SeizeDomain Instance;

    private int numberOfColumns;
    private int numberOfRows;
    public List<int> playerColumns = new List<int>();
    private int adjacentColumn;


    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        numberOfColumns = scr_Grid.GridController.columnSizeMax;
        numberOfRows = scr_Grid.GridController.columnSizeMax;

        for (int i = 0; i < numberOfColumns; i++)
        {
            for (int j = 0; j < numberOfRows; j++)
            {
                if (scr_Grid.GridController.grid[i, j].territory.name != TerrName.Player)
                {
                    return;
                }
                if (j == numberOfRows - 1)
                {
                    playerColumns.Add(i);
                    adjacentColumn = playerColumns.Count;
                }
            }
        }
    }

    public void Activate(float duration)
    {
        //Either 4 or 5
        playerColumns.Add(adjacentColumn);

        for(int j = 0; j < numberOfRows; j++)
        {
            if(scr_Grid.GridController.grid[adjacentColumn, j].territory.name != TerrName.Player)
            {
                //Push back, if not, push up, if not, push down, if not push backx2, repeat.
            }
            scr_Grid.GridController.SetTileTerritory(adjacentColumn, j, TerrName.Player, scr_TileDict.colorDict[TerrName.Player]);
        }
    }

    private void Move()
    {

    }



    public void Deactivate()
    {
        playerColumns.Remove(adjacentColumn);
    }

    /*
    //Seize Domain Card
    public void seizeDomain(float duration)
    {
        bool colFound = false;

        for (int i = 0; i < numberOfColumns; i++)
        {

            for (int j = 0; j < numberOfRows; j++)
            {
                //Debug.Log(scr_Grid.GridController.grid[i, j].territory.name);
                if (grid[i, j].territory.name != TerrName.Player)
                {
                    //Debug.Log("Column: " + i);
                    colFound = true;

                    if (!grid[i, j].occupied)
                    {
                        //Debug.Log("SEIZING!");
                        SetTileTerritory(i, j, TerrName.Player, scr_TileDict.colorDict[TerrName.Player]);
                    }
                }
            }
            //Debug.Log(colFound);
            if (colFound)
            {
                StartCoroutine(reSeize(duration));
                break;
            }


        }
    }

    private IEnumerator reSeize(float waitTime)
    {
        Debug.Log("RESEIZE");
        yield return new WaitForSeconds(waitTime);
        bool colFound = false;
        for (int i = numberOfColumns - 1; i >= 0; i--)
        {

            for (int j = 0; j < numberOfRows; j++)
            {
                //Debug.Log(scr_Grid.GridController.grid[i, j].territory.name);
                if (grid[i, j].territory.name != TerrName.Enemy)
                {
                    //Debug.Log("Column: " + i);
                    colFound = true;

                    if (!grid[i, j].occupied)
                    {
                        //Debug.Log("SEIZING!");
                        SetTileTerritory(i, j, TerrName.Enemy, scr_TileDict.colorDict[TerrName.Enemy]);
                    }
                }
            }
            //Debug.Log(colFound);
            if (colFound)
            {
                break;
            }
        }
    }
}*/
}

