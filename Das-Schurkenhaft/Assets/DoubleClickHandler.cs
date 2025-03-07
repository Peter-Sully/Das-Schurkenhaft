using UnityEngine;
using UnityEngine.UI;

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

    // Get the image's sprite name
    Sprite itemSprite = slot.GetComponent<Image>().sprite;
    Debug.Log(itemSprite.name);
    string itemName = itemSprite.name; // Use sprite name as the item name

    // Load the prefab with the same name as the sprite from Resources
    GameObject itemPrefab = Resources.Load<GameObject>("Items/" + itemName);
    if (itemPrefab == null)
    {
        Debug.LogWarning("No prefab found with the name: " + itemName);
        return;
    }

    // Get the item script from the prefab (assumes the prefab has an 'item' script attached)
    item itemScript = itemPrefab.GetComponent<item>();
    if (itemScript == null)
    {
        Debug.LogWarning("No 'item' script attached to the prefab!");
        return;
    }

    // Get the item details from the prefab
    Sprite prefabSprite = itemScript.GetSprite();
    string itemDescription = itemScript.GetDescription();

    // Now you have the prefab's details, and you can proceed to add it to the inventory
    foreach (Transform child in inventoryParent)
    {
        ItemSlot itemSlot = child.GetComponent<ItemSlot>();
        if (itemSlot != null && !itemSlot.isFull)
        {
            // Debug if an available slot is found
            Debug.Log("Found available slot: " + itemSlot.gameObject.name);

            // Add the item to the found slot (adding 1 quantity)
            int leftoverItems = itemSlot.AddItem(itemName, 1, prefabSprite, itemDescription);
            
            // If there are leftover items, handle them (you can either discard them or find another slot)
            if (leftoverItems > 0)
            {
                Debug.LogWarning("There are leftover items: " + leftoverItems);
                // You can either add them to another available slot or handle them as needed.
            }
            break; // Exit the loop once the item is added
        }
        else
        {
            // Debug if the slot is full or not found
            Debug.Log("Slot is full or invalid: " + child.name);
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
