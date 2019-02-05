using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class GameState 
{
    PlayerState player;
    InventoryState inventory;
    RegionState region;
    int currentEncounterIndex;

    public GameState()
    {
        player = new PlayerState();
        inventory = new InventoryState();
        region = new RegionState();
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

    public int GetDustAmount()
    {
        return inventory.dustNum;
    }

    public void AddDust(int dust)
    {
        inventory.dustNum += dust;
    }

    public RegionState GetRegion()
    {
        return region;
    }

    public void SetRegion(RegionState newRegion)
    {
        region = newRegion;
    }

    public void SetCurrentEncounterIndex(int encounterIndex)
    {
        currentEncounterIndex = encounterIndex;
    }

    public EncounterData GetCurrentEncounter()
    {
        EncounterState encounterState = region.encounters[currentEncounterIndex];
        return EncounterPool.GetEncounterByTierAndIndex(encounterState.tier, encounterState.encounterIndexInPool);
    }

    public void SetCurrentEncounterCompleteToTrue()
    {
        region.encounters[currentEncounterIndex].isCompleted = true;
    }

    public List<CardState> GetCardList()
    {
        return inventory.cardInv;
    }

    public List<List<CardState>> GetDeckList()
    {
        return inventory.deckList;
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

[System.Serializable]
public class InventoryState
{
    public int dustNum;
    public List<CardState> cardInv;
    public List<List<CardState>> deckList;
    public int deckIndex;
}

[System.Serializable]
public class CardState
{
    public int numberOfCopies;
    public int cardIndexInPool;

    public CardState()
    {
        numberOfCopies = 0;
        cardIndexInPool = 0;
    }
}

[System.Serializable]
public class RegionState
{
    public List<EncounterState> encounters;

    public RegionState()
    {
        encounters = new List<EncounterState>();
    }
}

[System.Serializable]
public class EncounterState
{
    public int tier;
    public int encounterIndexInPool;
    public bool isCompleted;

    public EncounterState()
    {
        isCompleted = false;
        tier = 0;
        encounterIndexInPool = 0;
    }

    public void Clone(EncounterState encounter)
    {
        isCompleted = encounter.isCompleted;
        tier = encounter.tier;
        encounterIndexInPool = encounter.encounterIndexInPool;
    }
}