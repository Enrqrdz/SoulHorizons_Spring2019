using UnityEngine;
using UnityEngine.UI;

public enum CharacterName { Nobody, Kana, Grimoire, Merchant, Bandit, }

[CreateAssetMenu(fileName = "New Portrait", menuName = "Portrait")]
public class Portrait : ScriptableObject
{
    public CharacterName characterName = CharacterName.Nobody;
    public Image characterImage;
}
