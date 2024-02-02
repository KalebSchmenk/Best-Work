using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunarFlareMajorCard : MajorCardBase
{
    [Header("Lunar Flare Refs and Settings")]
    public GameObject lunarFlare; // Lunar Flare object for VFX and damaging
    public GameObject lunarFlareGhost; // Lunar Flare object ghost
    public float damageOutput = 10f;
    public float enableDamageAfterSpawnedIn = 1f;
    public float destroyAfterEnableDamageIn = 0.5f;

    GameObject cam; 
    GameObject spawnedFlareGhost; // Spawned Flare ghost
    Transform bulletSpawnTrans;
    Coroutine ghostRotateCoroutine;

    public LayerMask rayCollisionMask; // Anything set in this mask will cause the raycast to hit and the flare will target

    // On ability key down create lunar flare ghost
    public override void AbilityKeyDown()
    {
        if (GetCooldown()) return; // Guard clause. If we are cooling down - return

        print("Lunar Flare key down");
        if (spawnedFlareGhost == null) SpawnGhost();
    }

    // On ability key up do lunar flare ability and destroy ghost
    public override void AbilityKeyUp()
    {
        if (GetCooldown() || spawnedFlareGhost == null) return; // Guard clause. If we are cooling down - return.

        print(this + " called its ability");

        LunarFlare();

        StartCooldown();
    }

    // Spawns lunar flare
    private void LunarFlare()
    {
        var lunarFlareSpawnedObj = Instantiate(lunarFlare, spawnedFlareGhost.transform.position, spawnedFlareGhost.transform.rotation);
        LunarFlareController lunarFlareController = lunarFlareSpawnedObj.GetComponentInChildren<LunarFlareController>();
        lunarFlareController.damage = damageOutput;
        lunarFlareController.enableDamageIn = enableDamageAfterSpawnedIn;
        lunarFlareController.destroyAfterEnableIn = destroyAfterEnableDamageIn;

        DestroyGhost();
    }

    // Spawns ghost on the ground
    private void SpawnGhost()
    {
        spawnedFlareGhost = Instantiate(lunarFlareGhost, bulletSpawnTrans.position, player.transform.rotation);
        spawnedFlareGhost.transform.position = bulletSpawnTrans.transform.position;
        spawnedFlareGhost.transform.parent = player.transform;
        spawnedFlareGhost.gameObject.name = "Lunar Flare Ghost";

        if (ghostRotateCoroutine == null) ghostRotateCoroutine = StartCoroutine(RotateGhostToFollowCam());
    }

    // Moves ghost indicator based on where player is looking
    // Coroutine
    private IEnumerator RotateGhostToFollowCam()
    {
        while (true)
        {
            if (spawnedFlareGhost != null)
            {
                RaycastHit raycastHit = RayCast(cam.transform.position, cam.transform.forward, Mathf.Infinity, rayCollisionMask);
                print(raycastHit.point);
                spawnedFlareGhost.transform.LookAt(raycastHit.point);
            }
            else
            {
                yield break;
            }
            yield return null; // Waits next frame
        }
    }

    // Destroys ghost
    private void DestroyGhost()
    {
        if (ghostRotateCoroutine != null) StopCoroutine(ghostRotateCoroutine);
        ghostRotateCoroutine = null;
        Destroy(spawnedFlareGhost);
        spawnedFlareGhost = null;
    }

    // Grabs references when added to player inventory
    public override void OnAdd()
    {
        base.OnAdd();

        bulletSpawnTrans = player.GetComponent<PlayerWeaponController>().projectileSpawnPosition;

        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Prints to console that this card was removed
    public override void OnRemove()
    {
        base.OnRemove();
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
