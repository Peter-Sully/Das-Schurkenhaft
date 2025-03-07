using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PreviewPanel : MonoBehaviour
{
    public Image cardImage; // Drag UI Image component here
    public TMP_Text cardNameText; // Drag TMP Text for name
    public TMP_Text cardDescriptionText; // Drag TMP Text for description
    public TMP_Text cardCostText;

    public void ShowCard(Card card)
    {
        if (card == null) return;
        
        cardImage.sprite = card.artwork;
        cardNameText.text = $"Name: {card.cardName}";
        cardDescriptionText.text = $"Description: {card.description}";
        cardCostText.text = $"Cost: {card.cost}";
        
        gameObject.SetActive(true); // Show the panel
    }

    public void HidePanel()
    {
        gameObject.SetActive(false); // Hide the panel when leaving deckbuilder
    }
}
