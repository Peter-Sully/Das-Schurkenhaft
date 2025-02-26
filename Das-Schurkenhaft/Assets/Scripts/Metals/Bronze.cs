using UnityEngine;

public class Bronze : MonoBehaviour
{
    public int bronzeAmount = 10; // Amount of bronze added

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MaterialManager.Instance.AddBronze(bronzeAmount);
            Debug.Log("Picked up " + bronzeAmount + " bronze!");

            // Destroy the gold pickup object
            Destroy(gameObject);
        }
    }
}
