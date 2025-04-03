using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject projectile;
    public Transform projectileSpawnPos;

    private GenericGameObjectPool projectilePool;
    PlayerController playerController;

    private bool isAiming = false;
    private bool inFireCooldown = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponent<PlayerController>();

        playerController.inputHandler.OnAimInputRecieved += ToggleAim;
        playerController.inputHandler.OnFireInputRecieved += Shoot;

        projectilePool = this.AddComponent<GenericGameObjectPool>();
        projectilePool.Init(projectile, 80);
    }

    private void OnDestroy()
    {
        playerController.inputHandler.OnAimInputRecieved -= ToggleAim;
        playerController.inputHandler.OnFireInputRecieved -= Shoot;
    }

    /// <summary>
    /// Toggles player aiming
    /// </summary>
    public void ToggleAim()
    {
        playerController.IsAiming = !playerController.IsAiming;
        isAiming = playerController.IsAiming;
        playerController.animationController.SetAnimatorBool(PlayerAnimationController.ANIM_AIMING, playerController.IsAiming);
    }

    /// <summary>
    /// Fires a projectile out of the barrel of the gun
    /// </summary>
    private void Shoot()
    {
        if (inFireCooldown || !isAiming) return;

        var proj = projectilePool.Take();
        
        proj.transform.position = projectileSpawnPos.transform.position;
        
        var rot = projectileSpawnPos.transform.forward;
        rot.y = 0;
        proj.transform.rotation = Quaternion.LookRotation(rot);

        var bulletController = proj.GetComponent<BulletController>();
        bulletController.SetObjectPool(projectilePool);
        bulletController.Fire();

        StartCoroutine(WeaponCooldown());
    }

    /// <summary>
    /// Cooldown for weapon firing
    /// </summary>
    /// <returns></returns>
    private IEnumerator WeaponCooldown()
    {
        inFireCooldown = true;
        yield return new WaitForSeconds(0.2f);
        inFireCooldown = false;
    }
}
