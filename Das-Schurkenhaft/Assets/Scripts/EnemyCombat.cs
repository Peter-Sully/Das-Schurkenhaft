using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyCombat : MonoBehaviour
{
    public string enemyName = "Dummy_Enemy";
    public int maxHealth = 100;
    public int currentHealth;
    private Image healthBar;
    private TMP_Text healthtext;
    public bool isDead = false;

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
        Debug.Log("Nothing for now in enemy turn");
    }
}
