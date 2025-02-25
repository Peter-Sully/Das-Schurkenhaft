using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardSpawner : MonoBehaviour
{
    public GameObject cardPrefab;  // Assign your card prefab in the Inspector
    public Transform PlayerHand;  // Assign a UI panel or area where the card should appear
    public Card[] deck;
    public Text deckSizeText;

    void Start()
    {
        if (deckSizeText != null) deckSizeText.text = "DeckSize: " + deck.Length;

        ShuffleDeck();
        for (int i = 0; i < 5; i++)  // Draw 5 cards initially
        {
            DrawCard();
        }
    }

    void ShuffleDeck()
    {
        for (int i = 0; i < deck.Length; i++)
        {
            Card temp = deck[i];
            int randIndex = Random.Range(i, deck.Length);
            deck[i] = deck[randIndex];
            deck[randIndex] = temp;
        }
    }

    public void DrawCard()
    {
        if (deck.Length > 0)
        {
            Card drawnCard = deck[0];
            deck = RemoveCardFromDeck(deck, 0);
            SpawnCard(drawnCard);
            UpdateDeckSizeUI();
        }
        else
        {
            Debug.Log("Deck is empty!");
        }
    }

    public void SpawnCard(Card cardData)
    {
        if (cardPrefab == null || PlayerHand == null || cardData == null)
        {
            Debug.LogError("Missing references in CardSpawner!");
            return;
        }

        GameObject newCard = Instantiate(cardPrefab, PlayerHand);
        CardScript cardScript = newCard.GetComponent<CardScript>();

        if (cardScript != null)
        {
            cardScript.SetCard(cardData);  // Correct method call
        }
        else
        {
            Debug.LogError("CardScript is missing on the card prefab!");
        }
    }

    Card[] RemoveCardFromDeck(Card[] deckArray, int index)
    {
        List<Card> cardList = new List<Card>(deckArray);
        cardList.RemoveAt(index);
        return cardList.ToArray();
    }

    void UpdateDeckSizeUI()
    {
        if (deckSizeText != null) deckSizeText.text = "Deck Size: " + deck.Length;
    }
}
