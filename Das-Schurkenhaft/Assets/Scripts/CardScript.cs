using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardScript : MonoBehaviour, IPointerClickHandler
{
    public Card cardData;
    private Image cardImage;
    public Text cardNameText;
    public Text descriptionText;
    public Text costText;

    private Vector2 originalPosition;
    private Transform originalParent;

    void Start()
    {
        cardImage = GetComponent<Image>();  
        UpdateCardUI();

        originalPosition = transform.position;
        originalParent = transform.parent;
    }

    public void SetCard(Card newCard)
    {
        cardData = newCard;
        UpdateCardUI();
    }

    private void UpdateCardUI() 
    {
        if (cardData == null) return;

        if (cardImage && cardData.artwork) cardImage.sprite = cardData.artwork;
        if (cardNameText) cardNameText.text = cardData.cardName;
        if (descriptionText) descriptionText.text = cardData.description;
        if (costText) costText.text = cardData.cost.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (cardData == null)
            {
                Debug.LogError("Card data is NULL when clicked! Check if SetCard() was called.");
                return;
            }

            if (CardPreviewManager.instance == null)
            {
                Debug.LogError("CardPreviewManager instance is NULL! Make sure it's in the scene.");
                return;
            }

            Debug.Log($"Card clicked: {cardData.cardName}");
            CardPreviewManager.instance.ShowCard(this);
        }
    }
    
    public void PlayCard() 
    {
        Debug.Log($"Played {cardData.cardName}!");
        cardData.PlayCard();
        Destroy(gameObject);
    }

    public void ResetCardPosition() 
    {
        transform.SetParent(originalParent, true);
        transform.position = originalPosition;
    }
}
