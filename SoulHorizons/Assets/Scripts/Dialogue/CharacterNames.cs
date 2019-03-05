using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterName { Nobody, Kana, AngryGrimoire, ContentGrimoire, Merchant, Bandit }

public class CharacterNames
{
    public static CharacterName NameToEnum(string name)
    {
        if(name == "Kana")
        {
            return CharacterName.Kana;
        }

        else if (name == "AngryGrimoire")
        {
            return CharacterName.AngryGrimoire;
        }

        else if (name == "ContentGrimoire")
        {
            return CharacterName.ContentGrimoire;
        }

        else if (name == "Merchant")
        {
            return CharacterName.Merchant;
        }

        else if (name == "Bandit")
        {
            return CharacterName.Bandit;
        }
        else
        {
            return CharacterName.Nobody;
        }
    }

}