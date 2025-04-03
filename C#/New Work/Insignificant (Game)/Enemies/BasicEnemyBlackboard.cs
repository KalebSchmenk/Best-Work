using UnityEngine;

/// <summary>
/// DESIGN PATTERN: Blackboard
/// </summary>
public class BasicEnemyBlackboard : Singleton<BasicEnemyBlackboard>
{
    // Event when an enemy is spotted
    public delegate void PlayerSpotted();
    public static PlayerSpotted OnPlayerSpotted;

    // The enemy currently attacking the player
    public BaseEnemyController CurrentAttackingEnemy {  get; private set; }

    // bool whether or not an enemy is attacking the player
    private bool enemyAttackingPlayer = false;
    
    /// <summary>
    /// Is the player able to be attacked?
    /// </summary>
    /// <param name="enemy">Enemy asking the question</param>
    /// <returns>True if player can be attacked, false if not</returns>
    public bool IsPlayerOpenToAttack(BaseEnemyController enemy)
    {
        if (!enemyAttackingPlayer)
        {
            // Assume enemy WILL attack player
            enemyAttackingPlayer = true;
            enemy.OnFinishAttack += AttackEnd;
            CurrentAttackingEnemy = enemy;
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// When an enemy attack ends
    /// </summary>
    private void AttackEnd()
    {
        enemyAttackingPlayer = false;
        CurrentAttackingEnemy.OnFinishAttack -= AttackEnd;
    }
}
