using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeckCardScript : MonoBehaviour, IPointerClickHandler
{
    public Card cardData;
    private Image cardImage;
    public Text cardNameText;
    public Text descriptionText;
    public Text costText;

    void Start()
    {
        cardImage = GetComponent<Image>();  
        UpdateCardUI();
    }

    public void SetCard(Card newCard)
    {
        cardData = newCard;
        UpdateCardUI();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            DeckUI deckUI = Object.FindFirstObjectByType<DeckUI>();
            if (deckUI != null) deckUI.OnCardClicked(cardData, transform.parent);
        }
    }

    private void UpdateCardUI() 
    {
        if (cardData == null) return;

        if (cardImage && cardData.artwork) cardImage.sprite = cardData.artwork;
        if (cardNameText) cardNameText.text = cardData.cardName;
        if (descriptionText) descriptionText.text = cardData.description;
        if (costText) costText.text = cardData.cost.ToString();
    }
}
