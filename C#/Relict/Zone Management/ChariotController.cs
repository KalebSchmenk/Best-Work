using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChariotController : MonoBehaviour, IInteractable
{
    public string objectiveUpdateText = "Objective(s) complete! Find the chariot and exfil!";

    private void OnEnable()
    {
        PlayerEvents.onInteract += Interact;
        GameManager.instance.UpdateObjective(objectiveUpdateText);
        transform.Find("InteractableParent").gameObject.SetActive(true);

        //transform.Find("Chariot_Animated").GetComponent<Animator>().enabled = true;
        //Invoke(nameof(EnableInteraction), 7.2f);
    }

    private void OnDisable()
    {
        PlayerEvents.onInteract -= Interact;
    }


    private void EnableInteraction()
    {
        PlayerEvents.onInteract += Interact;
        GameManager.instance.UpdateObjective(objectiveUpdateText);
        transform.Find("InteractableParent").gameObject.SetActive(true);
    }

    public void Interact()
    {
        if (Vector3.Distance(GameManager.instance.player.transform.position, this.transform.position) < 25f)
        {
            LevelManager.instance.NextLevel();
        }
    }
}
