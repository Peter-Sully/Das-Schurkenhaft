using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CraftingManager : MonoBehaviour
{
    private Item currentItem;
    public Image customCursor;
    public Slot[] craftingSlots;
    public Canvas canvas;
    public List<Item> itemList;
    public string[] recipes;
    public Item[] recipeResults;
    public Slot resultSlot;

    private void Update() {
        if (Input.GetMouseButtonUp(0)) {
            if (currentItem != null) {
                customCursor.gameObject.SetActive(false);
                Slot nearestSlot = null;
                float shortestDistance = float.MaxValue;
                Vector2 mouseScreenPosition = Input.mousePosition;
                foreach (Slot slot in craftingSlots) {
                    RectTransform slotRectTransform = slot.GetComponent<RectTransform>();
                    Vector2 slotScreenPosition = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, slotRectTransform.position);
                    float distance = Vector2.Distance(mouseScreenPosition, slotScreenPosition);
                    if (distance < shortestDistance) {
                        shortestDistance = distance;
                        nearestSlot = slot;
                    }
                }
                if (nearestSlot != null) {
                    nearestSlot.gameObject.SetActive(true);
                    nearestSlot.GetComponent<Image>().sprite = currentItem.GetComponent<Image>().sprite;
                    nearestSlot.item = currentItem;
                    itemList[nearestSlot.index] = currentItem;
                    currentItem = null;  // Clear current item after placing
                    CheckForCompletedRecipes();
                }
            }
        }
    }

    void CheckForCompletedRecipes() {
        resultSlot.gameObject.SetActive(false);
        resultSlot.item = null;
        string currentRecipeString = "";
        foreach(Item item in itemList) {
            if (item != null) {
                currentRecipeString += item.itemName;
            }
            else {
                currentRecipeString += "null";
            }
        }

        for (int i = 0; i < recipes.Length; i++) {
            if(recipes[i] == currentRecipeString) {
                resultSlot.gameObject.SetActive(true);
                resultSlot.GetComponent<Image>().sprite = recipeResults[i].GetComponent<Image>().sprite;
                resultSlot.item = recipeResults[i];
            }
        }
    }

    public void OnClickSlot(Slot slot) {
        slot.item = null;
        itemList[slot.index] = null;
        slot.gameObject.SetActive(false);
        CheckForCompletedRecipes();
    }

    public void OnMouseDownItem(Item item) {
        if (currentItem == null) {
            currentItem = item;
            customCursor.gameObject.SetActive(true);
            customCursor.sprite = currentItem.GetComponent<Image>().sprite;
        }
    }
}
