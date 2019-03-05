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
    public List<Node> pastNodes = new List<Node>();
    public List<Node> nextNodes = new List<Node>();
    public EncounterState encounter = new EncounterState();

    private bool isRoot = false;

    public Node(Vector3 position)
    {
        this.position = position;
    }

    public void SetAsRootNode()
    {
        isRoot = true;
    }

    public void AddNextNode(Node nextNode)
    {
        nextNodes.Add(nextNode);
        nextNode.pastNodes.Add(this);
    }

    public bool IsAccessible()
    {
        foreach(Node node in pastNodes)
        {
            if(node.encounter.isCompleted)
                return true;
        }

        foreach(Node node in nextNodes)
        {
            if(node.encounter.isCompleted)
                return true;
        }

        if(isRoot)
            return true;

        return false;
    }

    public EncounterState GetEncounterState()
    {
        encounter.isAccessible = IsAccessible();
        return encounter;
    }
}