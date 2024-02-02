using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class KorosBossWanderState : EnemyBossBaseState
{
    public Vector3[] waypoints = new Vector3[30];
    Vector3 currentWaypoint;

    KorosBossAIController controller; // Controller ref

    private float startAttackEvery = 15f;

    private float speed = 5f;
    private float upAndDownSpeed = 8.5f;
    private float waitAtWaypointFor = 0.01f;
    private float fireProjectileEvery = 5f;

    private bool moving = true;
    private bool inCooldown = false;
    private bool inProjectileCooldown = false;
    
    private float minWander;
    private float maxWander;

    // Audio code
    AudioSource audioSource; // Audio source reference

    public override void EnterState(EnemyBossBaseController baseController)
    {
        print("Koros boss entered wander state!");

        controller = (KorosBossAIController)baseController;

        startAttackEvery = controller.startAttackEvery;
        minWander = controller.minWanderWaypointSpawn;
        maxWander = controller.maxWanderWaypointSpawn;
        speed = controller.wanderSpeed; 
        upAndDownSpeed = controller.wanderUpAndDownSpeed; 
        waitAtWaypointFor = controller.wanderWaitAtWaypointFor;
        fireProjectileEvery = controller.fireProjectileEvery;

        audioSource = controller.GetComponent<AudioSource>(); // Audio source code

        controller.enteredWanderState++;

        if (controller.enteredWanderState >= 3)
        {
            controller.SpawnEnemies();
            controller.enteredWanderState = 0;
        }

        GenerateWaypoints();

        currentWaypoint = waypoints[UnityEngine.Random.Range(0, waypoints.Length)];

        Invoke("SwitchState", startAttackEvery);

        PlaySound(controller.wanderStartSound); // Plays a sound when the boss enters wander state
    }

    public override void UpdateState()
    {
        if (Vector3.Distance(this.transform.position, currentWaypoint) < 2.5f && !inCooldown)
        {
            StartCoroutine(WanderCooldown());
        }

        if (moving)
        {
            var lookAt = currentWaypoint - this.transform.position;
            lookAt.y = 0f;
            var rotQuat = Quaternion.LookRotation(lookAt);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotQuat, 30f * Time.deltaTime);

            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            if (transform.position.y > controller.startPos.y + 0.25f)
            {
                transform.Translate(Vector3.down * upAndDownSpeed * Time.deltaTime);
            }
            else if (transform.position.y < controller.startPos.y - 0.25f)
            {
                transform.Translate(Vector3.up * upAndDownSpeed * Time.deltaTime);
            }
        }

        if (!inProjectileCooldown)
        {
            ShootProjectile();
        }
    }

    public override void ExitState()
    {
        print("Koros boss exited wander state!");

        PlaySound(controller.wanderExitSound); // Plays a sound when the boss exits wander state

        Destroy(this);
    }

    private void SwitchState()
    {
        controller.SwitchState(EnemyBossBaseController.BossStates.Attack);
    }

    private void GenerateWaypoints()
    {
        for (int i = 0; i < waypoints.Length; i++)
        {
            float randomZ = controller.startPos.z + UnityEngine.Random.Range(minWander, maxWander);
            float randomX = controller.startPos.x + UnityEngine.Random.Range(minWander, maxWander);
            waypoints[i] = new Vector3(randomX, controller.startPos.y, randomZ);
        }
    }

    private IEnumerator WanderCooldown()
    {
        moving = false;
        inCooldown = true;
        yield return new WaitForSeconds(waitAtWaypointFor);
        inCooldown = false;

        int i = 0;
        do
        {
            currentWaypoint = waypoints[UnityEngine.Random.Range(0, waypoints.Length)];

            i++;
            if (i > 50)
            {
                Debug.LogWarning("Couldnt find a far enough away waypoint. Going back to spawn");
                currentWaypoint = controller.startPos;
                break;
            }
        } while (Vector3.Distance(currentWaypoint, this.transform.position) < 30f);


        moving = true;
    }

    private void ShootProjectile()
    {
        var proj = Instantiate(controller.crystalProjectile, controller.projectileSpawnPos.position, Quaternion.identity);
        proj.transform.rotation = controller.projectileSpawnPos.rotation;
        proj.transform.parent = this.transform;
        StartCoroutine(ProjectileCooldown());
    }

    private IEnumerator ProjectileCooldown()
    {
        inProjectileCooldown = true;
        yield return new WaitForSeconds(fireProjectileEvery);
        inProjectileCooldown = false;
    }

    public void PlaySound(AudioClip clip) // Allows the enemy to play sounds
    {
        audioSource.PlayOneShot(clip);
    }
}
