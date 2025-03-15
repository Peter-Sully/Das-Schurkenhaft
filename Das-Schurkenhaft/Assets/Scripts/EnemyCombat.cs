using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyCombat : MonoBehaviour
{
    public static EnemyCombat instance;
    public string enemyName;
    public int maxEnemyHealth = 25;
    public int currentEnemyHealth;
    private Image healthBar;
    private TMP_Text healthtext;
    public bool isDead = false;

    public EnemyDeck enemyDeck;
    public GameObject enemyPrefab;
    
    void Awake() {
    // Make sure instance is set only once
    if (instance == null) {
        instance = this;
    } else {
        Debug.LogError("Multiple instances of EnemyCombat found!");
    }
}
    public void SetHealthBar(Image healthBar)

    {
        this.healthBar = healthBar;
    }

    public void SetHealthText(TMP_Text healthtext)
    {
        this.healthtext = healthtext;
        currentEnemyHealth = maxEnemyHealth;
        UpdateHealthText();
    }

    public void UpdateHealthText()
    {
        if (healthtext != null) healthtext.text = $"HP: {currentEnemyHealth}/{maxEnemyHealth}";
    }

    public void InitializeEnemy(string enemyType)
    {
        enemyDeck = enemyPrefab.GetComponent<EnemyDeck>();
        if (enemyDeck == null) {
            Debug.LogError("EnemyDeck component is missing on enemyPrefab!");
        } else {
            enemyDeck.GenerateDeckByType(enemyType);
        }
        enemyDeck.GenerateDeckByType(enemyType);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentEnemyHealth -= amount;
        Debug.Log($"{enemyName} took {amount} damage! Current health: {currentEnemyHealth}");

        if (currentEnemyHealth <= 0) 
        {
            currentEnemyHealth = 0;
            Die();
        }
    }

    public void AttackPlayer(int amount) {
        if (CombatSystem.instance.playerShield >= amount) {
            CombatSystem.instance.playerShield -= amount;
            Debug.Log("Player shield took {amount} damage");
        } else {
            CombatSystem.instance.playerHealth -= amount;
            Debug.Log($"{enemyName} attacked player for {amount} damage, Current health: {CombatSystem.instance.playerHealth}");
        }   
    }

    public void Heal(int amount) {
        CombatSystem.instance.enemyHealth += amount;
        currentEnemyHealth += amount;
        Debug.Log($"{enemyName} healed {amount}, Current health: {currentEnemyHealth}");
    }

    private void Die()
    {
        Debug.Log($"{enemyName} has died!");

        if (isDead) return;

        isDead = true;

        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(false);
        }

        if (healthtext != null)
        {
            healthtext.gameObject.SetActive(false);
        }

        CombatSystem.instance.RemoveEnemyFromList(this);
        Destroy(gameObject);
    }

    public void TakeTurn()
    {
        Debug.Log("Enemy Turn begins.");
        //Randomly draw the played card
        EnemyCard drawnCard = enemyDeck.drawCard();
        if (drawnCard != null) {
            Debug.Log("Enemy Draws: {drawnCard.name}");
            drawnCard.playCard();
            if (CombatSystem.instance.playerHealth <= 0) {
                Debug.Log("Player has died!");
            }
        } else {
            Debug.Log("Enemy has no cards left!");
        }
        CombatSystem.instance.playerHealth -= 10;
        Debug.Log("Enemy turn ends.");

    }
}


