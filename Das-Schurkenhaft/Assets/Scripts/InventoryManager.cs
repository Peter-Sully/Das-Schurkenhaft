using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
   public GameObject InventoryMenu;
   public GameObject CraftingMenu;
   private bool menuActivated;
   private bool canvasActivated;
   public ItemSlot[] itemSlot;
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

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription) {
        for (int i = 0; i < itemSlot.Length; i++) {
            if(itemSlot[i].isFull == false && itemSlot[i].itemName == itemName || itemSlot[i].quantity == 0) {
                int leftOverItems = itemSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription);
                if(leftOverItems > 0) {
                    leftOverItems = AddItem(itemName, leftOverItems, itemSprite, itemDescription);
                }
                return leftOverItems;
            }
        }
        return quantity;
    }

    public void DeselectAllSlots() {
        for (int i = 0; i < itemSlot.Length; i++) {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }
    }
}