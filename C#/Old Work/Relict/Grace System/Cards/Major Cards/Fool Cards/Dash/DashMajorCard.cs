using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMajorCard : MajorCardBase
{
    PlayerController controller;

    // On ability key down 
    public override void AbilityKeyDown()
    {
        if (GetCooldown()) return; // Guard clause. If we are cooling down - return

        print("Dash key down");
    }

    // On ability key up 
    public override void AbilityKeyUp()
    {
        if (GetCooldown()) return; // Guard clause. If we are cooling down - return.

        print(this + " called its ability");

        Dash();

        PlayerEvents.OnAbilityUsed?.Invoke(this);

        StartCooldown(); // Ability cooldown
    }

    public override void OnAdd()
    {
        base.OnAdd();

        controller = player.GetComponent<PlayerController>();
    }

    public override void OnRemove()
    {
        base.OnRemove();
    }

    private void Dash()
    {
        StartCoroutine(DefaultDash());

        // Plays dash sound and disables footstep sounds momentarily
        controller.PlaySound(controller.playerData.Player_Dash);
        controller.footstepsSound.enabled = false;
        controller.sprintSound.enabled = false;
    }

    private IEnumerator DefaultDash()
    {
        controller.playerVelocity = new Vector3(player.transform.forward.x * playerStats.DashPower.Value, 0f, player.transform.forward.z * playerStats.DashPower.Value);
        yield return new WaitForSeconds(playerStats.DashTime.Value);
        controller.playerVelocity = Vector3.zero;
        yield return new WaitForSeconds(playerStats.DashCooldown.Value);
    }
}
