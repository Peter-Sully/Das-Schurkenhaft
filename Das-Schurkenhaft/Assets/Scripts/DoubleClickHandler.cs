using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DoubleClickHandler : MonoBehaviour
{
    // References
    public Transform inventoryParent; // Parent hold all 20 slots
    private Slot slot; // The current slot that is clicked

    public float doubleClickThreshold = 0.3f;
    private float lastClickTime = 0f;

    void Start()
    {
        slot = GetComponent<Slot>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsMouseOverUI())
            {
                if (Time.time - lastClickTime < doubleClickThreshold)
                {
                    AddItemToInventorySlot(); // Add the item to the inventory slot on double-click
                }
                lastClickTime = Time.time;
            }
        }
    }

   void AddItemToInventorySlot()
{
    if (slot == null || slot.item == null)
    {
        Debug.LogWarning("No item in this result slot!");
        return;
    }

    // Get the image's sprite name and item name from the prefab
    Sprite itemSprite = slot.GetComponent<Image>().sprite;
    string itemName = itemSprite.name; // Use sprite name as the item name
    GameObject itemPrefab = Resources.Load<GameObject>("Items/" + itemName);

    if (itemPrefab == null)
    {
        Debug.LogWarning("No prefab found with the name: " + itemName);
        return;
    }

    item itemScript = itemPrefab.GetComponent<item>();
    if (itemScript == null)
    {
        Debug.LogWarning("No 'Item' script attached to the prefab!");
        return;
    }

    string itemDescription = itemScript.GetDescription();

    Dictionary<string, int> recipeMaterials = GetRecipeMaterials(itemName);

    // Check if user has enough material
    bool hasEnoughMaterials = CheckMaterialsInInventory(recipeMaterials);
    if (!hasEnoughMaterials)
    {
        Debug.LogWarning("Not enough materials to craft " + itemName);
        return;
    }

    bool itemAdded = false;

    // Check for existing slack
    foreach (Transform child in inventoryParent)
    {
        ItemSlot existingSlot = child.GetComponent<ItemSlot>();
        if (existingSlot != null && existingSlot.itemName == itemName && !existingSlot.isFull)
        {
            // Add the item to the existing stack
            int leftoverItems = existingSlot.AddItem(itemName, 1, itemSprite, itemDescription);

            if (leftoverItems > 0)
            {
                Debug.LogWarning("There are leftover items: " + leftoverItems);
            }

            itemAdded = true;
            break;
        }
    }

    // if no existing stack, find empty slot
    if (!itemAdded)
    {
        foreach (Transform child in inventoryParent)
        {
            ItemSlot newSlot = child.GetComponent<ItemSlot>();
            if (newSlot != null && (newSlot.isFull == false || newSlot.itemName == ""))
            {
                if (newSlot.itemName == "" || newSlot.itemName == itemName)
                {
                    Debug.Log("Found empty or matching slot: " + newSlot.gameObject.name);
                    int leftoverItems = newSlot.AddItem(itemName, 1, itemSprite, itemDescription);

                    if (leftoverItems > 0)
                    {
                        Debug.LogWarning("There are leftover items: " + leftoverItems);
                    }

                    itemAdded = true;
                    break;
                }
            }
        }
    }

    // if not empty slot or the stack is full
    if (!itemAdded)
    {
        Debug.LogWarning("Inventory is full! Cannot add item: " + itemName);
    }

    // Remove material after crafting
    RemoveMaterialsFromInventory(recipeMaterials);
}

Dictionary<string, int> GetRecipeMaterials(string itemName)
{
    Dictionary<string, int> recipeMaterials = new Dictionary<string, int>();

    if (itemName == "paper")
    {
        recipeMaterials.Add("wood", 2);
    }
    else if (itemName =="ink") {
        recipeMaterials.Add("dye",1);
        recipeMaterials.Add("flower",1);
        recipeMaterials.Add("bottle",1);

    }
    else if (itemName == "bronzeshield") {
        recipeMaterials.Add("bronze",3);
    }
    else if (itemName == "silvershield") {
        recipeMaterials.Add("silver",3);
    }
    else if (itemName == "goldshield") {
        recipeMaterials.Add("gold",3);
    }
    else if (itemName == "potion") {
        recipeMaterials.Add("water",1);
        recipeMaterials.Add("flower",1);

    }
    else if (itemName == "card")
    {
        recipeMaterials.Add("paper", 1);
        recipeMaterials.Add("ink", 1);
        recipeMaterials.Add("gold", 2);
    }

    return recipeMaterials;
}

bool CheckMaterialsInInventory(Dictionary<string, int> requiredMaterials)
{
    foreach (var material in requiredMaterials)
    {
        int totalMaterialCount = 0;

        foreach (Transform child in inventoryParent)
        {
            ItemSlot existingSlot = child.GetComponent<ItemSlot>();
            if (existingSlot != null && existingSlot.itemName == material.Key)
            {
                totalMaterialCount += existingSlot.quantity;
            }
        }

        // If we don't have enough of the material, return false
        if (totalMaterialCount < material.Value)
        {
            return false;
        }
    }

    return true;  // If all materials are available
}

void RemoveMaterialsFromInventory(Dictionary<string, int> requiredMaterials)
{
    foreach (var material in requiredMaterials)
    {
        int materialCountToRemove = material.Value;

        foreach (Transform child in inventoryParent)
        {
            ItemSlot existingSlot = child.GetComponent<ItemSlot>();
            if (existingSlot != null && existingSlot.itemName == material.Key)
            {
                if (existingSlot.quantity >= materialCountToRemove)
                {
                    existingSlot.RemoveItem(materialCountToRemove);
                    materialCountToRemove = 0; // We've removed enough material, exit loop
                    break;
                }
                else
                {
                    materialCountToRemove -= existingSlot.quantity;  // Decrease the remaining amount to be removed
                    existingSlot.RemoveItem(existingSlot.quantity); // Remove all items in this slot
                }
            }
        }
    }
}

    bool IsMouseOverUI()
    {
        // Check if the current mouse position is inside the RectTransform bounds of the GameObject
        return RectTransformUtility.RectangleContainsScreenPoint(
            GetComponent<RectTransform>(),
            Input.mousePosition,
            Camera.main
        );
    }
}