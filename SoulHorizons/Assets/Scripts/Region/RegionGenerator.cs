using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionGenerator : MonoBehaviour
{
    [Header("Options")]
    public int tiers = 10;
    public float distanceBetweenTiers = 10;
    public float connectionRange = 10;

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
            int maxNodesInTier = i * 3 + 1;
            int anglePortion = 360/maxNodesInTier;

            for(int j = 0; j < maxNodesInTier; j++)
            {
                float randomAngle = Random.Range(0, anglePortion) + j * anglePortion + Random.Range(0, 360f/maxNodesInTier/2);

                float radAngle = Mathf.Deg2Rad * randomAngle;
                Vector3 position = new Vector3((i * distanceBetweenTiers) * Mathf.Cos(radAngle), (i * distanceBetweenTiers) * -Mathf.Sin(radAngle));
               
                map.AddNode(new Node(position), i);
            }
        }

        for(int i = 0; i < tiers - 1; i++)
        {
            foreach(Node currentNode in map.rings[i])
            {
                foreach(Node nextNode in map.rings[i+1])
                {
                    if(Vector3.Distance(currentNode.position, nextNode.position) < (connectionRange + distanceBetweenTiers))
                    {
                        currentNode.AddNextNode(nextNode);
                    }
                }
            }
        }

        return map;
    }
}
