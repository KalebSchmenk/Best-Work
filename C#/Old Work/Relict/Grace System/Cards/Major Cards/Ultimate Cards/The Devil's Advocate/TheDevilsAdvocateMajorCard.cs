using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TheDevilsAdvocateMajorCard : MajorCardBase
{
    [Header("The Devil's Advocate Refs and Settings")]
    public GameObject flamethrowerParticleEffect;
    [SerializeField] private float flamethrowerDuration = 5f;

    private PlayerWeaponController weaponController;
    private Transform bulletSpawnPosition; 


    // On ability key down 
    public override void AbilityKeyDown()
    {
        if (GetCooldown()) return; // Guard clause. If we are cooling down - return

        print("Devil's Advocate key down");
    }

    // On ability key up
    public override void AbilityKeyUp()
    {
        if (GetCooldown()) return; // Guard clause. If we are cooling down - return

        print(this + " called its ability");

        StartFlamethrower();

        PlayerEvents.OnAbilityUsed?.Invoke(this);
        StartCooldown();
    }

    private void StartFlamethrower()
    {
        weaponController.canShoot = false;

        Quaternion lookRot = Quaternion.LookRotation(bulletSpawnPosition.forward);    // Look at bullet spawn point forward

        var spawnedFlamethrower = Instantiate(flamethrowerParticleEffect, bulletSpawnPosition.position, lookRot);
        spawnedFlamethrower.transform.parent = player.transform;
        spawnedFlamethrower.transform.position += -bulletSpawnPosition.right * 0.25f;                 // Shift left
        spawnedFlamethrower.transform.rotation = Quaternion.LookRotation(player.transform.forward); // Flatten to look forward

        var flamethrowerController = spawnedFlamethrower.GetComponent<FlamethrowerController>();
        flamethrowerController.playerWeaponController = weaponController;
        flamethrowerController.duration = flamethrowerDuration;

        AudioManager.instance.PlaySfx("DevilsAdvocateFlamethrower"); // Plays devil's advocate flamethrower sound
    }

    public override void OnAdd()
    {
        base.OnAdd();

        weaponController = player.GetComponent<PlayerWeaponController>();
        bulletSpawnPosition = weaponController.projectileSpawnPosition;
    }

    public override void OnRemove()
    {
        base.OnRemove();
    }
}
