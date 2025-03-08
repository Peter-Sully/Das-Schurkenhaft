using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public StatToChange statToChange = new StatToChange();
    public int amountToChangeStat;
    public AttributeToChange attributeToChange = new AttributeToChange();
    public int amountToChangeAttribute;

    // Using the PlayerController to modify stats
    public void UseItem()
    {
        // Find the player object and get the PlayerController component
        PlayerController playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        // If the player exists and we want to change health
        if (playerController != null)
        {
            if (statToChange == StatToChange.health)
            {
                playerController.ChangeHealth(amountToChangeStat);
            }
            else if (statToChange == StatToChange.speed)
            {
                playerController.ChangeSpeed(amountToChangeStat);  // If you also want to modify speed
            }
            else if (statToChange == StatToChange.strength)
            {
                playerController.ChangeStrength(amountToChangeStat);  // Modify strength if needed
            }
            else if (statToChange == StatToChange.defense)
            {
                playerController.ChangeDefense(amountToChangeStat);  // Modify defense if needed
            }
        }
    }

    public enum StatToChange
    {
        none,
        health,
        speed,
        defense,
        strength
    };

    public enum AttributeToChange
    {
        none,
        strength,
        defense
    };
}
