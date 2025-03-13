using UnityEngine;
using System.Collections.Generic;
public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    public List <GameObject> enemiesForCombat = new List<GameObject>();  // The current enemy prefab

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
    public void SetEnemiesForCombat(List<GameObject> enemies)
    {
        if (enemies == null || enemies.Count == 0)
        {
            Debug.LogError("Enemies list is null or empty.");
        } else {
            enemiesForCombat = enemies;
        }
    }
}
