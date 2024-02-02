using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EarthenPillarMajorCard : MajorCardBase
{
    [Header("Earthen Pillar Refs and Settings")]
    public GameObject pillar; // Pillar object reference
    public GameObject ghostPillar; // Ghost indicator for pillar object
    public LayerMask everythingButEnemyAndPlayerMask;
    public LayerMask everythingButEnemyMask;
    public float maxSpawnDistance = 6.5f; // Max spawn pos for pillar
    public float spawnBehindGhostBy = 7.5f; // How far behind the ghost pillar the real pillar spawns

    public Vector3 VFX_Pos;

    private GameObject spawnedGhostPillar; // Spawned indicator
    GameObject cam; // Camera reference
    Coroutine moveGhostCoroutine; // Move Ghost Coroutine ref
    Vector3 playerPosMod;


    // On ability key down create ghost indicator
    public override void AbilityKeyDown()
    {
        if (GetCooldown()) return; // Guard clause. If we are cooling down - return

        print("Earthen Pillar key down");
        SpawnGhost();
        if (moveGhostCoroutine == null) moveGhostCoroutine = StartCoroutine(MoveGhostCoroutine());
    }

    
    // Moves ghost indicator based on where player is looking
    // Coroutine
    private IEnumerator MoveGhostCoroutine()
    {
        while (true)
        {
            playerPosMod = player.transform.position;
            playerPosMod.y += 1.5f;

            var rayHit = RayCast(cam.transform.position, cam.transform.forward, maxSpawnDistance, everythingButEnemyAndPlayerMask);

            if (rayHit.collider != null && rayHit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")) // If ray from hit something and is on the ground layer
            {
                if (spawnedGhostPillar == null) SpawnGhost(); // We need it if it doesnt exist

                spawnedGhostPillar.transform.position = rayHit.point;
                var lookRot = rayHit.normal;
                spawnedGhostPillar.transform.rotation = Quaternion.FromToRotation(spawnedGhostPillar.transform.up, lookRot) * spawnedGhostPillar.transform.rotation;

                if (spawnedGhostPillar.transform.up.y > 0.99f) // If ghost is upright face the camera
                {
                    var upLookRot = cam.transform.forward;
                    upLookRot.y = 0;
                    spawnedGhostPillar.transform.rotation = Quaternion.LookRotation(upLookRot);
                }
            }
            else // If ray from cam did not hit something on the layer mask
            {
                var spawnPos = TryFindSpawnPosition();

                if (spawnPos == Vector3.zero)
                {
                    DestroyGhost();
                }
                else
                {
                    if (spawnedGhostPillar == null) SpawnGhost(); // We need it if it doesnt exist

                    var camRot = cam.transform.forward;
                    camRot.y = 0;
                    spawnedGhostPillar.transform.position = spawnPos;
                    spawnedGhostPillar.transform.rotation = Quaternion.LookRotation(camRot);
                }
            }
            yield return null; // Waits next frame
        }
    }

    // On ability key up do pillar spawn ability and destroy ghost indicator
    public override void AbilityKeyUp()
    {
        if (GetCooldown() || moveGhostCoroutine == null) return; // Guard clause. If we are cooling down - return. Or if ghost coroutine is empty

        print(this + " called its ability");

        StopCoroutine(moveGhostCoroutine);
        moveGhostCoroutine = null;

        if (spawnedGhostPillar == null) // Guard clause in case no ghost was ever shown to player
        {
            Debug.LogWarning("No place to spawn ghost. Pillar summon aborted");
            return;
        }

        SummonEarthenPillar();

        PlayerEvents.OnAbilityUsed?.Invoke(this);

        StartCooldown();
    }

    // Earthen Pillar ability
    private void SummonEarthenPillar()
    {
        VFX_Pos = spawnedGhostPillar.transform.position;
        var point = spawnedGhostPillar.transform.position - (spawnedGhostPillar.transform.up * spawnBehindGhostBy);
        var lookRot = spawnedGhostPillar.transform.rotation;
        DestroyGhost();

        Instantiate(pillar, point, lookRot);
        Invoke(nameof(SpawnVFX), .2f);
    }

    // Grabs references when added to player inventory
    public override void OnAdd()
    {
        base.OnAdd();

        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Prints to console that this card was removed
    public override void OnRemove()
    {
        base.OnRemove();
    }

    private void SpawnGhost()
    {
        spawnedGhostPillar = Instantiate(ghostPillar, player.transform.position, player.transform.rotation);
        spawnedGhostPillar.gameObject.name = "Ghost Earthen Pillar";
    }

    private void DestroyGhost()
    {
        Destroy(spawnedGhostPillar);
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
        Instantiate(vfx.gameObject, VFX_Pos, Quaternion.identity);
    }
}
