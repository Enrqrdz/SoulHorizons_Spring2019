//Colin
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Cards/NameToCard")]
/// <summary>
/// Connects the data loaded from the save file to the object that corresponds to that card. Holds a list of all the card objects.
/// </summary>
public class scr_NameToCard : ScriptableObject
{
    public List<CardEntry> cards = new List<CardEntry>();
    /// <summary>
    /// This class contains fields that can be edited in the inspector to put all card information in one object.
    /// </summary>
    [System.Serializable]
    public class CardEntry
    {
        public string name;
        public CardData cardObject;
    }

    /// <summary>
    /// Takes a card name and returns the object corresponding to that name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns>Returns the scriptable object or NULL.</returns>
    public CardData ConvertNameToCard(string name)
    {
        foreach (CardEntry entry in cards)
        {
            if(name.Equals(entry.name))
            {
                return entry.cardObject;
            }
        }
        return null;
    }
}
