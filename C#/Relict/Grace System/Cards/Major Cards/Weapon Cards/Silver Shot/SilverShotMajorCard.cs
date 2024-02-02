using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverShotMajorCard : MajorCardBase
{
    [Header("Strength's Shot Refs and Settings")]
    [SerializeField][Range(0.0f, 1.0f)] private float damageIncreaseMultiplier = 25f;

    public Transform silverShotProjectilePrefab;

    public override void OnAdd()
    {
        base.OnAdd();
        GameManager.instance.player.GetComponent<PlayerWeaponController>().SetProjectile(silverShotProjectilePrefab);
        PlayerEvents.OnBulletHitEnemy += BulletHitEnemy;
    }

    public override void OnRemove()
    {
        base.OnRemove();
        GameManager.instance.player.GetComponent<PlayerWeaponController>().ResetProjectile();
        PlayerEvents.OnBulletHitEnemy -= BulletHitEnemy;
    }

    private void BulletHitEnemy(GameObject enemy, ref float damage, ref float critChance, ref float critChanceDamageMultiplier)
    {
        damage = damage * (1f + (damageIncreaseMultiplier / 100f));

        //AudioManager.instance.PlaySfx("StrengthShot"); // Plays strength shot impact sound

    }
}
