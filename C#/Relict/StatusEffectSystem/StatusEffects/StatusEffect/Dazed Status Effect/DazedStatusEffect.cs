using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DazedStatusEffect : StatusEffectBase
{
    [SerializeField] private float damageMultiplier = 2.5f; // How much to increase damage by
    [SerializeField] private float dazedLength = 5f; // How long this status effect stuns enemy


    // Adds effect to player/enemy
    public override void AddEffect()
    {
        base.AddEffect();

        var takeDamage = parent.GetComponent<ITakeDamage>();

        if (takeDamage != null)
        {
            takeDamage.AboutToBeDamaged += AddValues;
        }

        var stunnable = parent.GetComponent<IStunnable>();
        
        if (stunnable != null)
        {
            stunnable.Stun(dazedLength);
        }

        data.Lifetime = dazedLength; // Overrides this instance....right?
    }

    // Removes effect from player/enemy
    public override void RemoveEffect()
    {
        base.RemoveEffect();

        if (parent != null )
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

    // Adds value to damange when damaged
    private void AddValues(ref float damage, ref float critChance, ref float critChanceDamageMultiplier)
    {
        damage *= damageMultiplier;
    }
}
