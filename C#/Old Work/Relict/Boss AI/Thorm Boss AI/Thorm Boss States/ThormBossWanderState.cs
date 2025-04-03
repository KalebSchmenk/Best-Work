using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ThormBossWanderState : EnemyBossBaseState
{
    ThormBossAIController controller;
    NavMeshAgent controllerAgent;
    private float wanderRadius;
    private Vector3 goToTarget;
    private Coroutine wanderCoroutine;
    private ThormLightningAttackState lightningState;

    bool isMoving = false;

    public override void EnterState(EnemyBossBaseController baseController)
    {
        print("Thorm boss entered wander state!");

        controller = (ThormBossAIController)baseController;
        controllerAgent = controller.navAgent;
        wanderRadius = controller.randomWanderRadius;

        lightningState = this.AddComponent<ThormLightningAttackState>();
        lightningState.EnterState(baseController);

        StartWandering();
        StartCoroutine(SpawnLightningEvery());

        StartCoroutine(StartAttackIn());
    }

    public override void UpdateState()
    {
        if (isMoving)
        {
            if (Vector3.Distance(goToTarget, controller.transform.position) < 3f)
            {
                StopWandering();
            }
        }
    }

    public override void ExitState()
    {
        print("Thorm boss exited wander state!");

        StopAllCoroutines();

        lightningState.ExitState();

        Destroy(this);
    }

    private void StartWandering()
    {
        goToTarget = RandomNavmeshPoint();

        controllerAgent.SetDestination(goToTarget);

        isMoving = true;

        wanderCoroutine = StartCoroutine(WanderFor());
    }

    private void StopWandering()
    {
        isMoving = false;

        StopCoroutine(wanderCoroutine);

        StartCoroutine(WanderCooldown());
    }

    private Vector3 RandomNavmeshPoint()
    {
        Vector3 randomDir = new Vector3();
        NavMeshHit hit;
        while (true)
        {
            randomDir = Random.insideUnitSphere * wanderRadius;

            randomDir += controller.transform.position;

            if (NavMesh.SamplePosition(randomDir, out hit, wanderRadius, 1))
            {
                randomDir = hit.position;
                break;
            }
        }
        return randomDir;
    }

    // In case never got close to target
    private IEnumerator WanderFor()
    {
        yield return new WaitForSeconds(12f);
        StopWandering();
    }

    private IEnumerator WanderCooldown()
    {
        yield return new WaitForSeconds(3f);
        StartWandering();
    }

    private IEnumerator SpawnLightningEvery()
    {
        while (true)
        {
            lightningState.SpawnLightning();
            yield return new WaitForSeconds(controller.spawnLightningEvery);    
        }
    }

    private IEnumerator StartAttackIn()
    {
        yield return new WaitForSeconds(20f);
        controller.SwitchState(typeof(ThormAttackState));
    }
}
