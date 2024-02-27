using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthsShotMajorCard : MajorCardBase
{
    [Header("Strength's Shot Refs and Settings")]
    [SerializeField][Range(0.0f, 1.0f)] private float damageIncreaseMultiplier = 25f;

    //[SerializeField] private float stunDuration = 5f;

    [SerializeField] private float knockBackChance = 100f;
    [SerializeField] private float knockBackForce = 2.5f;

    public StatusEffectData dazedStatusEffectData; // Dazed status effect data

    public AudioClip strengthsShotSound;


    public override void OnAdd()
    {
        base.OnAdd();

        PlayerEvents.OnBulletHitEnemy += BulletHitEnemy;
        player.GetComponent<PlayerWeaponController>().SetSound(strengthsShotSound);
    }

    public override void OnRemove()
    {
        base.OnRemove();

        PlayerEvents.OnBulletHitEnemy -= BulletHitEnemy;
        player.GetComponent<PlayerWeaponController>().ResetSound();
    }

    // Bullet obj is reference to the instantiated bullet
    // Enemy obj is reference to the enemy that was hit
    // Damage is a reference to the damage output about to be applied to the enemy
    private void BulletHitEnemy(GameObject enemy, ref float damage, ref float critChance, ref float critChanceDamageMultiplier)
    {
       // print("Strength shot changing values");

        damage = damage * (1f + (damageIncreaseMultiplier / 100f));

        int randomInt = UnityEngine.Random.Range(1, 101); // Roll for knockback

        AudioManager.instance.PlaySfx("StrengthShot"); // Plays strength shot impact sound

        if (randomInt <= knockBackChance)
        {
            // Knockback enemy
            IStunnable stunnable = null;
            enemy.TryGetComponent<IStunnable>(out stunnable);

            if (stunnable != null)
            {
                var effectable = enemy.GetComponent<IEffectable>();

                if (effectable == null)
                {
                    //print("Empty effectable. Returning");
                    return;
                }

                foreach (var effect in effectable.statusEffectBases)
                {
                    if (effect.GetType() == typeof(DazedStatusEffect))
                    {
                        //print("Enemy already dazed! Not dazing again");
                        return;
                    }

                }

                if (effectable != null)
                {
                    effectable.AddStatusEffect(dazedStatusEffectData);
                }
            }

        //Debug.Log("KnockBackEnemy");
        StartCoroutine(KnockBackEnemySoon(enemy));
        }
    }

    private IEnumerator KnockBackEnemySoon(GameObject enemy)
    {
        if (enemy == null) yield break;

        var knockbackDir = enemy.transform.position - player.transform.position;
        knockbackDir.y = 0;

        yield return new WaitForSeconds(0.05f);

        if (enemy == null) yield break;

        float forceMultiplier = 15f;

        if (enemy.TryGetComponent<AIMain>(out AIMain aiController))
        {
            aiController.Launch(knockbackDir.normalized, knockBackForce * forceMultiplier, dazedStatusEffectData.Lifetime);
        }
    }
}
