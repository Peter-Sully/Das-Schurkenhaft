using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatSystemTests
{
    private GameObject combatSystemObject;
    private CombatSystem combatSystem;

    [SetUp]
    public void SetUp()
    {
        combatSystemObject = new GameObject("CombatSystem");
        combatSystem = combatSystemObject.AddComponent<CombatSystem>();
        CombatSystem.instance = combatSystem;

        combatSystem.maxHealth = 100;
        combatSystem.playerHealth = combatSystem.maxHealth;
        combatSystem.playerShield = 0;
        combatSystem.maxEnergy = 10;
        combatSystem.playerEnergy = 5;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(combatSystemObject);
        CombatSystem.instance = null;
    }
    
    [Test]
    public void SpendEnergy_ReducesEnergy_WhenEnoughAvailable()
    {
        // Arrange
        combatSystem.playerEnergy = 5;

        // Act
        bool result = combatSystem.SpendEnergy(3);

        // Assert
        Assert.IsTrue(result, "SpendEnergy should return true if enough energy is available.");
        Assert.AreEqual(2, combatSystem.playerEnergy, "Energy did not decrease correctly.");
    }

    [Test]
    public void SpendEnergy_Fails_WhenNotEnoughEnergy()
    {
        // Arrange
        combatSystem.playerEnergy = 2;

        // Act
        bool result = combatSystem.SpendEnergy(5);

        // Assert
        Assert.IsFalse(result, "SpendEnergy should return false if not enough energy is available.");
        Assert.AreEqual(2, combatSystem.playerEnergy, "Energy should not decrease when insufficient energy.");
    }

    [Test]
    public void AddShield_IncreasesShield_UntilMax()
    {
        // Arrange
        combatSystem.playerShield = 5;
        
        // Act
        combatSystem.AddShield(10);

        // Assert
        Assert.AreEqual(15, combatSystem.playerShield, "Shield should increase when added.");
    }

    [Test]
    public void AddShield_CapsAtMaxHealth()
    {
        // Arrange
        combatSystem.playerShield = combatSystem.maxHealth - 5;

        // Act
        combatSystem.AddShield(10);

        // Assert
        Assert.AreEqual(combatSystem.maxHealth, combatSystem.playerShield, "Shield should cap at max health.");
    }

    [Test]
    public void HealPlayer_IncreasesHealth_UntilMax()
    {
        // Arrange
        combatSystem.playerHealth = 50;

        // Act
        combatSystem.HealPlayer(30);

        // Assert
        Assert.AreEqual(80, combatSystem.playerHealth, "Healing should increase player health correctly.");
    }

    [Test]
    public void HealPlayer_DoesNotExceedMaxHealth()
    {
        // Arrange
        combatSystem.playerHealth = 95;

        // Act
        combatSystem.HealPlayer(10);

        // Assert
        Assert.AreEqual(100, combatSystem.playerHealth, "Healing should not exceed max health.");
    }
}
