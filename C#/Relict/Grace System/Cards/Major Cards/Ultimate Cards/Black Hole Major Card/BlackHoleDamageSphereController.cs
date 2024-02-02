using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlackHoleDamageSphereController : MonoBehaviour
{
    public float spawnDamageSphereEvery = 2.5f;
    public float damageOutput = 15f;
    public SphereCollider colliderRef;

    private float damageRadius;
    
    private void Start()
    {
        StartCoroutine(SpawnDamageSphere());
        damageRadius = colliderRef.radius;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnDamageSphere()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDamageSphereEvery);

            var hits = Physics.OverlapSphere(this.transform.position, damageRadius);
            foreach (var hit in hits)
            {
                if (hit.gameObject.CompareTag("Enemy"))
                {
                    if (hit.gameObject.TryGetComponent<ITakeDamage>(out ITakeDamage damageable))
                    {
                        damageable.TakeDamage(hit.transform.position, Color.white, damageOutput, true);
                    }
                }
            }
        }
    }
}
