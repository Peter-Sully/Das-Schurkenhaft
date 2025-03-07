using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public string description;
    public Sprite artwork;
    public int cost;
    public CardType type; // Attack, Defense, Buff, Debuff, Heal
    public int value;  // General value for damage, healing, or buffs
    public bool isMultiTarget;

    public void PlayCard()
    {
        if (CombatSystem.instance == null)
        {
            Debug.LogError("CombatSystem is missing!");
            return;
        }

        switch (type)
        {
            case CardType.Attack://change to check mult
                if (isMultiTarget)
                {
                    CombatSystem.instance.AttackMultipleTargets(value);
                }
                else 
                {
                    CombatSystem.instance.AttackOneTarget(value);
                }
                break;

            case CardType.Defense:
                Debug.Log($"Played {cardName}: Gains {value} shield.");
                // Implement shield logic
                break;

            case CardType.Buff:
                Debug.Log($"Played {cardName}: Boosts stats by {value}.");
                // Implement stat boost logic
                break;

            case CardType.Debuff:
                Debug.Log($"Played {cardName}: Lowers enemy stats by {value}.");
                // Implement debuff logic
                break;

            case CardType.Heal:
                Debug.Log($"Played {cardName}: Restores {value} HP.");
                // Implement healing logic
                break;
        }
    }
}

public enum CardType
{
    Attack,
    Defense,
    Buff,
    Debuff,
    Heal
}
