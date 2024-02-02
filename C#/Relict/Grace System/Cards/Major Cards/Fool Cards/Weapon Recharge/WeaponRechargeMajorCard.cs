using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRechargeMajorCard : MajorCardBase
{
    public float percentOfMaxValueBack = 30f; // This value divided by 100 is what percentage of the current max charge will return to gun

    // On ability key down 
    public override void AbilityKeyDown()
    {
        if (GetCooldown()) return; // Guard clause. If we are cooling down - return

        print("Weapon Recharge key down");
    }

    // On ability key up 
    public override void AbilityKeyUp()
    {
        if (GetCooldown()) return; // Guard clause. If we are cooling down return

        print(this + " called its ability");

        weaponStats.CurrentCharge.currentValue += weaponStats.MaxCharge.Value * (percentOfMaxValueBack / 100f);
        StartCooldown();

        PlayerEvents.OnAbilityUsed?.Invoke(this);
    }

    public override void OnAdd()
    {
        base.OnAdd();
    }

    public override void OnRemove()
    {
        base.OnRemove();
    }
}
