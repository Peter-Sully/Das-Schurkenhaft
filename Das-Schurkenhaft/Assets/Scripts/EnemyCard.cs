using UnityEngine;

public class EnemyCard : MonoBehaviour
{
    public string cardType;
    public int cardValue;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void playCard() {
        if (cardType == "Damage") {
            EnemyCombat.instance.AttackPlayer(cardValue);
        } else if (cardType == "Heal") {
            EnemyCombat.instance.Heal(cardValue);
        } else {
            Debug.Log("Invalid card type");
        }
        Debug.Log("Card played");
    }
}
