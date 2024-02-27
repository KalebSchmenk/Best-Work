using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeededStatusEffect : StatusEffectBase
{
    [SerializeField] private float percentHealthBack = 0.5f;

    private PlayerHealth playerHealthController;

    // Adds effect to player/enemy
    public override void AddEffect()
    {
        base.AddEffect();

        playerHealthController = GameManager.instance.player.GetComponent<PlayerHealth>();
        this.OnEffectOutputDamage += ParentTakingDamage;
    }

    // Removes effect from player/enemy
    public override void RemoveEffect()
    {
        base.RemoveEffect();

        this.OnEffectOutputDamage -= ParentTakingDamage;

        effectable.RemoveStatusEffect(this);

        Destroy(this.gameObject);
    }

    // When this enemy takes damage from its DOT, heal the player
    private void ParentTakingDamage(float damage)
    {
        float healVal = damage * percentHealthBack;

        if (playerHealthController != null)
        {
            print("Player healed " +  healVal);
            playerHealthController.ChangePlayerHealth(healVal);
        }
        else
        {
            Debug.LogError(this + " could not find the player health script");
        }
    }
}
