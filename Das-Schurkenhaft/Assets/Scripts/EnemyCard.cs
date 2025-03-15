using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyCard", menuName = "Enemy Card", order = 0)]
public class EnemyCard : ScriptableObject
{
    public string cardType;
    public int cardValue;

    

    public void playCard() {
        if (EnemyCombat.instance == null) {
        Debug.LogError("EnemyCombat.instance is null!");
        return; // Early exit if instance is not set
        }
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
