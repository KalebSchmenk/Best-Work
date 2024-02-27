using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThormBossShellState : EnemyBossBaseState
{
    ThormBossAIController controller;

    public override void EnterState(EnemyBossBaseController baseController)
    {
        print("Thorm boss entered shell state!");

        controller = (ThormBossAIController)baseController;

        controller.softBodyShields.ForEach(shield => shield.Hide());

        controller.SpawnEnemies();

        StartCoroutine(HideFor());
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        print("Thorm boss exiting shell state!");

        Destroy(this);
    }

    private IEnumerator HideFor()
    {
        yield return new WaitForSeconds(controller.waitInShellFor);

        controller.softBodyShields.ForEach(shield => { shield.UnHide(); shield.Regenerate(); } );

        controller.SwitchState(typeof(ThormBossWanderState));
    }
}