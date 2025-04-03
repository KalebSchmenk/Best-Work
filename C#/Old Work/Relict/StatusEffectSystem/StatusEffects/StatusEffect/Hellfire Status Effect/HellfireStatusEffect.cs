using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellfireStatusEffect : StatusEffectBase
{

    // Adds effect to player/enemy
    public override void AddEffect()
    {
        base.AddEffect();
    }

    // Removes effect from player/enemy
    public override void RemoveEffect()
    {
        base.RemoveEffect();

        effectable.RemoveStatusEffect(this);

        Destroy(this.gameObject);
    }
}
