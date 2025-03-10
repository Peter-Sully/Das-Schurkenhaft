using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyCombat : MonoBehaviour
{
    public static EnemyCombat instance;
    public string enemyName = "Dummy_Enemy";
    public int maxHealth = 25;
    public int currentHealth;
    private Image healthBar;
    private TMP_Text healthtext;
    public bool isDead = false;

    public EnemyDeck enemyDeck;
    public GameObject enemyPrefab;
    
    public void SetHealthBar(Image healthBar)

    {
        this.healthBar = healthBar;
    }

    public void SetHealthText(TMP_Text healthtext)
    {
        this.healthtext = healthtext;
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    public void UpdateHealthText()
    {
        if (healthtext != null) healthtext.text = $"HP: {currentHealth}/{maxHealth}";
    }

    public void InitializeEnemy(string enemyType)
    {
        enemyDeck = enemyPrefab.GetComponent<EnemyDeck>();
        enemyDeck.GenerateDeckByType(enemyType);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log($"{enemyName} took {amount} damage! Current health: {currentHealth}");

        if (currentHealth <= 0) 
        {
            currentHealth = 0;
            Die();
        }
    }

    public void AttackPlayer(int amount) {
        if (CombatSystem.instance.playerShield >= amount) {
            CombatSystem.instance.playerShield -= amount;
        } else {
            CombatSystem.instance.playerHealth -= amount;
        }   
    }

    public void Heal(int amount) {
        CombatSystem.instance.enemyHealth += amount;
        currentHealth += amount;
        Debug.Log($"{enemyName} healed {amount}, Current health: {currentHealth}");
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
        }
        Debug.Log("Enemy turn ends.");

    }
}


