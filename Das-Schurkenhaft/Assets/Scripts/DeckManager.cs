using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab;  // Assign your card prefab in the Inspector
    public Transform PlayerHand;  // Assign a UI panel or area where the card should appear
    public Text deckSizeText;
    public Dictionary<string, int> deck = new Dictionary<string, int>();

    public void LoadDeck()
    {
        deck.Clear();

        string savedDeckData = PlayerPrefs.GetString("SavedDeck", "");
        Debug.Log("Loaded Deck Data: " + savedDeckData);

        if (!string.IsNullOrEmpty(savedDeckData))
        {
            foreach (string entry in savedDeckData.Split(','))
            {
                string[] parts = entry.Split(':');
                if (parts.Length == 2)
                {
                    string cardName = parts[0];
                    int count = int.Parse(parts[1]);
                    deck[cardName] = count;
                }
            }
        }

        if (deck.Count == 0)
        {
            AddCardsToDict(deck, "Fireball", 3);
            AddCardsToDict(deck, "Mend", 3);
            AddCardsToDict(deck, "ShieldUp", 3);

        }
    }

    public void AddCardsToDict(Dictionary<string, int> cardDict, string cardName, int count)
    {
        if (cardDict.ContainsKey(cardName))
        {
            cardDict[cardName] += count;
        }
        else
        {
            cardDict.Add(cardName, count);
        }
    }

    public void ShuffleDeck()
    {
        List<string> deckList = new List<string>();
        foreach (var card in deck)
        {
            for (int i = 0; i < card.Value; i++)
            {
                deckList.Add(card.Key);
            }
        }

        for (int i = 0; i < deckList.Count; i++)
        {
            string temp = deckList[i];
            int randIndex = Random.Range(i, deckList.Count);
            deckList[i] = deckList[randIndex];
            deckList[randIndex] = temp;
        }
    }

    public void DrawCard()
    {
        if (deck.Count == 0)
        {
            Debug.Log("Deck is empty!");
            return;
        }

        // Pick a random card type from the dictionary
        List<string> cardNames = new List<string>(deck.Keys);
        string randomCardName = cardNames[Random.Range(0, cardNames.Count)];

        // Find the corresponding Card object (assuming you have a way to map names to actual Card objects)
        Card drawnCard = FindCardByName(randomCardName);

        if (drawnCard != null)
        {
            SpawnCard(drawnCard);
            deck[randomCardName]--;

            // Remove from dictionary if count reaches 0
            if (deck[randomCardName] <= 0)
            {
                deck.Remove(randomCardName);
            }

            UpdateDeckSizeUI();
            UpdateCardLayoutUI();
        }
        else
        {
            Debug.LogError("Card not found: " + randomCardName);
        }
    }

    public Card FindCardByName(string cardName)
    {
        Card card = Resources.Load<Card>("Cards/" + cardName);
        if (card == null) Debug.LogError("Card not found: " + cardName);
        return card;
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
            cardScript.SetCard(cardData); 
        }
        else
        {
            Debug.LogError("CardScript is missing on the card prefab!");
        }
    }

    public void UpdateDeckSizeUI()
    {
        if (deckSizeText != null) deckSizeText.text = "Deck Size: " + deck.Count;
    }

    public void UpdateCardLayoutUI()
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
