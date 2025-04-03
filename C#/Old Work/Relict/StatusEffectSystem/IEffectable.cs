using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectable
{
    public List<StatusEffectBase> statusEffectBases { get; set; }

    public void AddStatusEffect(StatusEffectData statusEffectData);

    public void RemoveStatusEffect(StatusEffectBase statusEffect);
}
