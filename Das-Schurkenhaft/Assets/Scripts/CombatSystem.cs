using UnityEngine;
using UnityEngine.UI;

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
        deckManager.LoadDeck();
        deckManager.ShuffleDeck();
        for (int i = 0; i < 5; i++) deckManager.DrawCard();
        UpdateUI();
        StartTurn();
    }

    public void StartTurn()
    {
        turnCount++;

        if (turnCount == 1)
        {
            playerHealth = maxHealth;
            enemyHealth = 100;
            playerShield = 0;
            playerEnergy = 5;
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
        StartTurn();
    }

    public void UpdateUI()
    {
        //empty for now
    }
}