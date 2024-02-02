using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rot : StatusEffectBase
{
    public float critChanceIncrease = 25f; // How much to increase crit chance by on hit


    // Adds effect to player/enemy
    public override void AddEffect()
    {
        base.AddEffect();

        var takeDamage = parent.GetComponent<ITakeDamage>();

        if (takeDamage != null)
        {
            takeDamage.AboutToBeDamaged += AddValues;
        }
    }

    // Removes effect from player/enemy
    public override void RemoveEffect()
    {
        base.RemoveEffect();

        if (parent != null)
        {
            var takeDamage = parent.GetComponent<ITakeDamage>();

            if (takeDamage != null)
            {
                takeDamage.AboutToBeDamaged -= AddValues;
            }
        }

        effectable.RemoveStatusEffect(this);

        Destroy(this.gameObject);
    }


    // Adds value to critchance when damaged
    private void AddValues(ref float damage, ref float critChance, ref float critChanceDamageMultiplier)
    {
        critChance += critChanceIncrease;
    }
}
