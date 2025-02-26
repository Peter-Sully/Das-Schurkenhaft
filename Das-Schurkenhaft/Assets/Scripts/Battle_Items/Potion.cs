using UnityEngine;

public class Potion : MonoBehaviour
{
    public int healAmount = 3; // Health Restored
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if it's the player
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ChangeHealth(healAmount); // Heal the player
            }
            Destroy(gameObject); // Remove potion after use
        }
    }
}