using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThormAttackState : EnemyBossBaseState
{
    EnemyBossBaseState attackState;
    ThormBossAIController controller;

    [NonSerialized] public Transform targetField; 
    public Transform target
    {
        get
        {
            if (targetField == null)
            {
                return GameManager.instance.player.transform;
            }
            else
            {
                return targetField;
            }
        }
        set { targetField = value; }
    }

    [NonSerialized] public List<Transform> targetOverrides = new List<Transform>();

    private void OnEnable()
    {
        PlayerEvents.OnAbilityUsed += UpdateTargetToDecoy;
    }

    private void OnDisable()
    {
        print("Unsubbing from event");

        PlayerEvents.OnAbilityUsed -= UpdateTargetToDecoy;
    }


    public override void EnterState(EnemyBossBaseController baseController)
    {
        print("Thorm boss entered attack state!");

        controller = (ThormBossAIController)baseController;

        target = GameManager.instance.player.transform;

        ChooseRandomAttack();
    }

    public override void UpdateState()
    {
        attackState.UpdateState();
    }

    public override void ExitState()
    {
        print("Thorm boss exited attack state!");

        attackState.ExitState();

        Destroy(this);
    }

    private void ChooseRandomAttack()
    {
        print("Choosing random attack");

        if (attackState != null) attackState.ExitState();

        var random = UnityEngine.Random.Range(0, 1);

        switch (random)
        {
            case 0:
                attackState = this.AddComponent<GroundSlamAttackState>();
                GroundSlamAttackState groundSlam = (GroundSlamAttackState)attackState;
                groundSlam.controller = this;
                break;
        }

        attackState.EnterState(controller);
    }

    public void AttackDone(System.Type state)
    {
        controller.SwitchState(state);
    }

    #region Target Updating
    public void UpdateTargetToDecoy(MajorCardBase card)
    {
        if (card.GetType() == typeof(LunarWarpMajorCard))
        {
            LunarWarpMajorCard lunarCard = card as LunarWarpMajorCard;
            OverrideTarget(lunarCard.spawnedDecoy.transform);
            RemoveTargetOverride(lunarCard.spawnedDecoy.transform, 4.9f);
        }

        else if (card.GetType() == typeof(TrickOfTheLight))
        {
            TrickOfTheLight TrickCard = card as TrickOfTheLight;

            foreach (Transform decoy in TrickCard.spawnedDecoy)
            {
                OverrideTarget(decoy);
                RemoveTargetOverride(decoy, TrickCard.decoyDuration - 0.1f);
            }
        }
    }

    protected virtual IEnumerator RemoveTargetOverride(Transform target, float timer)
    {
        yield return new WaitForSeconds(timer);
        if (this.target == target)
        {
            target = GameManager.instance.player.transform;
        }
        else if (targetOverrides.Contains(target))
        {
            targetOverrides.Remove(target);
        }
    }

    public void OverrideTarget(Transform target)
    {
        print("Overriding Target");
        if (this.target == null)
        {
            this.target = target;
        }
        else if ((target.transform.position - this.transform.position).sqrMagnitude <
            (this.target.transform.position - this.transform.position).sqrMagnitude)
        {
            targetOverrides.Add(this.target);
            this.target = target;
        }
        else
        {
            targetOverrides.Add(target);
        }

    }
    #endregion
}
