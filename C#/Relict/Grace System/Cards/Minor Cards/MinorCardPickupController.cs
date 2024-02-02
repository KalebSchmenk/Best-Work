using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinorCardPickupController : MonoBehaviour, IInteractable
{
    public enum MinorCardType
    {
        Swords,
        Pentacles,
        Wands,
        Cups
    }
    public MinorCardType minorCardType; // Minor card type to upgrade

    [SerializeField] private GameObject interactUIElement; // Gameobject to display that high priestess is interactable
    [SerializeField] private float distanceToBeInteractable = 10f; // Distance player needs to be within to interact

    bool subbedToInRangeEvent = false;

    GameObject player;


    private void Start()
    {
        player = GameManager.instance.player;

        var cardImg = GetComponent<SpriteRenderer>();
        switch (minorCardType)
        {
            case MinorCardType.Swords:
                if (cardImg != null) cardImg.sprite = DeckManager.instance.swordsCard;
                break;

            case MinorCardType.Pentacles:
                if (cardImg != null) cardImg.sprite = DeckManager.instance.pentaclesCard;
                break;

            case MinorCardType.Wands:
                if (cardImg != null) cardImg.sprite = DeckManager.instance.wandsCard;
                break;

            case MinorCardType.Cups:
                if (cardImg != null) cardImg.sprite = DeckManager.instance.cupsCard;
                break;

            default:
                break;
        }
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
        if (player == null) return; // Guard clause

        var inventory = player.GetComponentInChildren<InventoryManager>();

        switch (minorCardType)
        {
            case MinorCardType.Swords:
                var swordCardScript = inventory.GetComponentInChildren<SwordMinorCard>();
                swordCardScript.Upgrade();
                break;

            case MinorCardType.Pentacles:
                var pentCardScript = inventory.GetComponentInChildren<PentaclesMinorCard>();
                pentCardScript.Upgrade();
                break;

            case MinorCardType.Wands:
                var wandCardScript = inventory.GetComponentInChildren<WandsMinorCard>();
                wandCardScript.Upgrade();
                break;

            case MinorCardType.Cups:
                var cupCardScript = inventory.GetComponentInChildren<CupsMinorCard>();
                cupCardScript.Upgrade();
                break;

            default:
                break;
        }

        PlayerEvents.onInteract -= Interact;
        Destroy(this.gameObject);

        AudioManager.instance.PlaySfx("MinorCardPickedUp"); // Plays pick up sound
    }
}
