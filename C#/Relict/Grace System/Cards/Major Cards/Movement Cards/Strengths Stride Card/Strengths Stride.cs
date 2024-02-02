using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthsStride : MajorCardBase
{
    [Header("Card References")]
    [Header("Movement Settings")]
    [SerializeField] private float movementMultiplier;
    [SerializeField] private StatModifier.StatModType movementModType = StatModifier.StatModType.PercentMult; // Mod type

    [Header("Card Duration")]
    [SerializeField] private float duration = 2f;

    private Coroutine removeCoroutine = null;

    public override void OnAdd()
    {
        base.OnAdd();
    }

    public override void OnRemove()
    {
        base.OnRemove();

        RemoveStatMultipliers();
    }

    public override void AbilityKeyUp()
    {

        if (GetCooldown() || removeCoroutine != null) return; // Guard clause. If we are cooling down - return. Or if coroutine is empty
        AddStatMultipliers();

        PlayerEvents.OnAbilityUsed?.Invoke(this);

        StartCooldown(); // Ability cooldown

        removeCoroutine = StartCoroutine(RemoveMultipliersIn()); // Cooldown for how long stat bonuses last
    }

    public override void AbilityKeyDown() 
    { 

    base.AbilityKeyDown();
    if (GetCooldown()) return; // Guard clause. If we are cooling down - return

    }

    void AddStatMultipliers()
    {
        playerStats.MovementSpeed.AddModifier(new StatModifier(movementMultiplier / 100f, movementModType, this));
    }

    void RemoveStatMultipliers()
    {
        playerStats.MovementSpeed.RemoveAllModifiersFromSource(this);
    }

    IEnumerator RemoveMultipliersIn()
    {
        yield return new WaitForSeconds(duration);
        removeCoroutine = null;
        RemoveStatMultipliers();
    }
}
