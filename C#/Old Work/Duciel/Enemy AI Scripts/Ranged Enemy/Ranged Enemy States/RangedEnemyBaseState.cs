using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedEnemyBaseState : MonoBehaviour
{
    public abstract void EnterState(RangedEnemyController rangedEnemy);

    public abstract void UpdateState();
}
