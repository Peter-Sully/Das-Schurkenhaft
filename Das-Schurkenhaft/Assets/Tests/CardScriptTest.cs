using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

[TestFixture]
public class CardScriptTests
{
    private GameObject cardObject;
    private CardScript cardScript;
    private Card testCard;

    [SetUp]
    public void SetUp()
    {
        // Create a new GameObject and attach the CardScript
        cardObject = new GameObject("TestCard");
        cardScript = cardObject.AddComponent<CardScript>();

        // Add UI components
        cardObject.AddComponent<Image>(); // Required for cardImage
        cardScript.cardNameText = new GameObject("CardNameText").AddComponent<Text>();
        cardScript.descriptionText = new GameObject("DescriptionText").AddComponent<Text>();
        cardScript.costText = new GameObject("CostText").AddComponent<Text>();

        // Create a mock card
        testCard = ScriptableObject.CreateInstance<Card>();
        testCard.cardName = "Fireball";
        testCard.description = "Deals 10 damage.";
        testCard.cost = 3;
    }

    [Test]
    public void SetCard_UpdatesUI_Correctly()
    {
        // Act
        cardScript.SetCard(testCard);

        // Assert
        Assert.AreEqual("Fireball", cardScript.cardNameText.text);
        Assert.AreEqual("Deals 10 damage.", cardScript.descriptionText.text);
        Assert.AreEqual("3", cardScript.costText.text);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(cardObject);
        Object.DestroyImmediate(testCard);
    }
}
