using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiantSphereMajorCard : MajorCardBase
{
    [Header("Radiant Sphere Refs and Settings")]
    public GameObject radiantSphere; // Radiant sphere object reference
    public GameObject ghostRadiantSphere; // Ghost indicator for Radiant sphere object
    public float maxSpawnDistance = 55f; // Max spawn pos for Radiant sphere

    private LayerMask everythingLayer = ~0;
    private GameObject spawnedGhost; // Spawned indicator
    GameObject cam; // Camera reference
    Coroutine moveGhostCoroutine; // Move Ghost Coroutine ref


    // On ability key down create ghost indicator
    public override void AbilityKeyDown()
    {
        if (GetCooldown()) return; // Guard clause. If we are cooling down - return

        print("Radiant Sphere key down");
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
                if (spawnedGhost == null) SpawnGhost(); // We need it if it doesnt exist

                spawnedGhost.transform.position = rayHit.point;
                var normalDir = rayHit.normal;
                spawnedGhost.transform.position += normalDir * 1.01f;
                var lookRot = cam.transform.forward;
                lookRot.y = 0;
                spawnedGhost.transform.rotation = Quaternion.LookRotation(lookRot);
            }
            else
            {
                if (spawnedGhost == null) SpawnGhost(); // We need it if it doesnt exist

                var lookRot = cam.transform.forward;
                spawnedGhost.transform.position = cam.transform.position + (lookRot * maxSpawnDistance);
                lookRot.y = 0;
                spawnedGhost.transform.rotation = Quaternion.LookRotation(lookRot);
            }
            yield return null;
        }
    }

    // On ability key up do radiant sphere spawn ability and destroy ghost indicator
    public override void AbilityKeyUp()
    {
        if (GetCooldown() || moveGhostCoroutine == null) return; // Guard clause. If we are cooling down - return. Or if ghost coroutine is empty

        print(this + " called its ability");

        StopCoroutine(moveGhostCoroutine);
        moveGhostCoroutine = null;

        if (spawnedGhost == null) // Guard clause in case no ghost was ever shown to player
        {
            Debug.LogWarning("No place to spawn ghost. Black Hole summon aborted");
            return;
        }

        SummonRadiantSphere();

        PlayerEvents.OnAbilityUsed?.Invoke(this);

        StartCooldown();
    }

    // Radiant Sphere ability
    private void SummonRadiantSphere()
    {
        var point = spawnedGhost.transform.position;
        var lookRot = spawnedGhost.transform.rotation;
        DestroyGhost();

        Instantiate(radiantSphere, point, lookRot);
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
        spawnedGhost = Instantiate(ghostRadiantSphere, player.transform.position, player.transform.rotation);
        spawnedGhost.gameObject.name = "Ghost Radiant Sphere";
    }

    private void DestroyGhost()
    {
        Destroy(spawnedGhost);
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
