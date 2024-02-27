using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ITakeDamage;

public class ShellController : MonoBehaviour, ITakeDamage
{
    public AboutToTakeDamage AboutToBeDamagedEvent;
    public AboutToTakeDamage AboutToBeDamaged { get => AboutToBeDamagedEvent; set => AboutToBeDamagedEvent = value; }

    public FinalDamage DamageToBeTaken;
    public FinalDamage BroadcastDamageToBeTaken { get => DamageToBeTaken; set => DamageToBeTaken = value; }

    // Damages
    public void TakeDamage(Vector3 damageLocation, Color damageNumberColor, float damage, bool invokeEvent)
    {
        DamageNumberSpawner.SpawnDamageNumber(damageLocation, 0f, damageNumberColor);
    }

    public void TakeDamage(Vector3 damageLocation, Color damageNumberColor, float damage, float minClamp)
    {
        throw new System.NotImplementedException();
    }

    // Damages 
    public void TakeDamage(Vector3 damageLocation, Color damageNumberColor, float damage, float baseCritChanceValue, float damageMultiplier)
    {
        DamageNumberSpawner.SpawnDamageNumber(damageLocation, 0f, damageNumberColor);
    }
}
