using UnityEngine;

public class Dye : MonoBehaviour
{
    public int dyeAmount = 1; // Amount of dye this pickup gives

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MaterialManager.Instance.AddDye(dyeAmount);
            Debug.Log("Picked up " + dyeAmount + " dye!");
            Destroy(gameObject);
        }
    }
}
