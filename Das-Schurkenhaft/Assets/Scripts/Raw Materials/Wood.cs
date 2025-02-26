using UnityEngine;

public class Wood : MonoBehaviour
{
    public int woodAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MaterialManager.Instance.AddWood(woodAmount);
            Debug.Log("Picked up " + woodAmount + " wood!");
            Destroy(gameObject);
        }
    }
}
