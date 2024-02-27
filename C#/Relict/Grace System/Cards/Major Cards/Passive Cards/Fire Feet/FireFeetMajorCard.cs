using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerEvents;

public class FireFeetMajorCard : MajorCardBase
{
    [Header("Fire Feet Refs and Settings")]
    public float jumpIncrease = 5f; // Amount to increase jump force

    // On card add
    public override void OnAdd()
    {
        base.OnAdd();

        StatModifier statMod = new StatModifier(jumpIncrease, StatModifier.StatModType.Flat, 0, this);
        playerStats.JumpHeight.AddModifier(statMod);
    }

    // On card remove
    public override void OnRemove()
    {
        base.OnRemove();

        playerStats.JumpHeight.RemoveAllModifiersFromSource(this);
    }
}
