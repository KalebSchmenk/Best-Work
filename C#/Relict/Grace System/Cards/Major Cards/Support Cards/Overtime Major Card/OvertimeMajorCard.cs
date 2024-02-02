using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvertimeMajorCard : MajorCardBase
{
    [Header("Overtime Refs and Settings")]

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeedMultiplier = 25f; // Movement speed multiplier
    [SerializeField] private StatModifier.StatModType movementModType = StatModifier.StatModType.PercentMult; // Mod type
    [Header("Rifle Energy Recharge Settings")]
    [SerializeField] private float rifleEnergyRechargeMultiplier = 25f; // Recharge rate multiplier
    [SerializeField] private StatModifier.StatModType rifleEnergyRechargModType = StatModifier.StatModType.PercentMult; // Mod type
    [Header("Fire Rate Settings")]
    [SerializeField] private float fireRateMultiplier = 25f; // Fire rate multiplier
    [SerializeField] private StatModifier.StatModType fireRateModType = StatModifier.StatModType.PercentMult; // Mod type

    [Header("Ability Duration")]
    [SerializeField] private float duration = 2.5f;

    private Coroutine removeCoroutine = null;

    // On ability key down 
    public override void AbilityKeyDown()
    {
        if (GetCooldown()) return; // Guard clause. If we are cooling down - return

        print("Overtime key down");
    }

    // On ability key up 
    public override void AbilityKeyUp()
    {
        if (GetCooldown() || removeCoroutine != null) return; // Guard clause. If we are cooling down - return. Or if coroutine is empty

        print(this + " called its ability");

        AddStatMultipliers();

        PlayerEvents.OnAbilityUsed?.Invoke(this);

        StartCooldown(); // Ability cooldown
        removeCoroutine = StartCoroutine(RemoveMultipliersIn()); // Cooldown for how long stat bonuses last
    }

    public override void OnAdd()
    {
        base.OnAdd();
    }

    public override void OnRemove()
    {
        base.OnRemove();

        RemoveStatMultipliers();
    }

    // Adds stat multipliers to player and weapon stats
    void AddStatMultipliers()
    {
        playerStats.MovementSpeed.AddModifier(new StatModifier(movementSpeedMultiplier / 100f, movementModType, this));
        weaponStats.RechargeRate.AddModifier(new StatModifier(rifleEnergyRechargeMultiplier / 100f, rifleEnergyRechargModType, this));
        weaponStats.RateOfFire.AddModifier(new StatModifier(fireRateMultiplier / 100f, fireRateModType, this));
    }

    // Removes stat multipliers from player and weapon stats
    void RemoveStatMultipliers()
    {
        playerStats.MovementSpeed.RemoveAllModifiersFromSource(this);
        weaponStats.RechargeRate.RemoveAllModifiersFromSource(this);
        weaponStats.RateOfFire.RemoveAllModifiersFromSource(this);
    }

    // Removes stat bonuses after input time. This should happen before the ability cooldown wears off
    IEnumerator RemoveMultipliersIn()
    {
        yield return new WaitForSeconds(duration);
        removeCoroutine = null;
        RemoveStatMultipliers();
    }
}
