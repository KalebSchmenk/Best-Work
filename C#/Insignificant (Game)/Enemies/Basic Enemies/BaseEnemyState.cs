using UnityEngine;

public abstract class BaseEnemyState : MonoBehaviour
{
    protected BaseEnemyController controller;

    public abstract void EnterState(BaseEnemyController controller);
    public abstract void UpdateState();
    public abstract void ExitState();
}
