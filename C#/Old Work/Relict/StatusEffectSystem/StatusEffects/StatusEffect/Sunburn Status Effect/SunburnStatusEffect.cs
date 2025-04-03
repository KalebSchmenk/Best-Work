using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunburnStatusEffect : StatusEffectBase
{
    public float energyGivenBackOnHit = 2f; // Energy to give back to player

    private WeaponStats weaponStats; // Weapon stats ref

    // Adds effect to player/enemy
    public override void AddEffect()
    {
        base.AddEffect();

        PlayerEvents.OnBulletHitEnemy += BulletHitEnemy;

        weaponStats = GameManager.instance.player.GetComponent<WeaponStats>();
    }

    // Removes effect from player/enemy
    public override void RemoveEffect()
    {
        base.RemoveEffect();

        effectable.RemoveStatusEffect(this);

        PlayerEvents.OnBulletHitEnemy -= BulletHitEnemy;

        Destroy(this.gameObject);
    }

    // When a bullet hits an enemy
    private void BulletHitEnemy(GameObject enemy, ref float damage, ref float critChance, ref float critChanceDamageMultiplier)
    {
        if (GameObject.ReferenceEquals(enemy, this.transform.parent.gameObject)) // If we are the enemy that was hit
        {
            weaponStats.CurrentCharge.currentValue += energyGivenBackOnHit; // Give some energy back
        }
    }
}
