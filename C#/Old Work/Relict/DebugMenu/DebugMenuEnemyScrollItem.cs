using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DebugMenuEnemyScrollItem : MonoBehaviour
{
    [NonSerialized] public IEffectable effectable; // Effectable ref

    public GameObject selectedHighlight;
    public Button selectEnemyButton;
    public TextMeshProUGUI effectableName;

    bool isSelected = false;

    // On item selected
    public void OnSelect()
    {
        if (isSelected) // If item selected
        {
            selectedHighlight.SetActive(false);
            isSelected = false;

            DebugUIManager.AddStatusEffect -= AddStatusEffect;
            DebugUIManager.RemoveStatusEffect -= RemoveStatusEffect;
        }
        else // If not selected
        {
            selectedHighlight.SetActive(true);
            isSelected = true;

            DebugUIManager.AddStatusEffect += AddStatusEffect;
            DebugUIManager.RemoveStatusEffect += RemoveStatusEffect;
        }
    }

    // Adds passed in status effect to effectable
    private void AddStatusEffect(StatusEffectData statusEffect)
    {
        foreach (var effect in effectable.statusEffectBases)
        {
            if (effect.GetType() == statusEffect.StatusEffectPrefab.GetComponent<StatusEffectBase>().GetType())
            {
                print("Effectable already has this status effect! Not adding another.");
                return;
            }
        }

        effectable.AddStatusEffect(statusEffect);
    }

    // removes passed in status effect from effectable
    private void RemoveStatusEffect(StatusEffectData statusEffect)
    {
        foreach (var effect in effectable.statusEffectBases)
        {
            if (effect.GetType() == statusEffect.StatusEffectPrefab.GetComponent<StatusEffectBase>().GetType())
            {
                effectable.RemoveStatusEffect(effect);
                return;
            }
        }
    }

    // Event unsubs
    private void OnDestroy()
    {
        DebugUIManager.AddStatusEffect -= AddStatusEffect;
        DebugUIManager.RemoveStatusEffect -= RemoveStatusEffect;
    }
}
