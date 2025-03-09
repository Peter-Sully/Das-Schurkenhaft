using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardPreviewManager : MonoBehaviour, IPointerClickHandler
{
    public static CardPreviewManager instance; // Singleton for easy access
    public Image previewImage;  // UI Image to display the previewed card

    private CardScript currentCard; // Stores the currently previewed card

    void Awake()
    {
        GameObject cardPreviewPanel = GameObject.Find("CardPreview");

        if (cardPreviewPanel != null)
        {
            previewImage = cardPreviewPanel.GetComponent<Image>();
        }

        if (instance == null)
        {
            instance = this;
            Debug.Log("CardPreviewManager instance set successfully.");
        }
        else
        {
            Debug.LogWarning("Multiple instances of CardPreviewManager detected! Destroying duplicate.");
            Destroy(gameObject);
        }

        gameObject.SetActive(false); // Ensure preview starts hidden
    }

    public void ShowCard(CardScript cardScript)
    {
        if (cardScript == null || cardScript.cardData == null) return;

        currentCard = cardScript;

        previewImage.sprite = cardScript.cardData.artwork;

        gameObject.SetActive(true); // Activate the preview
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentCard == null) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Left-click on the preview: Play the card!");
            PlayCard();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Right-click on the preview: Remove the card!");
            ReturnToHand();
        }
    }

    private void PlayCard()
    {
        if (currentCard == null || CombatSystem.instance == null) return;

        if (currentCard.cardData.type == CardType.Defense && CombatSystem.instance.playerShield >= CombatSystem.instance.maxHealth)
        {
            Debug.Log("Max Shield Reached! Cannot Play!");
            return;
        }

        if (currentCard.cardData.type == CardType.Heal && CombatSystem.instance.playerHealth >= CombatSystem.instance.maxHealth)
        {
            Debug.Log("Max Health Reached! Cannot Play!");
            return;
        }

        if (CombatSystem.instance.SpendEnergy(currentCard.cardData.cost))
        {
            currentCard.PlayCard();
            CombatSystem.instance.UpdateUI();
        }
        ClosePreview();
    }

    private void ReturnToHand()
    {
        Debug.Log($"Returning card to hand: {currentCard.cardData.cardName}");
        currentCard.ResetCardPosition();
        ClosePreview();
    }

    private void ClosePreview()
    {
        gameObject.SetActive(false);
        currentCard = null;
    }
}
