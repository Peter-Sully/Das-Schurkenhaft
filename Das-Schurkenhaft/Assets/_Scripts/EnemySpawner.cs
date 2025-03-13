using UnityEngine;
using System.Collections.Generic;
public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    // Array of spawn positions
    public List<Vector2Int> spawnPositions;

    // Grid size (distance between each spawn position)
    public float gridSize = 1.0f;

    // Function to spawn enemies based on the spawn positions
    public void SpawnEnemies(List<Vector2Int> spawnPositions)
    {
        Debug.Log("Spawn position count: " + spawnPositions.Count);
        Debug.Log("Spawning enemies...");
        foreach (Vector2Int spawnPos in spawnPositions)
        {
            Debug.Log("Spawning enemy at: " + spawnPos);
            // Convert Vector2Int to Vector3 by multiplying with grid size
            Vector3 worldPos = new Vector3(spawnPos.x * gridSize, spawnPos.y * gridSize, 0);

            // Instantiate the enemy at the world position
            Instantiate(enemyPrefab, worldPos, Quaternion.identity);
            Debug.Log("Enemy spawned at: " + worldPos);
        }

        Debug.Log("Finished spawning enemies!");
    }
}
