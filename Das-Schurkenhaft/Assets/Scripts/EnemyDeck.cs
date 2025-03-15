using UnityEngine;
using System.Collections.Generic;

public class EnemyDeck : MonoBehaviour
{
    public List<EnemyCard> cards;
    public string EnemyType;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void GenerateDeckByType(string type)
    {
        cards = new List<EnemyCard>();
        if (type == "SPUM_20240911215637878 1(Clone)") 
        {
            GenerateSoldierDeck();
        }
        else if (type == "Vampire")
        {
            GenerateVampireDeck();
        } else {
            Debug.Log("Invalid enemy type");
        }


    }   
    
    private void GenerateSoldierDeck()
    {
        cards.Add(Resources.Load<EnemyCard>("Cards/SoldierCard1"));
        cards.Add(Resources.Load<EnemyCard>("Cards/SoldierCard2"));
        cards.Add(Resources.Load<EnemyCard>("Cards/SoldierCard3"));
        cards.Add(Resources.Load<EnemyCard>("Cards/SoldierCard4"));
        cards.Add(Resources.Load<EnemyCard>("Cards/SoldierCard5"));
        cards.Add(Resources.Load<EnemyCard>("Cards/SoldierCard6"));
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
