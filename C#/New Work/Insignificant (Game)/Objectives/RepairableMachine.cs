using System.Collections;
using UnityEngine;

public class RepairableMachine : MonoBehaviour, IInteractable
{
    bool repaired = false;

    /// <summary>
    /// Repair this machine when interacted with.
    /// </summary>
    /// <returns>Interaction success.</returns>
    public bool Interact()
    {
        if (repaired) return false;
        repaired = true;

        StartCoroutine(OnInteract());
        return true;
    }

    private IEnumerator OnInteract()
    {
        yield return new WaitForSeconds(6f);
        GameManager.OnTaskComplete?.Invoke();
    }
}
