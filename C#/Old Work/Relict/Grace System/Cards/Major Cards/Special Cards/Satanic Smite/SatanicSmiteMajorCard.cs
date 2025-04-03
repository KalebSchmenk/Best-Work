using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatanicSmiteMajorCard : MajorCardBase
{
    [Header("Satanic Smite Refs and Settings")]

    public GameObject fireCone; // Fire cone

    private PlayerWeaponController weaponController;


    // On ability key down
    public override void AbilityKeyDown()
    {
        if (GetCooldown()) return; // Guard clause. If we are cooling down - return

        print("Satanic Smite key down");
    }

    // On ability key up spawn fire cone
    public override void AbilityKeyUp()
    {
        if (GetCooldown()) return; // Guard clause. If we are cooling down - return. Or if ghost coroutine is empty

        print(this + " called its ability");

        SummonFireCone();

        PlayerEvents.OnAbilityUsed?.Invoke(this);

        StartCooldown();
    }

    // Fire cone ability
    private void SummonFireCone()
    {
        print(weaponController.projectileSpawnPosition.position);

        Transform projTrans = weaponController.projectileSpawnPosition;     // Bullet spawn point
        Quaternion lookRot = Quaternion.LookRotation(projTrans.forward);    // Look at bullet spawn point forward

        GameObject obj = Instantiate(fireCone, projTrans.position, lookRot);
        obj.transform.parent = player.transform;
        obj.transform.position += -projTrans.right * 0.25f;                 // Shift left
        obj.transform.rotation = Quaternion.LookRotation(player.transform.forward); // Flatten to look forward
    }

    // Grabs references when added to player inventory
    public override void OnAdd()
    {
        base.OnAdd();

        weaponController = player.GetComponent<PlayerWeaponController>();
    }

    // Prints to console that this card was removed
    public override void OnRemove()
    {
        base.OnRemove();
    }
}
