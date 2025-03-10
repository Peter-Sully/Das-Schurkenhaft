using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DeckManagerTests
{
    [Test]
    public void LoadDeck_AssignsStarterDeck_WhenNoSavedData()
    {
        // Arrange
        DeckManager deckManager = new GameObject().AddComponent<DeckManager>();

        // Act
        deckManager.LoadDeck();  

        // Assert
        Assert.IsTrue(deckManager.deck.ContainsKey("Fireball"));
        Assert.AreEqual(3, deckManager.deck["Fireball"]);
    }
}
