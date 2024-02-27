using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KorosBossAttackState : EnemyBossBaseState
{
    public delegate void AttackComplete();
    public static AttackComplete AttackFinished; // Attack is complete event

    KorosBossAIController controller; // Ref to controller

    private float moveToSpawnSpeed = 4.5f;
    private float upAndDownSpeed = 8.5f;
    private float dashSetupRotateSpeed = 30f;
    private float dashAtGroundRotateSpeed = 45f;
    private float startDashAfter = 2f;
    private float dashAttackRotateSpeed = 45f;
    private float dashAttackMoveSpeed = 25.5f;
    private float dashAttackDamageOutput = 250f;

    private int shockwaveCount = 3;
    private float spawnShockwaveEvery = 1.5f;

    bool startAttack = false;
    bool inAttack = false;
    //bool attackDone = false; // If attack is done bool // bool is not used anywhere so I commented it out - Noah
    bool listeningForCollision = false;
    bool hitSomething = false;

    // Audio code
    AudioSource audioSource; // Audio source reference

    public override void EnterState(EnemyBossBaseController baseController)
    {
        print("Koros boss entered attack state!");

        controller = (KorosBossAIController)baseController; 

        AttackFinished += AttackIsOver;

        moveToSpawnSpeed = controller.moveToSpawnSpeed;
        upAndDownSpeed = controller.upAndDownSpeed;
        dashSetupRotateSpeed = controller.dashSetupRotateSpeed;
        dashAtGroundRotateSpeed = controller.dashAtGroundRotateSpeed;
        startDashAfter = controller.startDashAfter;
        dashAttackRotateSpeed = controller.dashAttackRotateSpeed;
        dashAttackMoveSpeed = controller.dashAttackMoveSpeed;
        dashAttackDamageOutput = controller.dashAttackDamageOutput;

        shockwaveCount = controller.shockwaveCount;
        spawnShockwaveEvery = controller.spawnShockwaveEvery;

        audioSource = controller.GetComponent<AudioSource>(); // Audio source code
}

    public override void UpdateState()
    {
        //print("Updating attack state");

        if (!startAttack)
        {
            GoToSpawn();
        }

        if (startAttack && !inAttack)
        {
            int randomNum = UnityEngine.Random.Range(0, 101);

            if (randomNum > 50)
            {
                DashAttack();
            }
            else
            {
                //SpawnShockwave();
                DashAttack();
            }

            inAttack = true;
        }
    }

    public override void ExitState()
    {
        print("Koros boss exited attack state!");

        AttackFinished -= AttackIsOver;

        Destroy(this);
    }

    // Spawns shockwave
    private void SpawnShockwave()
    {
        StartCoroutine(SpawnShockwaves());

        PlaySound(controller.ambientSound1); // Plays a sound as the whale performs gavel attack
    }

    // Starts shockwave attack
    private void DashAttack()
    {
        StartCoroutine(DashAttackPrepCoroutine());

        PlaySound(controller.ambientSound3); // Plays a sound as the whale descends for dash attack
    }

    // Func to switch attackdone bool to true
    private void AttackIsOver()
    {
        //attackDone = true;
        controller.SwitchState(EnemyBossBaseController.BossStates.Wander);
    }

    // Goes to spawn pos
    private void GoToSpawn()
    {
        var lookAt = controller.startPos - this.transform.position;
        lookAt.y = 0;
        var rotQuat = Quaternion.LookRotation(lookAt);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotQuat, dashSetupRotateSpeed * Time.deltaTime);

        transform.Translate(Vector3.forward * moveToSpawnSpeed * Time.deltaTime);

        if (transform.position.y > controller.startPos.y + 0.25f)
        {
            transform.Translate(Vector3.down * upAndDownSpeed * Time.deltaTime);
        }
        else if (transform.position.y < controller.startPos.y - 0.25f)
        {
            transform.Translate(Vector3.up * upAndDownSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(this.transform.position, controller.startPos) < 1.5f)
        {
            startAttack = true;
        }
    }

    // Prep for dash attack
    private IEnumerator DashAttackPrepCoroutine()
    {
        while (true) // Rotate correctly
        {
            if (this.transform.eulerAngles == Vector3.zero) break;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.zero), dashSetupRotateSpeed * Time.deltaTime);

            yield return null;
        }

        Vector3 spawnAt = new Vector3();
        RaycastHit hit;
        if (Physics.Raycast(controller.startPos, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore)) // Find ground
        {
            if (hit.collider != null)
            {
                spawnAt = hit.point;
                spawnAt.y += 0.5f;
            }
            else
            {
                Debug.LogWarning("Justice dash attack prep raycast did not hit anything");
                controller.SwitchState(EnemyBossBaseController.BossStates.Wander);
                yield break;
            }
        }

        while (true) // Move down to player level
        {
            //print("Dist: " + (this.transform.position.y - spawnAt.y));
            if (this.transform.position.y - spawnAt.y < 1.15f) break;
            transform.Translate(Vector3.down * moveToSpawnSpeed * Time.deltaTime);
            yield return null;
        }

        Transform playerPos = GameManager.instance.player.transform;

        while (true) // Rotate to look at player
        {
            var lookAt = playerPos.position - this.transform.position;
            lookAt.y = 0f;
            var rotQuat = Quaternion.LookRotation(lookAt);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotQuat, dashAtGroundRotateSpeed * Time.deltaTime);

            if (Vector3.Dot((playerPos.position - this.transform.position).normalized, this.transform.forward.normalized) >= 0.975f)
            {
                print("Charging player");
                StartCoroutine(DashAttackCoroutine());
                PlaySound(controller.prepSound); // Plays a sound as the whale preps for dash attack
                yield break;
            }
            yield return null;
        }
    }

    // Dash attack move
    private IEnumerator DashAttackCoroutine()
    {
        yield return new WaitForSeconds(startDashAfter);
        listeningForCollision = true;
        var rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        Transform playerPos = GameManager.instance.player.transform;

        GetComponentInChildren<Animator>().SetTrigger("Dash");

        PlaySound(controller.dashAttackSound); // Plays dash attack sound

        while (true) // Dash to player
        {
            
            var lookAt = playerPos.position - this.transform.position;
            lookAt.y = 0f;
            var rotQuat = Quaternion.LookRotation(lookAt);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotQuat, dashAttackRotateSpeed * Time.deltaTime);

            if (hitSomething)
            {
                listeningForCollision = false;
                rb.isKinematic = true;
                StartCoroutine(DelayedReturn());
                yield break;
            }

            transform.Translate(Vector3.forward * dashAttackMoveSpeed * Time.deltaTime);

            float justiceToStartPos = Vector3.Distance(this.transform.position, controller.startPos);
            float playerToStartPos = Vector3.Distance(playerPos.position, controller.startPos);

            if (justiceToStartPos - playerToStartPos > 15f) 
            {
                controller.SwitchState(EnemyBossBaseController.BossStates.Wander);
            }

            yield return null;
        }
    }

    private IEnumerator DelayedReturn()
    {
        yield return new WaitForSeconds(3.5f);
        hitSomething = false;
        AttackFinished?.Invoke();
    }

    // Spawns multiple shockwaves
    private IEnumerator SpawnShockwaves()
    {
        int spawnedCount = 0;
        while (true)
        {
            if (spawnedCount >= shockwaveCount)
            {
                AttackFinished?.Invoke();
                yield break;
            }

            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore))
            {
                if (hit.collider != null)
                {
                    Vector3 spawnAt = hit.point;
                    spawnAt.y += .75f;

                    GetComponentInChildren<Animator>().SetTrigger("Shockwave");
                    Instantiate(controller.korosData.attackOnePrefab, spawnAt, Quaternion.identity);
                    spawnedCount++;
                }
            }
            
            yield return new WaitForSeconds(spawnShockwaveEvery);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!listeningForCollision || collision.gameObject.CompareTag("Projectile") || hitSomething) return;

        if (collision.gameObject.TryGetComponent<ITakeDamage>(out ITakeDamage damageable))
        {
            damageable.TakeDamage(collision.contacts[0].point, Color.white, dashAttackDamageOutput, true);
        }

        hitSomething = true;

        if (collision.gameObject.CompareTag("Player"))
        {
            controller.StartCoroutine(controller.Shove(collision.gameObject.GetComponent<PlayerController>(), collision.gameObject));
            controller.SwitchState(EnemyBossBaseController.BossStates.Wander);
        }
    }

    public void PlaySound(AudioClip clip) // Allows the enemy to play sounds
    {
        audioSource.PlayOneShot(clip);
    }
}
