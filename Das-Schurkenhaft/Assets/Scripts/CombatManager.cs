using UnityEngine;
using System.Collections.Generic;
public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    public GameObject enemyPrefab; // The enemy prefab that the player collided with
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensures that this object isn't destroyed on scene change
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance if it exists
        }
    }
    private int hitCount = 0;  // Counter to track how many times this enemy hit the player

    public void OnHitPlayer() // This method should be called when the enemy collides with the player
    {
        hitCount++;
        SaveHitCount(hitCount);  // Save the current number of hits
    }

    void SaveHitCount(int count)
    {
        PlayerPrefs.SetInt("EnemyHitCount", count);  // Save the hit count to PlayerPrefs
        PlayerPrefs.Save();  // Save the PlayerPrefs data
    }
}
