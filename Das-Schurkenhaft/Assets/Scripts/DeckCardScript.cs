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
<<<<<<< HEAD
        DeckUI deckUI = Object.FindFirstObjectByType<DeckUI>();

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (deckUI != null) deckUI.OnCardClicked(cardData, transform.parent);
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (deckUI != null) deckUI.OnCardRightClicked(cardData);
        }
=======
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            DeckUI deckUI = Object.FindFirstObjectByType<DeckUI>();
            if (deckUI != null) deckUI.OnCardClicked(cardData, transform.parent);
        }
>>>>>>> 55ee72da2acb38afcba78e00e31633f24bdb229b
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
