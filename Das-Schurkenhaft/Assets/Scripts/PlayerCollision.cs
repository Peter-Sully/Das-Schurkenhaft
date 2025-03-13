using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
public class PlayerCollision : MonoBehaviour
{

 
    public List<GameObject> enemiesHit = new List<GameObject>(); // Assign the enemy prefab in the inspector
    public int maxEnemiesInCombat = 3;
    public GameObject enemyPrefab;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) // Check if the player hits an enemy
        {
            // Set the enemy prefab in CombatManager
            GameObject enemy = other.gameObject;
            if (!enemiesHit.Contains(enemyPrefab) && enemiesHit.Count < maxEnemiesInCombat)
            {
                enemiesHit.Add(enemyPrefab);
            }
            Debug.Log("Enemy hit!");
            // Now load the combat scene
            CombatManager.Instance.SetEnemiesForCombat(enemiesHit);
            SceneManager.LoadScene("CombatScene");
        }
    }
}