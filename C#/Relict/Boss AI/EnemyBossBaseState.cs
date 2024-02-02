using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossBaseState : MonoBehaviour
{
    public virtual void EnterState(EnemyBossBaseController baseController)
    {
        // Enter a state and pass in the controller for reference
    }
    public virtual void UpdateState()
    {
        // Update the state
    }
    public virtual void ExitState()
    {
        // Exit a state
    }
}
