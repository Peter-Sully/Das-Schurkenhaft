using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class DeckUI : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform deckPanel;
    public Transform allCardsPanel;
    public PreviewPanel previewPanel;
    public Dictionary<string, int> deck = new Dictionary<string, int>();
    public Dictionary<string, int> allCards = new Dictionary<string, int>();

    void Start()
    {
        LoadCardData();
        //deck.Clear();
        //allCards.Clear();
        //InitializeTestCards();
        SaveCardData();
        SpawnCards();
        UpdateCardLayoutUI();
    }

    void InitializeTestCards() {
        AddCardsToDict(allCards, "Fireball", 5);
        AddCardsToDict(allCards, "Mend", 3);
        AddCardsToDict(allCards, "ShieldUp", 4);
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

    public void SaveCardData()
    {
        string deckData = string.Join(",", deck.Select(kv => kv.Key + ":" + kv.Value));
        string allCardsData = string.Join(",", allCards.Select(kv => kv.Key + ":" + kv.Value));

        PlayerPrefs.SetString("SavedDeck", deckData);
        PlayerPrefs.SetString("SavedAllCards", allCardsData);
        PlayerPrefs.Save();
        Debug.Log("Card Data Saved.");
    }

    public void LoadCardData()
    {
        string savedDeckData = PlayerPrefs.GetString("SavedDeck", "");
        string savedAllCardsData = PlayerPrefs.GetString("SavedAllCards", "");

        Debug.Log("Loaded Deck Data: " + savedDeckData);
        Debug.Log("Loaded All Cards Data: " + savedAllCardsData);

        deck.Clear();
        allCards.Clear();

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

        if (!string.IsNullOrEmpty(savedAllCardsData))
        {
            foreach (string entry in savedAllCardsData.Split(','))
            {
                string[] parts = entry.Split(':');
                if (parts.Length == 2)
                {
                    string cardName = parts[0];
                    int count = int.Parse(parts[1]);
                    allCards[cardName] = count;
                }
            }
        }
    }

    public Card FindCardByName(string cardName)
    {
        Card card = Resources.Load<Card>("Cards/" + cardName);
        if (card == null) Debug.LogError("Card not found: " + cardName);
        return card;
    }

    public void SpawnCards()
    {
        // Spawn deck cards
        foreach (var kv in deck)
        {
            string cardName = kv.Key;
            int count = kv.Value;
            Card card = FindCardByName(cardName);

            if (card == null) 
            {
                Debug.LogError("Card is null: " + cardName);
                continue;
            }

            for (int i = 0; i < count; i++)
            {
                SpawnCard(card, deckPanel);
            }
        }

        // Spawn all available cards
        foreach (var kv in allCards)
        {
            string cardName = kv.Key;
            int count = kv.Value;
            Card card = FindCardByName(cardName);

            if (card == null) 
            {
                Debug.LogError("Card is null: " + cardName);
                continue;
            }

            for (int i = 0; i < count; i++)
            {
                SpawnCard(card, allCardsPanel);
            }
        }
        Debug.Log("Layout made");

    }

    public void SpawnCard(Card card, Transform panel)
    {
        if (cardPrefab == null || panel == null || card == null)
        {
            Debug.LogError("Missing references in SpawnCard!");
            if (cardPrefab == null) Debug.LogError("Missing prefab");
            if (panel == null) Debug.LogError("Missing panel");
            if (card == null) Debug.LogError("Missing card");
            return;
        }

        GameObject newCard = Instantiate(cardPrefab, panel);
        DeckCardScript cardScript = newCard.GetComponent<DeckCardScript>();

        if (cardScript != null)
        {
            cardScript.SetCard(card);
        }
        else
        {
            Debug.LogError("DeckCardScript is missing on the card prefab!");
        }
    }

    public void OnCardClicked(Card card, Transform curPanel)
    {
        string cardName = card.cardName;
        int totalCardsInDeck = deck.Values.Sum();

        if (curPanel == deckPanel) // Move from deck to allCards
        {
            if (deck.ContainsKey(cardName))
            {
                deck[cardName]--;
                if (deck[cardName] <= 0)
                    deck.Remove(cardName);
            }

            if (allCards.ContainsKey(cardName))
                allCards[cardName]++;
            else
                allCards[cardName] = 1;
        }
        else if (curPanel == allCardsPanel) // Move from allCards to deck
        {
            if (totalCardsInDeck >= 20)
            {
                Debug.Log("Deck is full!");
                return;
            }
            if (allCards.ContainsKey(cardName))
            {
                allCards[cardName]--;
                if (allCards[cardName] <= 0)
                    allCards.Remove(cardName);
            }

            if (deck.ContainsKey(cardName))
                deck[cardName]++;
            else
            {
                deck[cardName] = 1;
            }
        }

        SaveCardData();
        Invoke(nameof(RefreshUI), 0.05f);
        //UpdateCardLayoutUI();
    }

    public void OnCardRightClicked(Card card)
    {
        if (previewPanel != null)
        {
            previewPanel.ShowCard(card);
        }
    }

    public void RefreshUI()
    {
        AdjustCardCount(deck, deckPanel);
        AdjustCardCount(allCards, allCardsPanel);
        UpdateCardLayoutUI();
        Debug.Log("Cards Updated");
    }

    void AdjustCardCount(Dictionary<string, int> cardDict, Transform panel)
    {
        // Track how many cards of each type exist in the panel
        Dictionary<string, List<Transform>> existingCards = new Dictionary<string, List<Transform>>();

        foreach (Transform child in panel)
        {
            DeckCardScript cardScript = child.GetComponent<DeckCardScript>();
            if (cardScript != null)
            {
                string cardName = cardScript.cardData.cardName;
                if (!existingCards.ContainsKey(cardName))
                {
                    existingCards[cardName] = new List<Transform>();
                }
                existingCards[cardName].Add(child);
            }
        }

        // Remove excess cards
        foreach (var kv in existingCards)
        {
            string cardName = kv.Key;
            List<Transform> cardInstances = kv.Value;

            int correctCount = cardDict.ContainsKey(cardName) ? cardDict[cardName] : 0;
            int excess = cardInstances.Count - correctCount;

            if (excess > 0)
            {
                for (int i = 0; i < excess; i++)
                {
                    Destroy(cardInstances[i].gameObject);
                }
            }
        }

        // Spawn missing cards
        foreach (var kv in cardDict)
        {
            string cardName = kv.Key;
            int correctCount = kv.Value;
            int existingCount = existingCards.ContainsKey(cardName) ? existingCards[cardName].Count : 0;

            int missingCount = correctCount - existingCount;
            if (missingCount > 0)
            {
                Card card = FindCardByName(cardName);
                for (int i = 0; i < missingCount; i++)
                {
                    SpawnCard(card, panel);
                }
            }
        }
    }


    public void UpdateCardLayoutUI()
    {
        LayoutCardsInPanel(deckPanel);
        LayoutCardsInPanel(allCardsPanel);
    }

    void LayoutCardsInPanel(Transform panel)
    {
        int cardCount = panel.childCount;
        if (cardCount == 0) return;

        float panelWidth = panel.GetComponent<RectTransform>().rect.width;
        float cardWidth = cardPrefab.GetComponent<RectTransform>().rect.width;
        
        float maxSpacing = cardWidth * 0.75f;  
        float overlapFactor = 0.4f;  

        float totalWidth = (cardCount - 1) * maxSpacing + cardWidth;
        float spacing = totalWidth > panelWidth 
            ? -cardWidth * overlapFactor  
            : maxSpacing;                 

        float startX = -((cardCount - 1) * spacing) / 2f;

        Debug.Log($"Panel: {panel.name}, CardCount: {cardCount}, Spacing: {spacing}, StartX: {startX}, PanelWidth: {panelWidth}");

        for (int i = 0; i < cardCount; i++)
        {
            RectTransform cardTransform = panel.GetChild(i).GetComponent<RectTransform>();
            cardTransform.anchoredPosition = new Vector2(startX + i * spacing, 0);
            
        }
    }
    
}
