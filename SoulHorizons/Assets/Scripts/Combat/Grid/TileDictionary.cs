using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDictionary{
    
    //Dictionary that holds territory name and color associated with it
    public static Dictionary<TerrName, Color> colorDict = new Dictionary<TerrName, Color>()
    {
        {TerrName.Player, new Color(156/255f, 97/255f, 82/255f, 1f) },
        {TerrName.Enemy, new Color(47/255f, 14/255f, 13/255f, 1f) },
        {TerrName.Neutral, new Color(255/255f, 255/255f, 255/255f, 1f) },
        {TerrName.Blocked, new Color(0/255f, 0/255f, 0/255f, 1f) }
    };
}
