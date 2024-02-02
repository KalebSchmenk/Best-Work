using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KorosBossIdleState : EnemyBossBaseState
{
    KorosBossAIController controller; // Controller ref


    public override void EnterState(EnemyBossBaseController baseController)
    {
        print("Koros boss entered idle state!");

        controller = (KorosBossAIController)baseController;

        StartCoroutine(SwitchToAttackIn());
    } 

    public override void UpdateState()
    {
        //print("Updating idle state");
    }

    public override void ExitState()
    {
        print("Koros boss exited idle state!");

        Destroy(this);
    }

    IEnumerator SwitchToAttackIn()
    {
        yield return new WaitForSeconds(5f);
        controller.SwitchState(EnemyBossBaseController.BossStates.Attack);
    }
}
