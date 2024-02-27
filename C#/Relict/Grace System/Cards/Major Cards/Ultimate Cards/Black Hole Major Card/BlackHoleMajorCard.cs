using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleMajorCard : MajorCardBase
{
    [Header("Black Hole Refs and Settings")]
    public GameObject blackHole; // BlackHole object reference
    public GameObject ghostBlackHole; // Ghost indicator for BlackHole object
    public float maxSpawnDistance = 55f; // Max spawn pos for BlackHole

    private LayerMask everythingLayer = ~0;
    private GameObject spawnedBlackHoleGhost; // Spawned indicator
    GameObject cam; // Camera reference
    Coroutine moveGhostCoroutine; // Move Ghost Coroutine ref


    // On ability key down create ghost indicator
    public override void AbilityKeyDown()
    {
        if (GetCooldown()) return; // Guard clause. If we are cooling down - return

        print("Black Hole key down");
        SpawnGhost();
        if (moveGhostCoroutine == null) moveGhostCoroutine = StartCoroutine(MoveGhostCoroutine());
    }


    // Moves ghost indicator based on where player is looking
    // Coroutine
    private IEnumerator MoveGhostCoroutine()
    {
        while (true)
        {
            var rayHit = RayCast(cam.transform.position, cam.transform.forward, maxSpawnDistance, everythingLayer);
            if (rayHit.collider != null)
            {
                if (spawnedBlackHoleGhost == null) SpawnGhost(); // We need it if it doesnt exist

                spawnedBlackHoleGhost.transform.position = rayHit.point;
                var normalDir = rayHit.normal;
                spawnedBlackHoleGhost.transform.position += normalDir * 1.01f;
                var lookRot = cam.transform.forward;
                lookRot.y = 0;
                spawnedBlackHoleGhost.transform.rotation = Quaternion.LookRotation(lookRot);
            }
            else
            {
                if (spawnedBlackHoleGhost == null) SpawnGhost(); // We need it if it doesnt exist

                var lookRot = cam.transform.forward;
                spawnedBlackHoleGhost.transform.position = cam.transform.position + (lookRot * maxSpawnDistance);
                lookRot.y = 0;
                spawnedBlackHoleGhost.transform.rotation = Quaternion.LookRotation(lookRot);
            }
            yield return null;
        }
    }

    // On ability key up do black hole spawn ability and destroy ghost indicator
    public override void AbilityKeyUp()
    {
        if (GetCooldown() || moveGhostCoroutine == null) return; // Guard clause. If we are cooling down - return. Or if ghost coroutine is empty

        print(this + " called its ability");

        StopCoroutine(moveGhostCoroutine);
        moveGhostCoroutine = null;

        if (spawnedBlackHoleGhost == null) // Guard clause in case no ghost was ever shown to player
        {
            Debug.LogWarning("No place to spawn ghost. Black Hole summon aborted");
            return;
        }

        SummonBlackHole();

        PlayerEvents.OnAbilityUsed?.Invoke(this);

        StartCooldown();
    }

    // Black Hole ability
    private void SummonBlackHole()
    {
        var point = spawnedBlackHoleGhost.transform.position;
        var lookRot = spawnedBlackHoleGhost.transform.rotation;
        DestroyGhost();

        Instantiate(blackHole, point, lookRot);

        AudioManager.instance.PlaySfx("BlackHole"); // Plays black hole sound
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
        spawnedBlackHoleGhost = Instantiate(ghostBlackHole, player.transform.position, player.transform.rotation);
        spawnedBlackHoleGhost.gameObject.name = "Ghost Black Hole";
    }

    private void DestroyGhost()
    {
        Destroy(spawnedBlackHoleGhost);
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
}
