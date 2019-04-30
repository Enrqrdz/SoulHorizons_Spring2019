using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class GameState 
{
    PlayerState player;
    public InventoryState inventory;
    RegionState region;
    EncounterState currentEncounter;

    public GameState()
    {
        player = new PlayerState();
        inventory = new InventoryState();
    }
    
    public int GetPlayerLevel()
    {
        return player.playerLevel;
    }

    public string GetPlayerName()
    {
        return player.name;
    }

    public int GetPlayerHealth()
    {
        return player.currentHealth;
    }

    public void SetPlayerHealth(int health)
    {
        player.currentHealth = health;
    }

    public RegionState GetRegion()
    {
        return region;
    }

    public void SetRegion(RegionState newRegion)
    {
        region = newRegion;
    }

    public void SetCurrentEncounterState(EncounterState encounter)
    {
        currentEncounter = encounter;
    }

    public EncounterState GetCurrentEncounterState()
    {
        return currentEncounter;
    }

    public EncounterData GetCurrentEncounterData()
    {
        return currentEncounter.GetEncounterData();
    }

    public void SetCurrentEncounterCompleteToTrue()
    {
        currentEncounter.isCompleted = true;
    }
}

[System.Serializable]
public class PlayerState
{
    public int playerLevel;
    internal string name;
    public int currentHealth;

    public PlayerState()
    {
        name = "Kana";
        playerLevel = 1;
        currentHealth = 100;
    }
}