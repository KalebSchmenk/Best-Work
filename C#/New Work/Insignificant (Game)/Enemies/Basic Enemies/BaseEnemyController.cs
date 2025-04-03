using UnityEngine;
using System.Collections;
using UnityEngine.AI;

/// <summary>
/// DESIGN PATTERN: State Machine
/// </summary>
public class BaseEnemyController : MonoBehaviour, IShootable
{
    private const float callToFightDistance = 70f;
    
    public delegate void FinishAttack();
    public FinishAttack OnFinishAttack;

    public delegate void EnemyDied(GameObject enemy);
    public EnemyDied OnEnemyDied;

    [SerializeField] private float health = 50f;

    private BaseEnemyState currentState;
    [SerializeField] private BaseEnemyState wanderState;
    [SerializeField] private BaseEnemyState attackState;

    [SerializeField] private float playerDetectRadius;

    public Animator animator;

    private bool dead = false;

    private void OnEnable()
    {
        BasicEnemyBlackboard.OnPlayerSpotted += PlayerDetected;
    }

    private void OnDisable()
    {
        BasicEnemyBlackboard.OnPlayerSpotted -= PlayerDetected;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        wanderState.enabled = false;
        attackState.enabled = false;
        SwitchState(EnemyState.Wander);

        StartCoroutine(PlayerInRangeCheck());
    }

    // Update is called once per frame
    protected void Update()
    {
        if (currentState != null) currentState.UpdateState();
    }

    /// <summary>
    /// When an enemy is shot decrease health by 6 and trigger hit anim.
    /// </summary>
    public virtual void Shot()
    {
        if (dead) return;

        health -= 6f;
        if (health < 0)
        {
            health = 0;
            Die();
        }
        animator.SetTrigger("Hit");
    }

    /// <summary>
    /// When an enemy is killed set anim information, exit our current state and clear the ref, set our death bool,
    /// and disable our NavMeshAgent.
    /// </summary>
    public virtual void Die()
    {
        print("Enemy died");

        animator.SetTrigger("Die");
        animator.SetBool("Dead", true);

        if (currentState != null) currentState.ExitState();
        currentState = null;
        dead = true;
        GetComponent<NavMeshAgent>().enabled = false;

        StartCoroutine(AwaitDeathAnim());
    }

    /// <summary>
    /// Await the death anim before triggering death event
    /// </summary>
    /// <returns></returns>
    private IEnumerator AwaitDeathAnim()
    {
        yield return new WaitForSeconds(5f);
        OnEnemyDied?.Invoke(this.gameObject);
    }

    /// <summary>
    /// Blackboard informing us an enemy has spotted the player. Determine if we should join the fight if we are in call to fight distance.
    /// </summary>
    public virtual void PlayerDetected()
    {
        if (Vector3.Distance(this.transform.position, GameManager.Instance.Player.transform.position) < callToFightDistance) // am I close enough to be called to the fight?
        {
            if (attackState.enabled == false) SwitchState(EnemyState.Attack);
        }
    }

    /// <summary>
    /// Switch this enemies state to the passed in new state.
    /// </summary>
    /// <param name="newState">New state to be in.</param>
    public virtual void SwitchState(EnemyState newState)
    {
        if (dead) return;

        if (currentState != null) currentState.ExitState();

        switch (newState)
        {
            case EnemyState.Wander:
                if (wanderState.enabled == false) 
                {
                    currentState = wanderState;
                    wanderState.enabled = true;
                    attackState.enabled = false;

                    currentState.EnterState(this);
                }
                break;

            case EnemyState.Attack:
                if (attackState.enabled == false) 
                {
                    currentState = attackState;
                    attackState.enabled = true;
                    wanderState.enabled = false;

                    currentState.EnterState(this);
                }
                break;
        } 
    }

    /// <summary>
    /// Check every certain amount of time if the player is nearby and if so tell the blackboard.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayerInRangeCheck()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        while (true)
        {
            yield return new WaitForSeconds(.75f);
            float distance = Vector3.Distance(this.transform.position, player.transform.position);

            if (distance < playerDetectRadius)
            {
                BasicEnemyBlackboard.OnPlayerSpotted?.Invoke();
            }
        }
    }
}

public enum EnemyState
{ 
    Wander,
    Attack
}
