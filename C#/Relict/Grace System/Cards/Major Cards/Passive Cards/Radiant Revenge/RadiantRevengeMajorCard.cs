using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiantRevengeMajorCard : MajorCardBase
{
    [Header("Radiant Revenge Refs and Settings")]
    public float damageSphereRadius = 20f;
    [Range(1, 100)] public float percentOfDamageToDeal = 25f;
    public StatusEffectData sunburnData;
    public LayerMask everythingLayerMask;

    // Grabs references when added to player inventory
    public override void OnAdd()
    {
        base.OnAdd();

        player.GetComponent<PlayerHealth>().BroadcastDamageToBeTaken += SpawnDamageSphere;
    }

    // Prints to console that this card was removed
    public override void OnRemove()
    {
        base.OnRemove();

        player.GetComponent<PlayerHealth>().BroadcastDamageToBeTaken -= SpawnDamageSphere;
    }

    // Spawns damage sphere
    private void SpawnDamageSphere(ref float damage)
    {
        if (GetCooldown()) return; // If we are in cooldown, don't create damage sphere

        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, damageSphereRadius);

        foreach (Collider collider in hitColliders)
        {
            if (!collider.gameObject.CompareTag("Enemy")) continue; // Guard clause if not enemy


            if (collider.gameObject.TryGetComponent<ITakeDamage>(out ITakeDamage damageable))
            {
                // Deal damage to enemy by taking the damage the player took and multiplying it by variable
                damageable.TakeDamage(collider.gameObject.transform.position, sunburnData.damageNumberColor, damage * (percentOfDamageToDeal / 100), true);
            }

            if (collider.gameObject.TryGetComponent<IEffectable>(out IEffectable effectable))
            {
                effectable.AddStatusEffect(sunburnData); // Gives sunburn
            }
        }

        StartCooldown();
    }
}
