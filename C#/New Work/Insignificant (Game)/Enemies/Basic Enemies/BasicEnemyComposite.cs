using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DESIGN PATTERN: COMPOSITE
/// </summary>
public class BasicEnemyComposite : BaseEnemyController
{
    public List<BaseEnemyController> enemies = new List<BaseEnemyController>();

    /// <summary>
    /// Switch stored enemies state to passed in state
    /// </summary>
    /// <param name="newState"></param>
    public override void SwitchState(EnemyState newState)
    {
        foreach (BaseEnemyController controller in enemies)
        {
            controller.SwitchState(newState);
        }
    }

    /// <summary>
    /// Simulates each enemy in list being shot
    /// </summary>
    public override void Shot()
    {
        foreach(BaseEnemyController controller in enemies)
        {
            controller.Shot();
        }
    }
}
