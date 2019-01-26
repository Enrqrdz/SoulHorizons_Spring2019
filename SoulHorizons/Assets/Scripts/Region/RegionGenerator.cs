using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionGenerator : MonoBehaviour
{
    [SerializeField]
    private int numberOfEncounters;

    public List<Encounter> encounterPool = new List<Encounter>();

    public void Start()
    {
        EncounterPool.AddEncounter(encounterPool);
    }

    public RegionState GenerateRegion()
    {
        RegionState newRegion = new RegionState();

        for (int i = 0; i < numberOfEncounters; i++)
        {
            EncounterState newEncounter = new EncounterState();

            if (i < 1)
            {
                newEncounter.tier = 0;
            }
            else if (i >= 1 && i <= 7)
            {
                newEncounter.tier = 1;
            }
            else if (i > 7)
            {
                newEncounter.tier = 2;
            }

            newEncounter.encounterIndex = EncounterPool.GetRandomEncounterIndexOfTier(newEncounter.tier);
            newRegion.encounters.Add(newEncounter);
        }

        return newRegion;
    }
}
