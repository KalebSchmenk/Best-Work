using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyAttackState : BaseEnemyState
{
    public int damageOutput = 5;
    public float chaseSpeed = 5f;
    public GameObject damageSphere;

    private GameObject player;
    private NavMeshAgent agent;

    private bool attacking = false;

    /// <summary>
    /// Attack state setup
    /// </summary>
    /// <param name="controller">Melee enemy controller. Owner of the state.</param>
    public override void EnterState(BaseEnemyController controller)
    {
        this.controller = controller;
        //print($"Melee enemy entered attack state");

        controller.animator.SetTrigger("BattleIdle");
        controller.animator.SetBool("BattleIdle", true);
        controller.animator.SetBool("Idle", false);

        player = GameManager.Instance.Player;
        agent = this.GetComponent<NavMeshAgent>();

        StartCoroutine(TryAttack());
    }

    /// <summary>
    /// Update callback for this state
    /// </summary>
    public override void UpdateState()
    {
        //print("Melee enemy updating attack state");

        controller.animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    /// <summary>
    /// State cleanup before ending this state's run as current state.
    /// </summary>
    public override void ExitState()
    {
        //print("Melee enemy exiting attack state");
        controller.animator.SetTrigger("Idle");
        controller.animator.SetBool("BattleIdle", false);
        controller.animator.SetBool("Idle", true);

        controller.OnFinishAttack?.Invoke();

        StopAllCoroutines();
    }

    /// <summary>
    /// Try to attack the player if we are allowed by the blackboard.
    /// </summary>
    /// <returns></returns>
    private IEnumerator TryAttack()
    {
        while (true)
        {
            // If we are not close to the player get closer.
            if (Vector3.Distance(this.transform.position, player.transform.position) > 4f)
            {
                agent.SetDestination(player.transform.position);
            }
            else
            {
                // Query the blackboard if we can attack the player
                var canAttack = BasicEnemyBlackboard.Instance.IsPlayerOpenToAttack(controller);

                // If we can, begin the behavior
                if (canAttack)
                {
                    StartCoroutine(MoveToAttack());
                }
                else // If we cant just stand still and watch.
                {
                    if (!attacking) agent.SetDestination(this.transform.position);

                    var dir = player.transform.position - this.transform.position;
                    dir.y = 0f;
                    this.transform.rotation = Quaternion.LookRotation(dir);
                }
            }
            yield return null;
        }
    }

    /// <summary>
    /// Move to attach the player
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveToAttack()
    {
        attacking = true;
        while (true)
        {
            // Get close the player
            agent.SetDestination(player.transform.position);
            yield return null;

            // If we are close enough do a random attack
            if (Vector3.Distance(this.transform.position, player.transform.position) < 1f)
            {
                var ran = UnityEngine.Random.Range(0, 1);

                if (ran == 1)
                {
                    controller.animator.SetTrigger("Attack1");
                }
                else
                {
                    controller.animator.SetTrigger("Attack2");
                }

                yield return new WaitForSeconds(4f);
                
                // Tell the blackboard we are done attacking and let someone else go 
                controller.OnFinishAttack?.Invoke();
                attacking = false;
            }
        }
    }
}
