using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionGenerator : MonoBehaviour
{
    [Header("Options")]
    public int tiers = 10;
    public float distanceBetweenTiers = 10;
    public float connectionRange = 10;
    public float mapWidth = 10;

    public RegionState GenerateRegion()
    {
        bool trainingEncounterPlaced = false;

        RegionState newRegion = new RegionState();
        newRegion.map = GenerateMap();

        int maxDifficulty = EncounterPool.GetMaxDifficulty();
        float diffcultyPerTier = (float) maxDifficulty / tiers;

        for(int i = 0; i < newRegion.map.rings.Count; i++)
        {
            foreach(Node node in newRegion.map.rings[i])
            {
                EncounterState newEncounter = new EncounterState();

                int encounterDifficulty = Mathf.CeilToInt(i * diffcultyPerTier);

                newEncounter.tier = encounterDifficulty;

                newEncounter.Randomize();
                node.encounter = newEncounter;
            }
        }

        int randInt = Random.Range(0, newRegion.map.rings[newRegion.map.rings.Count - 1].Count);
        newRegion.map.rings[newRegion.map.rings.Count - 1][randInt].encounter.type = EncounterType.Boss;
        newRegion.map.rings[newRegion.map.rings.Count - 1][randInt].encounter.Randomize();

        return newRegion;
    }

    private Map GenerateMap()
    {
        Map map = new Map(tiers);

        map.AddNode(new Node(new Vector3(0, 0, 0)), 0);
        map.rings[0][0].SetAsRootNode();

        for(int i = 1; i < tiers; i++)
        {
            int numNodesInTier;

            if(i <=3)
                numNodesInTier = i + 1;
            else
                numNodesInTier = Random.Range(3, 6);

            float widthPortion = mapWidth / numNodesInTier;
            Debug.Log("tier" + i + "widthportion" + widthPortion);

            for(int j = 0; j < numNodesInTier; j++)
            {
                float yPosition = i * distanceBetweenTiers;
                yPosition += Random.Range(0, distanceBetweenTiers / 2);
                float xPosition = widthPortion * j;
                xPosition += Random.Range(0, widthPortion / 2);
                xPosition -= mapWidth/2;


                Vector3 position = new Vector3(xPosition, yPosition, 0);
               
                map.AddNode(new Node(position), i);
            }
        }

        for(int i = 0; i < tiers - 1; i++)
        {
            foreach(Node currentNode in map.rings[i])
            {
                foreach(Node nextNode in map.rings[i+1])
                {
                    if(i == 0)
                    {
                        currentNode.AddNextNode(nextNode);
                    }
                    else if(Vector3.Distance(currentNode.position, nextNode.position) < (connectionRange + distanceBetweenTiers))
                    {
                        currentNode.AddNextNode(nextNode);
                    }
                }
            }
        }

        return map;
    }
}
