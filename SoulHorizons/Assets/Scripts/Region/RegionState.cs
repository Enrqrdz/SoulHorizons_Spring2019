using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class RegionState
{
    public Map map;
}

[System.Serializable]
public class Map
{
    public List<List<Node>> rings = new List<List<Node>>();

    public Map(int numberOfTiers)
    {
        for(int i = 0; i < numberOfTiers; i ++)
        {
            rings.Add(new List<Node>());
        }
    }

    public void AddNode(Node newNode, int tier)
    {
        rings[tier].Add(newNode);
    }
}

[System.Serializable]
public class Node
{
    public Vector3 position;
    public List<Node> nextNodes = new List<Node>();
    public EncounterState encounter = new EncounterState();

    public Node(Vector3 position)
    {
        this.position = position;
    }

    public void AddConnectedNode(Node connectedNode)
    {
        nextNodes.Add(connectedNode);
    }
}