using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotShotMajorCard : MajorCardBase
{
    [Header("Rot Shot Refs and Settings")]
    public float damageIncrease = 5f; // Amount to increase damage by
    public float chanceToAddRot = 100f; // Chance to inflict rot
    public StatusEffectData rotStatusEffectData; // Rot status effect data
    public AudioClip rotShotSound;

    // On card add
    public override void OnAdd()
    {
        base.OnAdd();

        PlayerEvents.OnBulletHitEnemy += BulletHitEnemy;
        player.GetComponent<PlayerWeaponController>().SetSound(rotShotSound);
    }

    // On card remove
    public override void OnRemove()
    {
        base.OnRemove();

        PlayerEvents.OnBulletHitEnemy -= BulletHitEnemy;
        player.GetComponent<PlayerWeaponController>().ResetSound();
    }

    // When a bullet hits an enemy
    private void BulletHitEnemy(GameObject enemy, ref float damage, ref float critChance, ref float critChanceDamageMultiplier)
    {
        var effectable = enemy.GetComponent<IEffectable>();

        damage += damageIncrease;

        print("Rot Shot changing damage to: " + damage);

        if (effectable == null)
        {
            print("This enemy does not have the effectable interface, exiting");
            return;
        }

        foreach (var effect in effectable.statusEffectBases)
        {
            if (effect.GetType() == typeof(Rot))
            {
                print("Enemy already has rot! Not adding another.");
                return;
            }
        }

        int randomInt = UnityEngine.Random.Range(0, 101);

        if (randomInt < chanceToAddRot)
        {
            if (effectable != null)
            {
                effectable.AddStatusEffect(rotStatusEffectData);
            }
        }
    }
}
