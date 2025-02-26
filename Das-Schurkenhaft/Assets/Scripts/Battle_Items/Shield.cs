// using UnityEngine;

// public class Shield : MonoBehaviour
// {
//     public int durability = 5;
//     public float shieldDuration = 5f;
//     public int defenseBoost = 2;

//     private PlayerController player;
//     private bool isEquipped = false;  // Check if the shield is equipped

//     private void Start()
//     {
//         // player is assigned a shield
//         player = FindObjectOfType<PlayerController>();

//         // Destroy the shield after a certain duration
//         Destroy(gameObject, shieldDuration);
//     }

//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player") && !isEquipped)
//         {
//             EquipShield(other.gameObject);
//         }
//         else if (other.CompareTag("Enemy") && isEquipped)
//         {
//             // If enemy hits the shield, durability goes down
//             durability--;
//             Debug.Log("Shield hit! Durability left: " + durability);

//             // Destroy the shield if durability reaches zero
//             if (durability <= 0)
//             {
//                 DestroyShield();
//             }
//         }
//     }

//     private void EquipShield(GameObject playerObject)
//     {
//         // Equip the shield
//         player.ChangeDefense(defenseBoost);
//         isEquipped = true;
//         Debug.Log("Shield equipped! Defense increased to " + player.defense);
//     }

//     private void DestroyShield()
//     {
//         if (player != null)
//         {
//             player.ChangeDefense(-defenseBoost); // Reset player's defense
//         }

//         Debug.Log("Shield destroyed!");
//         Destroy(gameObject); // Destroy the shield game object
//     }
// }
