using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class KorosBossDeadState : EnemyBossBaseState
{
    KorosBossAIController controller; // Ref to controller

    // Audio code
    AudioSource audioSource; // Audio source reference

    public override void EnterState(EnemyBossBaseController baseController)
    {
        print("Koros boss entered dead state!");

        controller = (KorosBossAIController)baseController;

        audioSource = controller.GetComponent<AudioSource>(); // Audio source code

        PlaySound(controller.deathSound); // Plays death sound when whale is defeated
    }

    public override void UpdateState()
    {
        //print("Updating dead state");
        controller.healthText.text = (controller.health.ToString() + "/" + controller.maxHealth);
    }

    public override void ExitState()
    {
        print("Koros boss exited dead state!");

        Destroy(this);
    }

    public void PlaySound(AudioClip clip) // Allows the enemy to play sounds
    {
        audioSource.PlayOneShot(clip);
    }
}
