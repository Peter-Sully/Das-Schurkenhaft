using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class item : MonoBehaviour {
[SerializeField]
private string itemName;

[SerializeField]
private int quantity;

[SerializeField]
private Sprite sprite;

[TextArea]
[SerializeField]
private string itemDescription;

private InventoryManager inventoryManager;

void Start() {
    inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
}

private void OnCollisionEnter2D(Collision2D collision)
{
    if(collision.gameObject.tag =="Player") {
        int leftOverItems = inventoryManager.AddItem(itemName, quantity, sprite, itemDescription);
        if(leftOverItems <= 0) {
            Destroy(gameObject);
        }
        else {
            quantity = leftOverItems;
        }
    }
}
public string GetName() {
    return itemName;
}
public int GetQuantity() {
    return quantity;
}
public Sprite GetSprite() {
    return sprite;
}
public string GetDescription() {
    return itemDescription;
}

}