using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    public delegate void UsedAbility(MajorCardBase majorCardBase);
    public static UsedAbility OnAbilityUsed;

    public delegate void PlayerEvent();
    public static PlayerEvent OnJump;

    public delegate void HealOnKill(int amountToAdd);
    public static HealOnKill OnEnemyKill;

    public delegate void BulletHitEnemy(GameObject enemy, ref float damage, ref float critChance, ref float critChanceDamageMultiplier);
    public static BulletHitEnemy OnBulletHitEnemy;

    public delegate void OnPlayerDeath();
    public static OnPlayerDeath onDeath;

    public delegate void OnPlayerWin();
    public static OnPlayerWin onWin;

    public delegate void OnPlayerInteract();
    public static OnPlayerInteract onInteract;

    public delegate void OnGunFired();
    public static OnGunFired gunFired;
    
    public delegate void OnPlayerCollison(GameObject collidedWith);
    public static OnPlayerCollison onCollision;
}
