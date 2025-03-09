using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public bool isUsable; // Determines if the item can be used
    public StatToChange statToChange = StatToChange.none;
    public int amountToChangeStat;

    // Using the PlayerController to modify stats
    public void UseItem(PlayerController playerController) // Accepts player instance
    {
        // Prevent using the item if it's not usable
        if (!isUsable)
        {
            Debug.Log(itemName + " cannot be used.");
            return;
        }

        // If the player exists, modify stats
        if (playerController != null)
        {
            switch (statToChange)
            {
                case StatToChange.health:
                    playerController.ChangeHealth(amountToChangeStat);
                    break;
                case StatToChange.speed:
                    playerController.ChangeSpeed(amountToChangeStat);
                    break;
                case StatToChange.strength:
                    playerController.ChangeStrength(amountToChangeStat);
                    break;
                case StatToChange.defense:
                    playerController.ChangeDefense(amountToChangeStat);
                    break;
                default:
                    Debug.Log(itemName + " has no effect.");
                    break;
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
}
