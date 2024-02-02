using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarBeamMajorCard : MajorCardBase
{
    [Header("Solar Beam Refs and Settings")]
    public GameObject targetGroundReticle; // Gameobject of targetting ground reticle
    public GameObject solarBeam; // Solar Beam object for VFX and damaging
    public float maxSpawnDistance = 25f; // Max spawn pos for solar beam
    public float damageOutput = 10f;
    public float enableDamageAfterSpawnedIn = 1f;
    public float destroyAfterEnableDamageIn = 0.5f;

    public LayerMask everythingButEnemyAndPlayerMask;
    public LayerMask everythingButEnemyMask;
    Vector3 playerPosMod;

    GameObject spawnedTargetGroundReticle; // Spawned ground targetting reticle
    GameObject cam; // Camera reference
    Coroutine moveTargetReticleCoroutine; // Move Target Reticle Coroutine ref

    public Vector3 VFX_Pos;
    PlayerController playerController; // Player controller script reference
    public AudioClip targetLockedSound; // Target Locked sound effect


    // On ability key down create target reticle indicator
    public override void AbilityKeyDown()
    {
        if (GetCooldown()) return; // Guard clause. If we are cooling down - return

        print("Solar Beam key down");
        if (moveTargetReticleCoroutine == null) moveTargetReticleCoroutine = StartCoroutine(MoveTargetReticleCoroutine());
    }


    // On ability key up do solar beam ability and destroy target reticle
    public override void AbilityKeyUp()
    {
        if (GetCooldown() || moveTargetReticleCoroutine == null) return; // Guard clause. If we are cooling down - return. Or if target reticle coroutine is empty

        print(this + " called its ability");

        StopCoroutine(moveTargetReticleCoroutine);
        moveTargetReticleCoroutine = null;

        if (spawnedTargetGroundReticle == null) // Guard clause in case no target reticle was ever shown to player
        {
            Debug.LogWarning("No place to summon solar beam. Solar beam summon aborted");
            return;
        }

        SolarBeam();
        Invoke(nameof(SpawnVFX), enableDamageAfterSpawnedIn);

        StartCooldown();
    }

    // Spawns solar beam
    private void SolarBeam()
    {
        var solarBeamSpawnedObj = Instantiate(solarBeam, spawnedTargetGroundReticle.transform.position, solarBeam.transform.rotation);
        SolarBeamController solarBeamController = solarBeamSpawnedObj.GetComponent<SolarBeamController>();
        solarBeamController.damage = damageOutput;
        solarBeamController.enableDamageIn = enableDamageAfterSpawnedIn;
        solarBeamController.destroyAfterEnableIn = destroyAfterEnableDamageIn;

        VFX_Pos = spawnedTargetGroundReticle.transform.position;

        DestroyTargetReticle();
    }

    // Spawns target reticle on the ground
    private void SpawnTargetReticle()
    {
        spawnedTargetGroundReticle = Instantiate(targetGroundReticle, player.transform.position, player.transform.rotation);
        spawnedTargetGroundReticle.gameObject.name = "Solar Beam Target";
    }

    // Destroys target reticle on ground
    private void DestroyTargetReticle()
    {
        Destroy(spawnedTargetGroundReticle);

        playerController.PlaySound(targetLockedSound); // Plays target locked sound
    }

    // Moves ghost indicator based on where player is looking
    // Coroutine
    private IEnumerator MoveTargetReticleCoroutine()
    {
        while (true)
        {
            playerPosMod = player.transform.position;
            playerPosMod.y += 1.5f;

            var rayHit = RayCast(cam.transform.position, cam.transform.forward, maxSpawnDistance, everythingButEnemyAndPlayerMask);

            if (rayHit.collider != null && rayHit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")) // If ray from hit something and is on the ground layer
            {
                if (spawnedTargetGroundReticle == null) SpawnTargetReticle(); // We need it if it doesnt exist

                spawnedTargetGroundReticle.transform.position = rayHit.point;
                var lookRot = rayHit.normal;
                spawnedTargetGroundReticle.transform.rotation = Quaternion.FromToRotation(spawnedTargetGroundReticle.transform.up, lookRot) * spawnedTargetGroundReticle.transform.rotation;

                if (spawnedTargetGroundReticle.transform.up.y > 0.99f) // If ghost is upright face the camera
                {
                    var upLookRot = cam.transform.forward;
                    upLookRot.y = 0;
                    spawnedTargetGroundReticle.transform.rotation = Quaternion.LookRotation(upLookRot);
                }
            }
            else // If ray from cam did not hit something on the layer mask
            {
                var spawnPos = TryFindSpawnPosition();

                if (spawnPos == Vector3.zero)
                {
                    DestroyTargetReticle();
                }
                else
                {
                    if (spawnedTargetGroundReticle == null) SpawnTargetReticle(); // We need it if it doesnt exist

                    var camRot = cam.transform.forward;
                    camRot.y = 0;
                    spawnedTargetGroundReticle.transform.position = spawnPos;
                    spawnedTargetGroundReticle.transform.rotation = Quaternion.LookRotation(camRot);
                }
            }
            yield return null; // Waits next frame
        }
    }

    // Ground Check Raycast
    private RaycastHit RayCast(Vector3 from, Vector3 dir, float len, LayerMask layerMask)
    {
        RaycastHit hit;

        //Debug.DrawLine(from, from + (dir * maxSpawnDistance), UnityEngine.Color.green); // Debug draw

        if (Physics.Raycast(from, dir, out hit, len, layerMask, QueryTriggerInteraction.Ignore))
        {
            return hit;
        }

        return new RaycastHit();
    }

    // Grabs references when added to player inventory
    public override void OnAdd()
    {
        base.OnAdd();

        cam = GameObject.FindGameObjectWithTag("MainCamera");

        playerController = player.GetComponent<PlayerController>();
    }

    // Prints to console that this card was removed
    public override void OnRemove()
    {
        base.OnRemove();
    }

    private Vector3 TryFindSpawnPosition()
    {
        int i = 0;
        var camRot = cam.transform.forward;
        camRot.y = 0;

        while (true)
        {
            //print("Iteration: " + i);
            if (i >= 25) return Vector3.zero;

            var rayHit = RayCast(cam.transform.position + (camRot * (maxSpawnDistance - i)), -Vector3.up, Mathf.Infinity, everythingButEnemyAndPlayerMask);

            i++;
            if (rayHit.collider == null || rayHit.collider.gameObject.layer != 8) continue;

            var camDownY = cam.transform.position;
            camDownY.y = -100;

            //Debug.DrawLine(cam.transform.position + (camRot * (maxSpawnDistance - i)), camDownY, UnityEngine.Color.green);

            RaycastHit hitToPlayer;
            if (Physics.Raycast(rayHit.point, playerPosMod - rayHit.point, out hitToPlayer, Mathf.Infinity, everythingButEnemyMask, QueryTriggerInteraction.Ignore))
            {
                if (hitToPlayer.collider.gameObject.layer == LayerMask.NameToLayer("Player")) // 6 = player layer
                {
                    return rayHit.point - (camRot * 2f);
                }
            }
        }
    }

    public override void SpawnVFX()
    {
        Vector3 rotation = vfx.gameObject.transform.rotation.eulerAngles;
        rotation += new Vector3(-90f, 0, 0);

        Instantiate(vfx.gameObject, VFX_Pos, Quaternion.Euler(rotation));
    }
}
