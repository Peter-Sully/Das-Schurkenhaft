using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
public class PlayerCollision : MonoBehaviour
{

 
    public List<GameObject> enemiesHit = new List<GameObject>(); // Assign the enemy prefab in the inspector
    public int maxEnemiesInCombat = 3;
    public GameObject enemyPrefab;
    private GameObject floorTilemap, wallTilemap;
    private PlayerController playerController;

    private void Start()
    {
        floorTilemap = GameObject.Find("Floor");
        wallTilemap = GameObject.Find("Walls");
    }

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
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerController = player.GetComponent<PlayerController>();
                GameData.playerPosition = playerController.GetPosition();
            }
            CombatManager.Instance.SetEnemiesForCombat(enemiesHit);
            Debug.Log(PlayerPrefs.GetInt("EnemyHitCount", 0));
            SceneManager.LoadScene("CombatScene");
            Debug.Log(floorTilemap.name + " " + wallTilemap.name);
            floorTilemap.SetActive(false);
            wallTilemap.SetActive(false);
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            Debug.Log("Enemies found: " + enemies.Length);
            foreach (GameObject enemyObj in enemies)
            {
                enemyObj.SetActive(false);
            }
        }
    }
}