using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DebugUIMajorCardAddController : MonoBehaviour, IPointerClickHandler
{
    public MajorCardSO card; // Card to add to inventory
    public Image cardImageSlot; // Slot for card image
    public GameObject backgroundObj; // Background obj for on select

    bool isSelected = false;
    InventoryManager inventoryManager; // Player inventory ref

    private void Start()
    {
        inventoryManager = GameManager.instance.player.GetComponentInChildren<InventoryManager>(); // Gets inventory from player held in Game Manager
        cardImageSlot.sprite = card.cardImage;  
    }

    // Event handling
    private void OnEnable()
    {
        DebugUIManager.ResetAllMajorCards += OnResetMajorCards;
    }
    private void OnDestroy()
    {
        DebugUIManager.ResetAllMajorCards -= OnResetMajorCards;
    }

    // Image select event
    public void OnPointerClick(PointerEventData eventData)
    {
        OnSelect();
    }

    // Adds or removes card based upon isSelected bool
    public void OnSelect()
    {
        if (!isSelected)
        {
            int success = 0;

            success = inventoryManager.AddCard(card);

            if (success != 0)
            {
                isSelected = true;
                backgroundObj.SetActive(true);
            }
        }
        else
        {
            isSelected = false;
            RemoveCard();
            backgroundObj.SetActive(false);
        }
    }

    // Adds the card referenced in this script to the inventory
    private void AddCard()
    {
        if(inventoryManager != null)
        {
            inventoryManager.AddCard(card);
        }
        else
        {
            Debug.LogWarning("Add Card Controller disconnected from inventory. Card was not added");
        }
    }

    // Removes card from inventory
    private void RemoveCard()
    {
        inventoryManager.DestroyCard(card);
    }

    // Handles on reset major cards event
    private void OnResetMajorCards()
    {
        isSelected = false;
        backgroundObj.SetActive(false);
    }

    // Checks if card is already in inventory but not reflected here
    public void CheckIfAlreadyOwned()
    {
        if (inventoryManager == null) inventoryManager = GameManager.instance.player.GetComponentInChildren<InventoryManager>(); // Gets inventory from player held in Game Manager


        var inventorySlot = inventoryManager.GetMajorCards(card.cardType);

        foreach (var majorCard in inventorySlot)
        {
            if (card == majorCard.cardSO)
            {
                isSelected = true;
                backgroundObj.SetActive(true);
                break;
            }
        }
    }
}
