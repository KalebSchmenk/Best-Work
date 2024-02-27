using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighPriestessController : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject interactUIElement; // Gameobject to display that high priestess is interactable
    [SerializeField] private float distanceToBeInteractable = 10f; // Distance player needs to be within to interact

    bool hasBeenInteracted = false;
    bool subbedToInRangeEvent = false;

    GameObject player;

    PlayerStats playerStats;


    private void Start()
    {
        player = GameManager.instance.player;
        playerStats = player.GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (!subbedToInRangeEvent && playerStats.Cash >= 100 && Vector3.Distance(this.transform.position, player.transform.position) < distanceToBeInteractable) // If we arent subbed and are in interact distance
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

    private void OnDestroy()
    {
        PlayerEvents.onInteract -= Interact;
        interactUIElement.SetActive(false);
        subbedToInRangeEvent = false;
    }

    // Interact funciton
    public void Interact()
    {
        if (!subbedToInRangeEvent || hasBeenInteracted) return;

        playerStats.Cash -= 100;

        GameManager.instance.BeginTarotReading();
        GameManager.instance.currentHighPriestessAnimator = GetComponent<Animator>();
        interactUIElement.SetActive(false);

        hasBeenInteracted = true;

        GetComponent<Animator>().SetBool("Reading", true);

        Destroy(this);
    }
}
