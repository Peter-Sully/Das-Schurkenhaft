using UnityEngine;
using System.Collections.Generic;

public class EnemyDeck : MonoBehaviour
{
    public List<EnemyCard> cards;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void GenerateDeckByType(string type)
    {
        cards = new List<EnemyCard>();
        if (type == "Soldier")
        {
            GenerateSoldierDeck();
        }
        else if (type == "Vampire")
        {
            GenerateVampireDeck();
        }
        cards.Add(new EnemyCard { cardValue = 10, cardType = "Heal" });


    }   
    
    private void GenerateSoldierDeck()
    {
        cards.Add(new EnemyCard { cardValue = 10, cardType = "Damage" });
        cards.Add(new EnemyCard { cardValue = 10, cardType = "Damage" });
        cards.Add(new EnemyCard { cardValue = 10, cardType = "Damage" });
        cards.Add(new EnemyCard { cardValue = 5, cardType = "Damage" });
        cards.Add(new EnemyCard { cardValue = 5, cardType = "Damage" });
        cards.Add(new EnemyCard { cardValue = 5, cardType = "Damage" });
        cards.Add(new EnemyCard { cardValue = 5, cardType = "Damage" });
        cards.Add(new EnemyCard { cardValue = 5, cardType = "Damage" });
        cards.Add(new EnemyCard { cardValue = 5, cardType = "Damage" });
        cards.Add(new EnemyCard { cardValue = 5, cardType = "Damage" });
    }

    public void GenerateVampireDeck() {
        cards.Add(new EnemyCard { cardValue = 10, cardType = "Heal" });
        cards.Add(new EnemyCard { cardValue = 10, cardType = "Heal" });
        cards.Add(new EnemyCard { cardValue = 10, cardType = "Heal" });
        cards.Add(new EnemyCard { cardValue = 5, cardType = "Heal" });
        cards.Add(new EnemyCard { cardValue = 5, cardType = "Damage" });
        cards.Add(new EnemyCard { cardValue = 5, cardType = "Damage" });
        cards.Add(new EnemyCard { cardValue = 5, cardType = "Damage" });
        cards.Add(new EnemyCard { cardValue = 5, cardType = "Damage" });
        cards.Add(new EnemyCard { cardValue = 5, cardType = "Damage" });
        cards.Add(new EnemyCard { cardValue = 5, cardType = "Damage" });
    }
    public EnemyCard drawCard() {
        if (cards.Count > 0) {
            int randomIndex = Random.Range(0, cards.Count);
            EnemyCard drawnCard = cards[randomIndex];
            cards.RemoveAt(randomIndex);
            return drawnCard;
        }
        return null;
    }
    
    
}
