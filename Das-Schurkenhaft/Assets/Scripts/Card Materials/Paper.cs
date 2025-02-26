using UnityEngine;

public class Paper : MonoBehaviour
{
    public int paperAmount = 1;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MaterialManager.Instance.AddPaper(paperAmount);
            Debug.Log("Picked up " + paperAmount + " paper!");
            Destroy(gameObject);
        }
    }
}
