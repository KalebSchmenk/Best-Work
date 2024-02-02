using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DazedDestroyerMajorCard : MajorCardBase
{
    [Header("Dazed Destroyer Refs and Settings")]
    [SerializeField] private float extraDamage = 5f; // Extra damage when enemy is dazed
    private List<ITakeDamage> dazedEnemies = new List<ITakeDamage>(); // List of dazed enemies


    // Deal extra damage to rotted enemies
    public void AddDamage(ref float damage, ref float baseCritChanceValue, ref float damageMultiplier)
    {
        print("Dazed Destroyer adding this amount to damage: " + extraDamage);
        damage += extraDamage;
    }

    // Add rotted enemy to our list and subscribe to their about to be damaged event
    public void AddDazedEnemy(ITakeDamage damageable)
    {
        dazedEnemies.Add(damageable);
        damageable.AboutToBeDamaged += AddDamage;
    }

    // Remove rotted enemy to our list and subscribe to their about to be damaged event
    public void RemoveRottedEnemy(ITakeDamage damageable)
    {
        dazedEnemies.Remove(damageable);
        damageable.AboutToBeDamaged -= AddDamage;
    }

    // If a status effect is added to a player and is of Dazed type add the enemy here
    private void ListenForStatusEffect(IEffectable effectable, StatusEffectBase statusEffect, ITakeDamage damageable)
    {
        if (statusEffect.GetType() == typeof(DazedStatusEffect))
        {
            AddDazedEnemy(damageable);
        }
    }

    // If a status effect is removed from player and is of Rot type add the enemy here
    private void ListenForStatusEffectRemoval(IEffectable effectable, StatusEffectBase statusEffect, ITakeDamage damageable)
    {
        if (statusEffect.GetType() == typeof(DazedStatusEffect))
        {
            RemoveRottedEnemy(damageable);
        }
    }

    // Grabs references when added to player inventory
    public override void OnAdd()
    {
        base.OnAdd();

        EnemyEvents.OnEnemyAddStatusEffect += ListenForStatusEffect;
        EnemyEvents.OnEnemyRemovedStatusEffect += ListenForStatusEffectRemoval;
    }

    // Prints to console that this card was removed
    public override void OnRemove()
    {
        base.OnRemove();

        EnemyEvents.OnEnemyAddStatusEffect -= ListenForStatusEffect;
        EnemyEvents.OnEnemyRemovedStatusEffect -= ListenForStatusEffectRemoval;

        foreach (var interactable in dazedEnemies)
        {
            if (interactable != null)
            {
                interactable.AboutToBeDamaged -= AddDamage;
            }
        }
    }
}
