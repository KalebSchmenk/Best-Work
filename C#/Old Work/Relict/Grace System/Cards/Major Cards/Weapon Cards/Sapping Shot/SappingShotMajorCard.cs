using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SappingShotMajorCard : MajorCardBase
{
    [Header("Sapping Shot Refs and Settings")]
    public float damageIncrease = 5f; // Amount to increase damage by
    public float chanceToAddSeeded = 25f; // Chance to inflict seeded
    public StatusEffectData seededStatusEffect; // Seeded status effect data
    public AudioClip sappingShotSound;

    // On card add
    public override void OnAdd()
    {
        base.OnAdd();

        PlayerEvents.OnBulletHitEnemy += BulletHitEnemy;
        player.GetComponent<PlayerWeaponController>().SetSound(sappingShotSound);
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

        print("Sapping shot increasing damage to: " + damage);

        if (effectable == null)
        {
            print("This enemy does not have the effectable interface, exiting");
            return;
        }

        int randomInt = UnityEngine.Random.Range(0, 101);

        if (randomInt < chanceToAddSeeded)
        {
            if (effectable != null)
            {
                effectable.AddStatusEffect(seededStatusEffect);
            }
        }
    }
}
