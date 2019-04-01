using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Header("Must Be Set")]
    public Transform cameraTransform;
    public scr_Tile tilePrefab;
    public List<TileData> possibleTiles;

    [Header("Options")]
    public Vector2 tileSpacing = new Vector2(.7f, .55f);
    public float columnOffset = 0, rowOffset = 0;

    public scr_Tile[,] GenerateGrid(EncounterData encounter)
    {
        int columnSize = encounter.GetNumberOfColumns();
        int rowSize = encounter.GetNumberOfRows();

        scr_Tile[,] grid = new scr_Tile[columnSize, rowSize];

        Vector2 gridCenter = new Vector2((tileSpacing.x * (columnSize-1) / 2), (tileSpacing.y * rowSize / 2));

        cameraTransform.transform.position = new Vector3(gridCenter.x,gridCenter.y,cameraTransform.transform.position.z); 

        for (int j = 0; j < rowSize; j++)
        {
            for (int i = 0; i < columnSize; i++)
            {
                scr_Tile tileToAdd = null; 

                tileToAdd = (scr_Tile)Instantiate(tilePrefab, new Vector3((i * tileSpacing.x) + columnOffset, (j * tileSpacing.y) + rowOffset, 0), Quaternion.identity);

                tileToAdd.territory = encounter.GetTerrorityAtXAndY(i, j);
                tileToAdd.gridPositionX = i;
                tileToAdd.gridPositionY = j;

                SpriteRenderer spriteR = tileToAdd.GetComponent<SpriteRenderer>();

                int randomTileIndex = Random.Range(0, possibleTiles.Count);
                spriteR.sprite = possibleTiles[randomTileIndex].backGroundSprite;
                
                if (possibleTiles[randomTileIndex].backGroundSprite == null) Debug.Log("MISSING SPRITE");

                grid[i, j] = tileToAdd;
            }
        }

        return grid;
    }
}
