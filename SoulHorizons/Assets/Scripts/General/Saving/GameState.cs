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
        return scr_Inventory.dustNum;
    }

    public RegionState GetRegion()
    {
        return region;
    }

    public void SetRegion(RegionState newRegion)
    {
        region = newRegion;
    }

    public void SaveInventory()
    {
        try
        {
            inventory.dustNum = scr_Inventory.dustNum;
            inventory.cardInv = scr_Inventory.getCardInv();
            inventory.deckList = scr_Inventory.getDeckList();
            inventory.deckIndex = scr_Inventory.deckIndex;
            inventory.numDecks = scr_Inventory.numDecks;
        }
        catch (System.NullReferenceException e)
        {
            Debug.Log("This is a " + e);
        }
    }

    public void LoadInventory()
    {
        scr_Inventory.dustNum = inventory.dustNum;
        scr_Inventory.deckList = inventory.deckList;
        scr_Inventory.deckIndex = inventory.deckIndex;
        scr_Inventory.numDecks = inventory.numDecks;
    }

    public List<KeyValuePair<string, int>> GetCardList()
    {
        return inventory.cardInv;
    }

    public List<List<KeyValuePair<string, int>>> GetDeckList()
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
    public List<KeyValuePair<string, int>> cardInv;
    public List<List<KeyValuePair<string, int>>> deckList;
    public int deckIndex;
    public int numDecks;
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
    public int encounterIndex;
    public bool completed;

    public EncounterState()
    {
        completed = false;
        tier = 0;
        encounterIndex = 0;
    }

    public void Clone(EncounterState encounter)
    {
        completed = encounter.completed;
        tier = encounter.tier;
        encounterIndex = encounter.encounterIndex;
    }
}