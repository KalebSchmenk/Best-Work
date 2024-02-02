using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChariotController : MonoBehaviour, IInteractable
{
    private void OnEnable()
    {
        PlayerEvents.onInteract += Interact;
    }
    private void OnDisable()
    {
        PlayerEvents.onInteract -= Interact;
    }

    public void Interact()
    {
        if (Vector3.Distance(GameManager.instance.player.transform.position, this.transform.position) < 25f)
        {
            LevelManager.instance.NextLevel();
        }
    }
}
