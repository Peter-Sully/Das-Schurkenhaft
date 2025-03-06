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