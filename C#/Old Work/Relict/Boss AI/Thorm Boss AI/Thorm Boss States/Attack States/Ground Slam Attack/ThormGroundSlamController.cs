using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ThormGroundSlamController : MonoBehaviour
{
    public GroundSlamAttackState stateController;
    public float pushForce = 65f;
    public float damageOutput = 15f;

    public bool hitPlayer = false;

    private void OnTriggerEnter(Collider other)
    {
        ITakeDamage damageable;

        if (other.CompareTag("Player"))
        {
            if (hitPlayer) return;

            hitPlayer = true;

            damageable = other.GetComponent<ITakeDamage>();

            var damageMultiplier = 65 / Vector3.Distance(other.transform.position, this.transform.position);

            var finalDamage = Mathf.Clamp(damageOutput * damageMultiplier, 100f, 500f);

            damageable.TakeDamage(other.transform.position, Color.white, finalDamage, false);

            StartCoroutine(Shove(other.GetComponent<PlayerController>(), other.gameObject));
        }
        else if (other.gameObject.layer != LayerMask.NameToLayer("Boss") && other.CompareTag("Enemy"))
        {
            damageable = other.GetComponent<ITakeDamage>();

            var damageMultiplier = 10 / Vector3.Distance(other.transform.position, this.transform.position);

            damageable.TakeDamage(other.transform.position, Color.white, damageMultiplier * damageOutput, true);

            IEffectable effectable = other.GetComponent<IEffectable>();

            effectable.AddStatusEffect(GameManager.instance.dazedEffectData);

            ILaunchable launchable = other.GetComponent<ILaunchable>();

            launchable.Launch(other.transform.position - this.transform.position, pushForce, 5f);
        }

        Destroy(this.gameObject, 3.5f);
    }

    private IEnumerator Shove(PlayerController controller, GameObject player)
    {
        Vector3 shoveDir = (player.transform.position - this.transform.position).normalized;
        controller.canMove = false;
        controller.playerVelocity = new Vector3(shoveDir.x * pushForce, 0f, shoveDir.z * pushForce);
        yield return new WaitForSeconds(1.5f);
        controller.playerVelocity = Vector3.zero;
        controller.canMove = true;
    }
}
