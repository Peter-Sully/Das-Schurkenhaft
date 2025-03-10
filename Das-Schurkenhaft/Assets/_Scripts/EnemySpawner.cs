using UnityEngine;
using System.Collections.Generic;
public class EnemySpawner : MonoBehaviour
{
   public GameObject enemyPrefab;

    // Array of spawn positions
    public List<Vector2Int> spawnPositions;

    // Grid size (distance between each spawn position)
    public float gridSize = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemies();
    }

    // Function to spawn enemies based on the spawn positions
    void SpawnEnemies()
    {

        spawnPositions = RoomFirstMapGenerator.instance.GetEnemySpawnPoints();
        foreach (Vector2Int spawnPos in spawnPositions)
        {
            // Convert Vector2Int to Vector3 by multiplying with grid size
            Vector3 worldPos = new Vector3(spawnPos.x * gridSize, spawnPos.y * gridSize, 0);

            // Instantiate the enemy at the world position
            Instantiate(enemyPrefab, worldPos, Quaternion.identity);
            Debug.Log("Enemy spawned at: " + worldPos);
        }
    }
}
