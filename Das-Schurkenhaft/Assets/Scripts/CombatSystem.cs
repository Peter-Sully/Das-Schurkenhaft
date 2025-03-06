using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CombatSystem : MonoBehaviour
{
    public static CombatSystem instance;

    public int maxHealth = 100;
    public int playerHealth;
    public int playerShield = 0;
    public int playerEnergy;
    public int maxEnergy = 10;
    public int enemyHealth;

    private int turnCount = 0;

    public Text playerShieldText;
    public Text enemyHealthText;
    public Text energyText;
    public Button endTurnButton;

    private DeckManager deckManager;
    public GameObject playerPrefab;
    public GameObject[] enemyPrefabs;
    public Transform playerSpawnPoint;
    public Transform[] enemySpawnPoints;

    private GameObject player;
    private List<EnemyCombat> enemies = new List<EnemyCombat>();

    void Awake() 
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
    }

    void Start()
    {
        deckManager = Object.FindFirstObjectByType<DeckManager>();

        endTurnButton.onClick.AddListener(EndTurn);

        if (playerSpawnPoint == null)
        {
            GameObject foundPlayerSpawn = GameObject.FindWithTag("PlayerSpawn");
            if (foundPlayerSpawn != null)
            {
                playerSpawnPoint = foundPlayerSpawn.transform;
            }
            else Debug.LogError("Gameobject PlayerSpawn not found");
        }

        if (enemySpawnPoints == null || enemySpawnPoints.Length == 0)
        {
            GameObject[] foundEnemies = GameObject.FindGameObjectsWithTag("EnemySpawn");
            Debug.Log("Searching for enemy spawns");
            if (foundEnemies.Length > 0)
            {
                enemySpawnPoints = new Transform[foundEnemies.Length];
                for (int i = 0; i < foundEnemies.Length; i++)
                {
                    enemySpawnPoints[i] = foundEnemies[i].transform;
                    Debug.Log($"Spawn point {i} assigned");
                }
            }
            else Debug.LogError("GameObjects EnemySpawn not found");
        }

        playerPrefab = Resources.Load<GameObject>("Prefabs/PlayerPrefab");
        if (playerPrefab == null) Debug.LogError("Failed to load PlayerPrefab");

        enemyPrefabs = new GameObject[1];
        enemyPrefabs[0] = Resources.Load<GameObject>("Prefabs/EnemyPrefab"); //this needs to change when you add more enemies
        if (enemyPrefabs == null) Debug.LogError("Failed to load EnemyPrefabs");

        startCombat();
    }

    void startCombat()
    {
        turnCount = 0;
        SpawnPlayer();
        SpawnEnemies();
        deckManager.LoadDeck();
        deckManager.ShuffleDeck();
        for (int i = 0; i < 5; i++) deckManager.DrawCard();
        UpdateUI();
        StartTurn();
    }

    void SpawnPlayer()
    {
        player = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
        playerHealth = maxHealth;
        playerEnergy = 5;
        playerShield = 0;
    }

    void SpawnEnemies()
    {
        for (int i=0; i < enemySpawnPoints.Length; i++)
        {
            int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
            GameObject enemyInstance = Instantiate(enemyPrefabs[randomEnemyIndex], enemySpawnPoints[i].position, Quaternion.identity);
            enemies.Add(enemyInstance.GetComponent<EnemyCombat>());
            Debug.Log($"Enemy {i+1} spawned!");
        }
    }

    public void RemoveEnemyFromList(EnemyCombat enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            Debug.Log($"Enemy {enemy.enemyName} removed from combat.");
        }
    }

    public void StartTurn()
    {
        turnCount++;

        if (turnCount == 1)
        {
            Debug.Log("Combat started!");
        }
        else
        {
            playerEnergy = Mathf.Min(playerEnergy + 3, maxEnergy);
            deckManager.DrawCard();
            deckManager.DrawCard();
        }
        Debug.Log($"Turn {turnCount} started. Energy: {playerEnergy}");
        UpdateUI();
    }

    public bool SpendEnergy(int amount)
    {
        if (playerEnergy >= amount)
        {
            playerEnergy -= amount;
            UpdateUI();
            return true;
        }
        Debug.Log("Not enough energy!");
        return false;
    }

    public void EndTurn()
    {
        Debug.Log("Turn ended.");

        foreach (EnemyCombat enemy in enemies)
        {
            enemy.TakeTurn();
        }

        StartTurn();
    }

    public void UpdateUI()
    {
        //empty for now
    }
}