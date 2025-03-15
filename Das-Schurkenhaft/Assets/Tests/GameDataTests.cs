using NUnit.Framework;
using UnityEngine;

public class GameDataTests
{
    [Test]
    public void PlayerHealth_InitialValue_IsTen()
    {
        int expectedHealth = 10;

        int actualHealth = GameData.playerHealth;

        Assert.AreEqual(expectedHealth, actualHealth, "Initial player health should be 10.");
    }

    [Test]
    public void PlayerPosition_InitialValue_IsZero()
    {
        Vector2 expectedPosition = Vector2.zero;

        Vector2 actualPosition = GameData.playerPosition;

        Assert.AreEqual(expectedPosition, actualPosition, "Initial player position should be (0, 0).");
    }

    [Test]
    public void PlayerHealth_CanBeModified()
    {
        int newHealth = 5;

        GameData.playerHealth = newHealth;

        Assert.AreEqual(newHealth, GameData.playerHealth, "Player health should be updated to the new value.");
    }

    [Test]
    public void PlayerPosition_CanBeModified()
    {
        Vector2 newPosition = new Vector2(10, 20);

        GameData.playerPosition = newPosition;

        Assert.AreEqual(newPosition, GameData.playerPosition, "Player position should be updated to the new value.");
    }

    [TearDown]
    public void TearDown()
    {
        // Reset the static fields to their initial values after each test
        GameData.playerHealth = 10;
        GameData.playerPosition = Vector2.zero;
    }
}