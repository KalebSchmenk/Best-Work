using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectScrollListManager : MonoBehaviour
{
    public DebugUIManager debugUIManager; // Ref to debug UI manager

    public bool somethingSelected = false; // Is something currently selected

    // Changes selected status effect to passed in status effect
    public void ChangeSelectedStatusEffect(StatusEffectData passedStatusEffect)
    {
        debugUIManager.currentSelectedStatusEffect = passedStatusEffect;
    }

    // Sets selected status effect to null
    public void SetStatusEffectNull()
    {
        debugUIManager.currentSelectedStatusEffect = null;
    }
}
