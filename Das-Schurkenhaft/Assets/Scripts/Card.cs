using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public string description;
    public int damage;
    public Sprite artwork;
    public int cost;
    public CardType type; // Enum: Attack, Defense, etc.

    public void PlayCard() 
    {
        Debug.Log($"Played {cardName}!");
        // Implement card effect logic here
    }
}

public enum CardType
{
    Attack,
    Defense,
    Buff,
    Debuff
}
