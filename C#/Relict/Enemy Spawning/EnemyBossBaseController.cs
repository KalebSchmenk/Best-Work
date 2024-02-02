using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBossBaseController : MonoBehaviour, ITakeDamage, IEffectable
{
    protected EnemyBossBaseState currentState; // Current boss state
    public enum BossStates // Boss states enum
    {
        Idle,
        Attack,
        Wander,
        Dead
    }
    public abstract ITakeDamage.AboutToTakeDamage AboutToBeDamaged { get; set; } // Interface event
    public abstract ITakeDamage.FinalDamage BroadcastDamageToBeTaken { get; set; } // Interface broadcast final damage event
    public abstract List<StatusEffectBase> statusEffectBases { get; set; } // Interface status effects list
    public abstract void AddStatusEffect(StatusEffectData statusEffectData); // Interface add a status effect
    public abstract void RemoveStatusEffect(StatusEffectBase statusEffect); // interface remove a status effect
    public abstract void TakeDamage(Vector3 damageLocation, Color damageNumberColor, float damage, bool invokeTookDamageEvent); // Damageable interface implementation
    public abstract void TakeDamage(Vector3 damageLocation, Color damageNumberColor, float damage, float baseCritChance, float critDamageMultiplier); // Damageable interface implementation
    public abstract void TakeDamage(Vector3 damageLocation, Color damageNumberColor, float damage, float minDamage);
}
