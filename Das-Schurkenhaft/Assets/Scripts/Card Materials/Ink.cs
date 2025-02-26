using UnityEngine;

public class Ink : MonoBehaviour
{
    public int inkAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MaterialManager.Instance.AddInk(inkAmount);
            Debug.Log("Picked up " + inkAmount + " ink!");
            Destroy(gameObject);
        }
    }
}
