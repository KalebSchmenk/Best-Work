using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectralSustenanceMajorCard : MajorCardBase
{
    [Header("Spectral Sustenance References")]
    public float maxTargetDistance = 15f; // Max target distance
    public float healOnSteal = 50f; // How much health to give back to player

    PlayerHealth playerHealth;
    AIMain aiMain;
    Coroutine enemyRayFinderCoroutine; // reference for the raycast coroutine
    GameObject cam; // Cam reference

    public override void AbilityKeyDown()
    {
        if (GetCooldown()) return; // Check if we're still cooling down

        if (enemyRayFinderCoroutine == null) enemyRayFinderCoroutine = StartCoroutine(EnemyRayFinder());

        print("Spectral Sustenance Key Down");
    }

    public override void AbilityKeyUp()
    {
        if (GetCooldown() || enemyRayFinderCoroutine == null) return; // if cooling down guard claause

        StopCoroutine(enemyRayFinderCoroutine);
        enemyRayFinderCoroutine = null;

        // If we found an enemy and they are dead, steal health
        if (aiMain != null)
        {
            if (aiMain.state == AIMain.AiState.dead)
            {
                StealLife(); 
            }
        }
    }

    // Grabs references when added to player inventory
    public override void OnAdd()
    {
        base.OnAdd();

        cam = GameObject.FindGameObjectWithTag("MainCamera");
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    // Prints to console that this card was removed
    public override void OnRemove()
    {
        base.OnRemove();
    }

    // Finds an enemy (NEEDS REFACTORED WHEN ENEMIES ARE REFACTORED)
    IEnumerator EnemyRayFinder()
    {
        while (true)
        {
            var rayHit = RayCast(cam.transform.position, cam.transform.forward, maxTargetDistance);

           // if (rayHit.collider == null) yield return null; // Guard clause

            if (rayHit.collider != null && rayHit.collider.gameObject.TryGetComponent<AIMain>(out AIMain main)) 
            {
                aiMain = main;
            }

            yield return null; // Yields next frame
        }
    }

    // Steals health from soul of dead enemy
    private void StealLife()
    {
        if (aiMain != null) // If we found any enemy
        {
            playerHealth.ChangePlayerHealth(healOnSteal);
            StartCooldown(); // Only on successful steal start the cooldown
            aiMain = null;
        }
    }

    private RaycastHit RayCast(Vector3 from, Vector3 dir, float len)
    {
        RaycastHit hit;

        //Debug.DrawRay(transform.position, cam.transform.forward, Color.green);

        if (Physics.Raycast(from, dir, out hit, len))
        {
            return hit;
        }

        return new RaycastHit();
    }
}
