using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StatusEffectScrollListItem : MonoBehaviour
{
    public StatusEffectScrollListManager listManager; // Reference to manager that talks to debug menu UI manager
    public StatusEffectData statusEffect; // Status effect data ref
    public GameObject selectedHighlight; // Ref to highlight obj

    bool isSelected = false;

    // On this effect selected
    public void SelectStatusEffect()
    {
        if (isSelected) // If already selected
        {
            selectedHighlight.SetActive(false);
            isSelected = false;

            listManager.somethingSelected = false;
            listManager.SetStatusEffectNull();
        }
        else // If not selected
        {
            if (listManager.somethingSelected) return; // Guard clause for if something else is already selected

            selectedHighlight.SetActive(true);
            isSelected = true;
            listManager.somethingSelected = true;
            listManager.ChangeSelectedStatusEffect(statusEffect);
        }
    }
}
