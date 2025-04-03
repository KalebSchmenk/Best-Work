using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleController : MonoBehaviour
{
    public float pullPower = 6f;
    public StatusEffectData stunEffect;

    private void OnTriggerStay(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.layer != LayerMask.NameToLayer("Boss") && obj.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<IEffectable>(out IEffectable effectable))
            {
                effectable.AddStatusEffect(stunEffect);
            }

            if (other.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                if (Vector3.Distance(this.transform.position, other.transform.position) < 5f)
                {
                    rb.velocity = Vector3.zero;
                    return; // If enemy is already in black hole, dont pull anymore
                }

                // Pull
                var pullDir = this.transform.position - obj.transform.position;
                pullDir.Normalize();

                var force = pullPower * (25 / Vector3.Distance(this.transform.position, obj.transform.position)); // 25 is constant

                rb.AddForce(pullDir * force * 10, ForceMode.Force);
            }  
        }
    }  
}
