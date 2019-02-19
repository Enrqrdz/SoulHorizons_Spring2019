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
    EncounterState currentEncounter;

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

    public void SetCurrentEncounterState(EncounterState encounter)
    {
        currentEncounter = encounter;
    }

    public EncounterData GetCurrentEncounter()
    {
        return currentEncounter.GetEncounterData();
    }

    public void SetCurrentEncounterCompleteToTrue()
    {
        currentEncounter.isCompleted = true;
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