using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITakeDamage
{
    public delegate void AboutToTakeDamage(ref float damage, ref float critChance, ref float critChanceDamageMultiplier);
    public AboutToTakeDamage AboutToBeDamaged
    {
        get; set;
    }

    public delegate void FinalDamage(ref float finalDamage);
    public FinalDamage BroadcastDamageToBeTaken
    {
        get; set;
    }

    // Function for taking damage. Parameter is what pos damage happened at, how much damage, and whether to invoke the event
    public void TakeDamage(Vector3 damageLocation, Color damageNumberColor, float damage, bool invokeEvent);

    // Function for taking damage. Parameter is how much damage to deal, and a clamp value to ensure its within a value range.
    public void TakeDamage(Vector3 damageLocation, Color damageNumberColor, float damage, float minClamp = 1f);

    // Function for taking damage with a base crit chance value and crit chance damage multiplier
    public void TakeDamage(Vector3 damageLocation, Color damageNumberColor, float damage, float baseCritChance, float critDamageMultiplier); 

}
