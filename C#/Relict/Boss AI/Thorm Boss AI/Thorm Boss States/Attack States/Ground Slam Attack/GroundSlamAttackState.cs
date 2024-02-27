using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundSlamAttackState : EnemyBossBaseState
{
    public ThormAttackState controller;
    ThormBossAIController aiController;

    private float moveUpSpeed = 10f;
    private float moveDownSpeed = 18f;
    ThormGroundSlamController slamController;

    bool stopMoving = false;

    public override void EnterState(EnemyBossBaseController baseController)
    {
        print("Thorm boss entered ground slam attack state!");

        aiController = (ThormBossAIController)baseController;
    }

    public override void UpdateState()
    {
        if (stopMoving) return;

        aiController.navAgent.SetDestination(controller.target.transform.position);

        if (Vector3.Distance(this.transform.position, controller.target.transform.position) < 15f)
        {
            stopMoving = true;
            StartGroundSlam();
        }
    }

    public override void ExitState()
    {
        print("Thorm boss exited ground slam attack state!");

        Destroy(this);
    }

    private void StartGroundSlam()
    {
        aiController.navAgent.enabled = false;

        StartCoroutine(GroundSlam());
    }

    private IEnumerator GroundSlam()
    {
        while (true) // Jump up
        {
            if (this.transform.position.y - controller.target.transform.position.y > 25f) break;
            transform.Translate(Vector3.up * moveUpSpeed * Time.deltaTime);
            yield return null;
        }

        SpawnGroundSlam();

        while (true) // Fall Down
        {
            bool isGrounded = Physics.CheckSphere(aiController.transform.position, .25f, aiController.groundMask, QueryTriggerInteraction.Ignore);
            if (isGrounded) break;
            transform.Translate(Vector3.down * moveDownSpeed * Time.deltaTime);
            yield return null;
        }


        aiController.navAgent.enabled = true;
        controller.AttackDone(typeof(ThormBossShellState));
    }

    public void SpawnGroundSlam()
    {
        var obj = Instantiate(aiController.groundSlam, aiController.transform.position, aiController.transform.rotation, aiController.transform);
        slamController = obj.GetComponent<ThormGroundSlamController>();
        slamController.stateController = this;
    }
}
