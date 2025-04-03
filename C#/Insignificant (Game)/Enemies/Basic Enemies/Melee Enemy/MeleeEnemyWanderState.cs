using GLTFast.Schema;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.Image;

public class MeleeEnemyWanderState : BaseEnemyState
{
    private NavMeshAgent agent;

    /// <summary>
    /// Wander state setup
    /// </summary>
    /// <param name="controller"></param>
    public override void EnterState(BaseEnemyController controller)
    {
        base.controller = controller;

        //print("Melee enemy entered wander state");
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Wander());
    }

    /// <summary>
    /// Update callback for the state. Inform the animator of our current speed.
    /// </summary>
    public override void UpdateState()
    {
        //print("Melee enemy updating wander state");
        controller.animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    /// <summary>
    /// Cleanup anything we did before we exit as the current state. Kill our coroutines.
    /// </summary>
    public override void ExitState()
    {
        //print("Melee enemy exiting wander state");
        StopAllCoroutines();
    }

    /// <summary>
    /// Wander around the navmesh.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Wander()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            var pos = RandomNavSphere(this.transform.position, 10f, -1);
            agent.SetDestination(pos);
        }
    }

    /// <summary>
    /// Gets a random point on the navmesh given an origin point, an allowed distance, and a layer.
    /// </summary>
    /// <param name="origin">Original pos.</param>
    /// <param name="dist">Allowed distance from pos.</param>
    /// <param name="layermask">Layer(s) to use.</param>
    /// <returns></returns>
    private Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
