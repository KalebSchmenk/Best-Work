using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunarWarpMajorCard : MajorCardBase
{
    [Header("Lunar Warp Refs and Settings")]
    public GameObject decoy; // Decoy game object
    public GameObject ghostPlayer; // Indicator for where the player will teleport to 
    private GameObject spawnedGhostPlayer; // Spawned indicator for above
    public LayerMask defaultLayerMask; // Layer mask for ray cast
    public float teleportForward = 20f; // How far forward player teleports


    [System.NonSerialized] public GameObject spawnedDecoy; // Spawned decoy in the game

    PlayerController playerController; // Player controller script reference
    CharacterController characterController; // Character controller script reference
    GameObject mainCam; // Main camera gameobject reference
    CinemachineVirtualCamera cam; // Reference to main cam cinemachine
    Coroutine moveGhostCoroutine; // Move Ghost Coroutine ref

    public AudioClip warpSound; // Warp sound effect


    // On ability key down create ghost indicator
    public override void AbilityKeyDown()
    {
        if (GetCooldown()) return; // Guard clause. If we are cooling down - return

        print("Lunar Warp key down");
        if (moveGhostCoroutine == null) moveGhostCoroutine = StartCoroutine(MoveGhostCoroutine());
    }

    
    // Moves ghost indicator based on where player is looking
    private IEnumerator MoveGhostCoroutine()
    {
        while (true)
        {
            var rayHit = RayCast(mainCam.transform.position, mainCam.transform.forward);
            if (rayHit.collider != null)
            {
                if (spawnedGhostPlayer == null) SpawnGhost(); // We need it if it doesnt exist

                spawnedGhostPlayer.transform.position = rayHit.point;
                var normalDir = rayHit.normal;
                spawnedGhostPlayer.transform.position += normalDir * 1.01f;
                var lookRot = mainCam.transform.forward;
                lookRot.y = 0;
                spawnedGhostPlayer.transform.rotation = Quaternion.LookRotation(lookRot);
            }
            else
            {
                if (spawnedGhostPlayer == null) SpawnGhost(); // We need it if it doesnt exist

                var lookRot = mainCam.transform.forward;
                spawnedGhostPlayer.transform.position = mainCam.transform.position + (lookRot * teleportForward);
                lookRot.y = 0;
                spawnedGhostPlayer.transform.rotation = Quaternion.LookRotation(lookRot);
            }
            yield return null;
        }
    }

    // On ability key up do teleport ability and destroy ghost indicator
    public override void AbilityKeyUp()
    {
        if (GetCooldown() || moveGhostCoroutine == null) return; // Guard clause. If we are cooling down - return. Or if ghost coroutine is empty
        

        print(this + " called its ability");

        StopCoroutine(moveGhostCoroutine);
        moveGhostCoroutine = null;

        if (spawnedGhostPlayer == null) // Guard clause in case no ghost was ever shown to player
        {
            Debug.LogWarning("No place to spawn ghost. Teleport aborted");
            return;
        }

        Destroy(spawnedGhostPlayer);

        spawnedDecoy = Instantiate(decoy, player.transform.position, player.transform.rotation).gameObject;

        Destroy(spawnedDecoy, 5);

        playerController.PlaySound(warpSound);

        LunarWarp();

        StartCooldown();

        PlayerEvents.OnAbilityUsed?.Invoke(this);
        
        SpawnVFX();
    }

    // Lunar Warp Ability
    private void LunarWarp()
    {
        playerController.enabled = false;
        characterController.enabled = false;
        var oldPlayerPos = player.transform.position;
        player.transform.position = spawnedGhostPlayer.transform.position;
        cam.OnTargetObjectWarped(player.transform, spawnedGhostPlayer.transform.position - oldPlayerPos);
        playerController.enabled = true;
        characterController.enabled = true;
    }

    // Grabs references when added to player inventory
    public override void OnAdd()
    {
        base.OnAdd();

        cam = GameManager.instance.cinemachineCam.GetComponent<CinemachineVirtualCamera>();
        mainCam = GameManager.instance.mainCamera;

        playerController = player.GetComponent<PlayerController>();
        characterController = player.GetComponent<CharacterController>();
    }

    // Prints to console that this card was removed
    public override void OnRemove()
    {
        base.OnRemove();
    }

    // Spawns ghost that indicates where player will teleport to
    private void SpawnGhost()
    {
        spawnedGhostPlayer = Instantiate(ghostPlayer, player.transform.position, player.transform.rotation);
        spawnedGhostPlayer.gameObject.name = "Ghost Player";
    }

    // Ground Check Raycast
    private RaycastHit RayCast(Vector3 from, Vector3 dir)
    {
        RaycastHit hit;

        //Debug.DrawLine(from, from + (dir * maxSpawnDistance), UnityEngine.Color.green); // Debug draw

        if (Physics.Raycast(from, dir, out hit, teleportForward, defaultLayerMask))
        {
            return hit;
        }

        return new RaycastHit();
    }

    public override void SpawnVFX()
    {
        Vector3 rotation = playerController.transform.rotation.eulerAngles;
        rotation += new Vector3(0, -180, 0);

        GameObject.Instantiate(vfx.gameObject, playerController.gameObject.transform.position, Quaternion.Euler(rotation), playerController.transform);
    }
}
