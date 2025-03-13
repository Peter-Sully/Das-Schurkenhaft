using NUnit.Framework;
using UnityEngine.TestTools;
using System.Collections.Generic;
using UnityEngine;

public class DeckManagerTests
{
    private DeckManager deckManager;

    [SetUp]
    public void Setup()
    {
        GameObject gameObject = new GameObject();
        deckManager = gameObject.AddComponent<DeckManager>();

        GameObject playerHandObj = new GameObject("PlayerHand");
        playerHandObj.AddComponent<RectTransform>();
        deckManager.PlayerHand = playerHandObj.transform;

        GameObject cardPrefabObj = new GameObject("CardPrefab");
        cardPrefabObj.AddComponent<RectTransform>();
        cardPrefabObj.AddComponent<CardScript>();
        deckManager.cardPrefab = cardPrefabObj;
        deckManager.LoadDeck(); 
    }

    [Test]
    public void LoadDeck_TestWhenNoSavedData()
    {
        Assert.IsTrue(deckManager.deck.ContainsKey("Fireball"));
        Assert.AreEqual(3, deckManager.deck["Fireball"]);
    }

    [Test]
    public void ShuffleDeck_Test()
    {
        Dictionary<string, int> originalDeck = new Dictionary<string, int>(deckManager.deck);
        deckManager.ShuffleDeck();

        foreach (var card in originalDeck)
        {
            Assert.IsTrue(deckManager.deck.ContainsKey(card.Key));
            Assert.AreEqual(card.Value, deckManager.deck[card.Key]);
        }
    }

    [Test]
    public void DrawCard_RemoveCardFromDeck()
    {
        deckManager.deck["Fireball"] = 2;
        deckManager.DrawCard();

        Assert.LessOrEqual(deckManager.deck["Fireball"], 2);
    }

    [Test]
    public void DrawCard_HandlesEmptyDeck()
    {
        deckManager.deck.Clear();

        LogAssert.Expect(LogType.Log, "Deck is empty!");
        deckManager.DrawCard();
    }
}
