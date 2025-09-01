using UnityEngine;

[CreateAssetMenu(fileName = "New CardData", menuName = "Card Data")]
public class CardData : ScriptableObject
{
    public int cardID;
    public string cardName;
    public Sprite frontSprite;
    public Sprite backSprite;
}