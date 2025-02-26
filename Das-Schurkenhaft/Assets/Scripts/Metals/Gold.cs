using UnityEngine;

public class Gold : MonoBehaviour
{
    public int goldAmount = 10; // Amount of gold added

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MaterialManager.Instance.AddGold(goldAmount);
            Debug.Log("Picked up " + goldAmount + " gold!");

            // Destroy the gold pickup object
            Destroy(gameObject);
        }
    }
}
