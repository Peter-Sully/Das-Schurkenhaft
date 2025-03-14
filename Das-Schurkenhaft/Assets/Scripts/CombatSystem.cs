using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

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

    public Button endTurnButton;
    private DeckManager deckManager;
    public GameObject playerPrefab;
    public GameObject[] enemyPrefabs;
    public Transform playerSpawnPoint;
    public Transform[] enemySpawnPoints;
    public Image playerHealthBar;
    public Image playerShieldBar;
    public TMP_Text playerHealthText;
    public TMP_Text playerShieldText;
    public Image playerEnergyBar;
    public TMP_Text playerEnergyBarText;
    public Image[] enemyHealthBars;
    public TMP_Text[] enemyHealthBarsText;

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
            enemySpawnPoints = new Transform[3];
            enemySpawnPoints[0] = GameObject.Find("EnemySpawn1")?.transform;
            enemySpawnPoints[1] = GameObject.Find("EnemySpawn2")?.transform;
            enemySpawnPoints[2] = GameObject.Find("EnemySpawn3")?.transform;
        }

        playerHealthBar = GameObject.Find("HealthBar")?.GetComponent<Image>();
        playerShieldBar = GameObject.Find("ShieldBar")?.GetComponent<Image>();
        playerHealthText = GameObject.Find("HealthBarText")?. GetComponent<TMP_Text>();
        playerShieldText = GameObject.Find("ShieldBarText")?.GetComponent<TMP_Text>();
        playerEnergyBar = GameObject.Find("EnergyBar")?.GetComponent<Image>();
        playerEnergyBarText = GameObject.Find("EnergyBarText")?.GetComponent<TMP_Text>();

        enemyHealthBars = new Image[enemySpawnPoints.Length];
        enemyHealthBarsText = new TMP_Text[enemySpawnPoints.Length];

        for (int i = 0; i < enemyHealthBars.Length; i++)
        {
            enemyHealthBars[i] = GameObject.Find($"EnemyHealth{i+1}")?.GetComponent<Image>();
        }

        for (int i = 0; i < enemyHealthBarsText.Length; i++)
        {
            enemyHealthBarsText[i] = GameObject.Find($"EnemyHealth{i+1}Text")?.GetComponent<TMP_Text>();
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
        for (int i = 0; i < enemySpawnPoints.Length; i++)
        {
            int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);

            GameObject enemyInstance = Instantiate(enemyPrefabs[randomEnemyIndex], enemySpawnPoints[i].position, Quaternion.identity);
            EnemyCombat enemyCombat = enemyInstance.GetComponent<EnemyCombat>();
            enemies.Add(enemyCombat);

            if (i <  enemyHealthBars.Length)
            {
                enemyCombat.SetHealthBar(enemyHealthBars[i]);
                enemyCombat.SetHealthText(enemyHealthBarsText[i]);
            }

            Debug.Log($"Enemy {i+1} spawned at {enemySpawnPoints[i].name}!");
        }
    }

    public void RemoveEnemyFromList(EnemyCombat enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            Debug.Log($"Enemy {enemy.enemyName} removed from combat.");

            if (enemies.Count == 0) {
                Debug.Log("All enemies defeated");
                StartCoroutine(EndCombatAndSwitchScene());
            }
        }
    }
    
    private IEnumerator EndCombatAndSwitchScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainScene");
    }

    public void StartTurn()
    {
        if (playerHealth <= 0) 
        {
            playerHealth = 0;
            //scene switch
        }

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

    public void AttackOneTarget(int damage)
    {
        if (enemies.Count > 0)
        {
            EnemyCombat enemy = enemies[0].GetComponent<EnemyCombat>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                enemy.UpdateHealthText();
            }
        }
    }

    public void AttackMultipleTargets(int damage)
    {
        List<EnemyCombat> enemiesCopy = new List<EnemyCombat>(enemies);

        foreach (EnemyCombat enemy in enemiesCopy)
        {
            if (enemy != null && !enemy.isDead)
            {
                enemy.TakeDamage(damage);
                enemy.UpdateHealthText();
            }
        }
    }

    public void AddShield(int shield)
    {
        if (playerShield < maxHealth)
        {
            playerShield += shield;
            if(playerShield >= maxHealth) playerShield = maxHealth;
        }
        else Debug.Log("Max Shield Reached!");
        
    }

    public void HealPlayer(int heal)
    {
        if (playerHealth < maxHealth)
        {
            playerHealth += heal;
            if (playerHealth >= maxHealth) playerHealth = maxHealth;
        }
        else Debug.Log("Max Health Reached!");
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
        if (playerHealthBar != null) playerHealthBar.fillAmount = (float)playerHealth / maxHealth;
        if (playerHealthText != null) playerHealthText.text = $"HP: {playerHealth}/{maxHealth}";
        if (playerShieldBar != null) playerShieldBar.fillAmount = (float)playerShield / maxHealth;
        if (playerShieldText != null) playerShieldText.text = $"Shield: {playerShield}";
        if (playerEnergyBar != null) playerEnergyBar.fillAmount = (float)playerEnergy / maxEnergy;
        if (playerEnergyBarText != null) playerEnergyBarText.text = $"Energy: {playerEnergy}/{maxEnergy}";
    }
}