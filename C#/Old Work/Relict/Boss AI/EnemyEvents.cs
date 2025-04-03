using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvents : MonoBehaviour
{
    public delegate void EnemyAddedStatusEffect(IEffectable effectable, StatusEffectBase statusEffect, ITakeDamage damageable); // When an enemy adds a status effect
    public static EnemyAddedStatusEffect OnEnemyAddStatusEffect;

    public delegate void EnemyRemovedStatusEffect(IEffectable effectable, StatusEffectBase statusEffect, ITakeDamage damageable); // When an enemy removes a status effect
    public static EnemyRemovedStatusEffect OnEnemyRemovedStatusEffect;

    public delegate void EnemyDied(GameObject thisEnemy);
    public static EnemyDied OnEnemyDied;

    public delegate void EnemyKnockbacked(GameObject thisEnemy);
    public static EnemyKnockbacked OnEnemyKnockbacked;
}
