using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DoubleClickHandler : MonoBehaviour
{
    // References
    public Transform inventoryParent; // The parent that holds all inventory slots (the 20 slots container)
    private Slot slot; // The current slot that is clicked (result slot)

    public float doubleClickThreshold = 0.3f; // Time between two clicks to be considered a double-click
    private float lastClickTime = 0f; // The last time the slot was clicked

    void Start()
    {
        slot = GetComponent<Slot>(); // Get the Slot script attached to this GameObject (result slot)
    }

    void Update()
    {
        // Detect mouse click (left click)
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            if (IsMouseOverUI()) // Check if we are clicking on the UI element
            {
                // Check if the click is within the double-click threshold time
                if (Time.time - lastClickTime < doubleClickThreshold)
                {
                    AddItemToInventorySlot(); // Add the item to the inventory slot on double-click
                }
                lastClickTime = Time.time; // Update the last click time
            }
        }
    }

   void AddItemToInventorySlot()
{
    // Ensure the result slot has an item to add
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

    // Get the item script from the prefab (assumes the prefab has an 'Item' script attached)
    item itemScript = itemPrefab.GetComponent<item>();
    if (itemScript == null)
    {
        Debug.LogWarning("No 'Item' script attached to the prefab!");
        return;
    }

    string itemDescription = itemScript.GetDescription();

    // Example: Assume you have predefined recipes that specify the required materials (e.g., "Wood")
    Dictionary<string, int> recipeMaterials = GetRecipeMaterials(itemName);  // Get required materials for crafting

    // 1️⃣ **Check if the player has enough materials**
    bool hasEnoughMaterials = CheckMaterialsInInventory(recipeMaterials);
    if (!hasEnoughMaterials)
    {
        Debug.LogWarning("Not enough materials to craft " + itemName);
        return;
    }

    bool itemAdded = false;

    // 2️⃣ **First, Check for an Existing Stack of the Same Item**
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

    // 3️⃣ **If No Existing Stack is Found, Find an Empty Slot**
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

    // 4️⃣ **If No Empty Slot or Stack is Found**
    if (!itemAdded)
    {
        Debug.LogWarning("Inventory is full! Cannot add item: " + itemName);
    }

    // 5️⃣ **Remove the materials from inventory after crafting**
    RemoveMaterialsFromInventory(recipeMaterials);
}

// Function to get the materials required for a specific recipe (could be fetched from a recipe manager or hardcoded)
Dictionary<string, int> GetRecipeMaterials(string itemName)
{
    Dictionary<string, int> recipeMaterials = new Dictionary<string, int>();

    // Example: Hardcoded recipes for Paper (requires 2 wood) and Card (requires 1 paper, 1 ink, and 2 gold)
    if (itemName == "paper")
    {
        recipeMaterials.Add("wood", 2);  // 2 Wood required
    }
    else if (itemName == "Card")
    {
        recipeMaterials.Add("paper", 1);  // 1 Paper required
        recipeMaterials.Add("ink", 1);    // 1 Ink required
        recipeMaterials.Add("gold", 2);   // 2 Gold required
    }

    return recipeMaterials;
}

// Function to check if the player has enough materials in their inventory
bool CheckMaterialsInInventory(Dictionary<string, int> requiredMaterials)
{
    foreach (var material in requiredMaterials)
    {
        int totalMaterialCount = 0;

        // Check the inventory for the required material and count how many we have
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

// Function to remove materials from inventory after crafting
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
                    existingSlot.RemoveItem(materialCountToRemove); // This function would need to be implemented in your ItemSlot script
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



    // Check if mouse is over the UI element (result image)
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