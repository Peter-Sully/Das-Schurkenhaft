using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public static EnemyCombat instance;
    public string enemyName = "Dummy_Enemy";
    public int maxHealth = 25;
    public int currentHealth;

    public EnemyDeck enemyDeck;
    
    public GameObject enemyPrefab;
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void InitializeEnemy(string enemyType)
    {
        enemyDeck = enemyPrefab.GetComponent<EnemyDeck>();
        enemyDeck.GenerateDeckByType(enemyType);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{enemyName} took {amount} damage! Current health: {currentHealth}");

        if (currentHealth <= 0) Die();
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
        Debug.Log($"{enemyName} healed {amount}, Current healt: {currentHealth}");
    }

    private void Die()
    {
        Debug.Log($"{enemyName} has died!");

        Destroy(gameObject, 1f);
        CombatSystem.instance.RemoveEnemyFromList(this);
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


