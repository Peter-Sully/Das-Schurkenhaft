using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
public class PlayerCollision : MonoBehaviour
{

 
    public int maxEnemiesInCombat = 3;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) // Check if the player hits an enemy
        {
            // Set the enemy prefab in CombatManager
            CombatManager.Instance.enemyPrefab = other.gameObject;
            // Call the OnHitPlayer method in CombatManager
            CombatManager.Instance.OnHitPlayer();

            // Check if the player has already hit the max number of enemies
            if (PlayerPrefs.GetInt("EnemyHitCount", 0) >= maxEnemiesInCombat)
            {
                Debug.Log("Max enemies hit!");
                PlayerPrefs.SetInt("EnemyHitCount", 3); // Set the hit count to 3
                SceneManager.LoadScene("CombatScene");
            }

            Debug.Log("Enemy hit!");
            // Now load the combat scene
            Debug.Log(PlayerPrefs.GetInt("EnemyHitCount", 0));
            SceneManager.LoadScene("CombatScene");
        }
    }
}