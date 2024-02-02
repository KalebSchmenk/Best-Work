using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MajorCardPickupController : MonoBehaviour, IInteractable
{
    [SerializeField] private MajorCardSO cardSO; // Scriptable Object
    [SerializeField] private GameObject interactUIElement; // Gameobject to display that high priestess is interactable
    [SerializeField] private float distanceToBeInteractable = 10f; // Distance player needs to be within to interact

    bool subbedToInRangeEvent = false;

    GameObject player;

    private void Start()
    {
        player = GameManager.instance.player;

        var cardImg = GetComponent<SpriteRenderer>();
        if (cardImg != null) cardImg.sprite = cardSO.cardImage;
    }

    private void Update()
    {
        if (!subbedToInRangeEvent && Vector3.Distance(this.transform.position, player.transform.position) < distanceToBeInteractable) // If we arent subbed and are in interact distance
        {
            PlayerEvents.onInteract += Interact;
            interactUIElement.SetActive(true);
            subbedToInRangeEvent = true;
        }
        else if (subbedToInRangeEvent && Vector3.Distance(this.transform.position, player.transform.position) > distanceToBeInteractable) // If we are subbed but no longer in interact distance
        {
            PlayerEvents.onInteract -= Interact;
            interactUIElement.SetActive(false);
            subbedToInRangeEvent = false;
        }
    }

    public void Interact()
    {
        if (player == null || cardSO == null) return; // Guard clause

        player.GetComponentInChildren<InventoryManager>().AddCard(cardSO); // Adds card to inventory

        PlayerEvents.onInteract -= Interact;
        Destroy(this.gameObject);
    }
}
