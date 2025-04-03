using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ITakeDamage;

public class SoftBodyController : MonoBehaviour, ITakeDamage, IEffectable
{
    public ThormBossAIController aiController;

    #region Events
    public AboutToTakeDamage AboutToBeDamaged { get => aiController.AboutToBeDamaged; set => aiController.AboutToBeDamaged = value; }
    public FinalDamage BroadcastDamageToBeTaken { get => aiController.BroadcastDamageToBeTaken; set => aiController.BroadcastDamageToBeTaken = value; }
    public List<StatusEffectBase> statusEffectBases { get => aiController.statusEffectBases; set => aiController.statusEffectBases = value; }
    #endregion

    #region Damage and effects
    public void AddStatusEffect(StatusEffectData statusEffectData)
    {
        aiController?.AddStatusEffect(statusEffectData);
    }

    public void RemoveStatusEffect(StatusEffectBase statusEffect)
    {
        aiController?.RemoveStatusEffect(statusEffect);
    }

    public void TakeDamage(Vector3 damageLocation, Color damageNumberColor, float damage, bool invokeTookDamageEvent)
    {
        float blankCritVal = -1;
        float blankDamageMul = -1; // Blank values so that we can still invoke event

        if (invokeTookDamageEvent) AboutToBeDamaged?.Invoke(ref damage, ref blankCritVal, ref blankDamageMul);

        print("Damage dealt to shield: " + damage);

        DamageNumberSpawner.SpawnDamageNumber(damageLocation, damage, damageNumberColor);
        aiController.health -= damage;
    }

    public void TakeDamage(Vector3 damageLocation, Color damageNumberColor, float damage, float baseCritChance, float critDamageMultiplier)
    {
        AboutToBeDamaged?.Invoke(ref damage, ref baseCritChance, ref critDamageMultiplier);

        var critValue = CritChanceController.instance.TryCritHit(damage, baseCritChance, critDamageMultiplier);

        if (critValue == -1)
        {
            BroadcastDamageToBeTaken?.Invoke(ref damage);
            DamageNumberSpawner.SpawnDamageNumber(damageLocation, damage, damageNumberColor);
            aiController.health -= damage; // Normal Damage
        }
        else
        {
            print("Crit damage dealt to shield: " + critValue);
            BroadcastDamageToBeTaken?.Invoke(ref critValue);
            DamageNumberSpawner.SpawnDamageNumber(damageLocation, critValue, damageNumberColor);
            aiController.health -= critValue; // Crit damage
        }
    }

    public void TakeDamage(Vector3 damageLocation, Color damageNumberColor, float damage, float minDamage)
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
