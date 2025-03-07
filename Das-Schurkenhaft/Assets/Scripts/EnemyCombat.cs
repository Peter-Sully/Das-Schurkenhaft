using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public string enemyName = "Dummy_Enemy";
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{enemyName} took {amount} damage! Current health: {currentHealth}");

        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        Debug.Log($"{enemyName} has died!");

        Destroy(gameObject, 1f);
        CombatSystem.instance.RemoveEnemyFromList(this);
    }

    public void TakeTurn()
    {
        Debug.Log("Nothing for now in enemy turn");
    }
}
