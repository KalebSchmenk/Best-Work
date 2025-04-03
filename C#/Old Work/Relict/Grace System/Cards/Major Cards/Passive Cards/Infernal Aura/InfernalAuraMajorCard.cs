using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfernalAuraMajorCard : MajorCardBase
{
    [Header("Infernal Aura Refs and Settings")]
    public float damageSphereRadius = 20f;
    public float damageOutput = 5f;
    public StatusEffectData hellfireData;
    public LayerMask everythingLayerMask;

    // Grabs references when added to player inventory
    public override void OnAdd()
    {
        base.OnAdd();

        player.GetComponent<PlayerHealth>().BroadcastDamageToBeTaken += InvokeDamageSphere;
    }

    // Prints to console that this card was removed
    public override void OnRemove()
    {
        base.OnRemove();

        player.GetComponent<PlayerHealth>().BroadcastDamageToBeTaken -= InvokeDamageSphere;
    }


    // Creates damage sphere and tries to add status effect to enemies
    private void InvokeDamageSphere(ref float damage)
    {
        if (GetCooldown()) return; // If we are in cooldown, don't create damage sphere

        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, damageSphereRadius, LayerMask.GetMask("Enemy"));

        foreach (Collider collider in hitColliders)
        {
            if (!collider.gameObject.CompareTag("Enemy")) continue; // Guard clause if not enemy

            // Deal Damage
            if (collider.gameObject.TryGetComponent<ITakeDamage>(out ITakeDamage damageable))
            {
                damageable.TakeDamage(collider.gameObject.transform.position, hellfireData.damageNumberColor, damageOutput, true);
            }

            // Try for status effect
            if (collider.gameObject.TryGetComponent<IEffectable>(out IEffectable effectable))
            {
                effectable.AddStatusEffect(hellfireData);
            }
        }

        StartCooldown();
    }
}
