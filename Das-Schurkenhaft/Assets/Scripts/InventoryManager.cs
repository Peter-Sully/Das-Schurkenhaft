using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
   public GameObject InventoryMenu;
   public GameObject CraftingMenu;
   private bool menuActivated;
   private bool canvasActivated;
   public ItemSlot[] itemSlot;
   public ItemSO[] itemSOs;

   void Start() {

   }

   void Update() {
    if (Input.GetKeyDown(KeyCode.E) && menuActivated) {
        Time.timeScale = 1;
        InventoryMenu.SetActive(false); // Deactivates
        menuActivated = false;
    }
    else if (Input.GetKeyDown(KeyCode.E) && !menuActivated) {
        Time.timeScale = 0;
        InventoryMenu.SetActive(true); // Activates
        menuActivated = true;
    }
    if (Input.GetKeyDown(KeyCode.R) && canvasActivated) {
        Time.timeScale = 1;
        CraftingMenu.SetActive(false); // Deactivates
        canvasActivated = false;
    }
    else if (Input.GetKeyDown(KeyCode.R) && !canvasActivated) {
        Time.timeScale = 0;
        CraftingMenu.SetActive(true); // Activates
        canvasActivated = true;
    }
   }


   public void UseItem(string itemName) {
    for (int i = 0; i < itemSOs.Length; i++) {
        if(itemSOs[i].itemName == itemName) {
            itemSOs[i].UseItem();
        }
    }
   }
    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
{
    // Step 1: Try to add to an existing stack of the same item
    for (int i = 0; i < itemSlot.Length; i++)
    {
        if (!itemSlot[i].isFull && itemSlot[i].itemName == itemName) // Only stack with same item
        {
            int leftOverItems = itemSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription);
            if (leftOverItems <= 0) return 0; // If all items fit, return 0
            quantity = leftOverItems; // Update quantity with leftover
        }
    }

    // Step 2: Find a truly empty slot (i.e., no item at all)
    for (int i = 0; i < itemSlot.Length; i++)
    {
        if (string.IsNullOrEmpty(itemSlot[i].itemName)) // Slot is completely empty
        {
            return itemSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription);
        }
    }

    // Step 3: If no space left, return remaining quantity
    Debug.LogWarning("Inventory full! Could not add " + quantity + "x " + itemName);
    return quantity;
}


    public void DeselectAllSlots() {
        for (int i = 0; i < itemSlot.Length; i++) {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }
    }
}