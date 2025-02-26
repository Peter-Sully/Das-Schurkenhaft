using UnityEngine;

public class StrengthTrinket : MonoBehaviour
{
    public int strengthBoostAmount = 2; // Amount of strength increase

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if it's the player
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.strength += strengthBoostAmount; // Increase player strength
                Debug.Log("Strength increased by " + strengthBoostAmount); // Confirm strength
            }
            Destroy(gameObject); // Remove the trinket after collection
        }
    }
}
