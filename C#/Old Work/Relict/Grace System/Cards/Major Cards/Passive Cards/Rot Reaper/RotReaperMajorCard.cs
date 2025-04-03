using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotReaperMajorCard : MajorCardBase
{
    [Header("Rot Reaper Refs and Settings")]
    [SerializeField] private float extraDamage = 5f; // Extra damage when enemy is rotted
    private List<ITakeDamage> rottedEnemies = new List<ITakeDamage>(); // List of rotted enemies


    // Deal extra damage to rotted enemies
    public void AddDamage(ref float damage, ref float baseCritChanceValue, ref float damageMultiplier)
    {
        print("Rot Reaper adding this amount to damage: " + extraDamage);
        damage += extraDamage;
    }
    
    // Add rotted enemy to our list and subscribe to their about to be damaged event
    public void AddRottedEnemy(ITakeDamage damageable)
    {
        rottedEnemies.Add(damageable);
        damageable.AboutToBeDamaged += AddDamage;
    }

    // Remove rotted enemy to our list and subscribe to their about to be damaged event
    public void RemoveRottedEnemy(ITakeDamage damageable)
    {
        rottedEnemies.Remove(damageable);
        damageable.AboutToBeDamaged -= AddDamage;
    }

    // If a status effect is added to player and is of Rot type add the enemy here
    private void ListenForStatusEffect(IEffectable effectable, StatusEffectBase statusEffect, ITakeDamage damageable)
    {
        if (statusEffect.GetType() == typeof(Rot))
        {
            AddRottedEnemy(damageable);
        }
    }

    // If a status effect is removed from player and is of Rot type add the enemy here
    private void ListenForStatusEffectRemoval(IEffectable effectable, StatusEffectBase statusEffect, ITakeDamage damageable)
    {
        if (statusEffect.GetType() == typeof(Rot))
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

        foreach (var interactable in rottedEnemies)
        {
            if (interactable != null)
            {
                interactable.AboutToBeDamaged -= AddDamage;
            }
        }
    }
}
