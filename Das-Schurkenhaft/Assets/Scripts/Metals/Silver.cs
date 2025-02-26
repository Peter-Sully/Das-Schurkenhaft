using UnityEngine;

public class Silver : MonoBehaviour
{
    public int silverAmount = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MaterialManager.Instance.AddSilver(silverAmount);
            Debug.Log("Picked up " + silverAmount + " silver!");
            Destroy(gameObject);
        }
    }
}
