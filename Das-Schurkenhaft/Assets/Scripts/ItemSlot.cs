using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    // Item Data
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;
    public Sprite emptySprite;
    [SerializeField]
    private int maxNumberOfItems;
    // Item Slot
    [SerializeField]
    private TMP_Text quantityText;
    [SerializeField]
    private Image itemImage;

    // Item Description Slot
    public Image itemDescriptionImage;
    public TMP_Text ItemDescriptionNameText;
    public TMP_Text ItemDescriptionText;

    public GameObject selectedShader;
    public bool thisItemSelected;

    private InventoryManager inventoryManager;

    private void Start() {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    // AddItem to next available slot method
    public int AddItemToNextAvailableSlot(string itemName, int quantity, Sprite itemSprite, string itemDescription) {
        int leftovers = quantity;

        // Check through all slots in the inventory to find a suitable slot for overflow
        foreach (var slot in inventoryManager.itemSlot) {
            if (slot.itemName == itemName && !slot.isFull) {
                // Try adding to this slot
                leftovers = slot.AddItem(itemName, leftovers, itemSprite, itemDescription);

                // If there are no leftovers, break the loop
                if (leftovers == 0) break;
            }
        }

        // If there are still leftovers, place them in the first available empty slot
        if (leftovers > 0) {
            foreach (var slot in inventoryManager.itemSlot) {
                if (slot.itemName == "" || !slot.isFull) {
                    leftovers = slot.AddItem(itemName, leftovers, itemSprite, itemDescription);
                    if (leftovers == 0) break;
                }
            }
        }

        return leftovers;
    }

    // AddItem Method
    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription) {
        // Try to add items to the current slot
        if (isFull) {
            // Slot is full, check next available slot
            return quantity;  // Return leftover quantity
        }

        // If current slot has space, add items here
        this.itemName = itemName;
        this.itemSprite = itemSprite;
        itemImage.enabled = true;
        itemImage.sprite = itemSprite;
        this.itemDescription = itemDescription;

        this.quantity += quantity;

        // Check if the quantity exceeds the maximum and overflow
        if (this.quantity >= maxNumberOfItems) {
            quantityText.text = maxNumberOfItems.ToString();
            quantityText.enabled = true;
            isFull = true;

            // Overflow logic: Calculate extra items
            int extraItems = this.quantity - maxNumberOfItems;
            this.quantity = maxNumberOfItems;
            return extraItems;
        }

        
        // Normal case: update the quantity text
        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;
        return 0;
    }

    public void RemoveItem(int quantityToRemove)
{
    // Check if the current slot has enough quantity to remove
    if (quantityToRemove > quantity)
    {
        Debug.LogWarning("Trying to remove more items than are available in the slot!");
        return;
    }

    // Subtract the quantity of items in this slot
    quantity -= quantityToRemove;

    // Update the quantity text
    quantityText.text = quantity.ToString();

    // If the quantity reaches 0, clear the slot
    if (quantity == 0)
    {
        // Reset item data
        itemName = "";
        itemSprite = null;
        itemDescription = "";
        itemImage.enabled = false; // Hide the image
        quantityText.enabled = false; // Hide the quantity text
        isFull = false; // Mark as not full
        selectedShader.SetActive(false); // Deselect the slot
    }
}

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right) {
            OnRightClick();
        }
    }

    public void OnLeftClick () {
        inventoryManager.DeselectAllSlots();
        selectedShader.SetActive(true);
        thisItemSelected = true;
        ItemDescriptionNameText.text = itemName;
        ItemDescriptionText.text = itemDescription;
        itemDescriptionImage.sprite = itemSprite;
        if (itemDescriptionImage.sprite == null) {
            itemDescriptionImage.sprite = emptySprite;
        }
    }

   public void OnRightClick() 
{
    // Check if there is an item in this slot
    if (itemName == "") return;

    // Fetch the corresponding ItemSO from InventoryManager
    ItemSO itemData = inventoryManager.GetItemData(itemName);
    
    // If itemData is found and is usable, apply the effect
    if (itemData != null && itemData.isUsable) 
    {
        PlayerController player = GameObject.Find("Player").GetComponent<PlayerController>();
        itemData.UseItem(player);

        // Remove one item after use
        RemoveItem(1);
    }
    else 
    {
        Debug.Log(itemName + " cannot be used.");
    }
}

}
