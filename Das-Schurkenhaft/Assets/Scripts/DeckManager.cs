using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour
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
            UpdateCardLayoutUI();
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

    void UpdateCardLayoutUI()
    {
        int cardCount = PlayerHand.childCount;
        if (cardCount == 0) return;

        float panelWidth = PlayerHand.GetComponent<RectTransform>().rect.width;
        float cardWidth = cardPrefab.GetComponent<RectTransform>().rect.width;
        
        float maxSpacing = cardWidth * 0.75f;  // Default spacing (before overlap)
        float overlapFactor = 0.4f;  // Adjust how much cards overlap when hand is full

        // Calculate total width needed to place all cards with maxSpacing
        float totalWidth = (cardCount - 1) * maxSpacing + cardWidth;

        // If total width exceeds panel width, calculate overlap
        float spacing = totalWidth > panelWidth 
            ? -cardWidth * overlapFactor  // Overlapping mode
            : maxSpacing;                 // Normal spacing mode

        // Find start position (center first card, then shift left)
        float startX = -((cardCount - 1) * spacing) / 2f;

        // Position each card
        for (int i = 0; i < cardCount; i++)
        {
            RectTransform cardTransform = PlayerHand.GetChild(i).GetComponent<RectTransform>();
            cardTransform.anchoredPosition = new Vector2(startX + i * spacing, 0);
        }
    }
}
