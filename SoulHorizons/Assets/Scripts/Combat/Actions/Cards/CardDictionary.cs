using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Cards/NameToCard")]
public class CardDictionary : ScriptableObject
{
    public List<CardEntry> cards = new List<CardEntry>();

    [System.Serializable]
    public class CardEntry
    {
        public string name;
        public ActionData cardObject;
    }

    public ActionData ConvertNameToCard(string name)
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
