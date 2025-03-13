using UnityEngine;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int executeOnLoad = 2;

    public List<Vector2Int> enemyCoordinates = new List<Vector2Int>(); // List to store enemy data
    public GameObject enemyPrefab; // The enemy prefab to spawn
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveEnemiesData(List<Vector2Int> enemies)
    {
        enemyCoordinates.Clear(); // Clear the previous data

        foreach (var enemy in enemies)
        {
            enemyCoordinates.Add(enemy);
        }
    }

    // Spawn enemies again in the main scene after loading
    public void SpawnEnemiesInMainScene(List<GameObject> enemyPrefabs)
    {
        foreach (var postion in enemyCoordinates) {
            Instantiate(enemyPrefab, (Vector3Int)postion, Quaternion.identity);
        }
    }
}