using UnityEngine;

[CreateAssetMenu(fileName = "New Portrait", menuName = "Portrait")]
public class Portrait : ScriptableObject
{
    public CharacterName characterName = CharacterName.Nobody;
    public Sprite characterSprite;
}
