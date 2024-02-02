using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarBeamController : MonoBehaviour
{
    [NonSerialized] public float damage = 10f;
    [NonSerialized] public float enableDamageIn = 1f;
    [NonSerialized] public float destroyAfterEnableIn = 10f;

    [SerializeField] CapsuleCollider capsuleCollider;
    [SerializeField] MeshRenderer meshRenderer;

    private void Start()
    {
        StartCoroutine(EnableDamageIn());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            var obj = other.gameObject;


            ITakeDamage damageInterface;
            obj.TryGetComponent<ITakeDamage>(out damageInterface);

            if (damageInterface != null)
            {
                damageInterface.TakeDamage(other.ClosestPoint(transform.position), Color.white, damage, true);
            }
        }
    }

    private IEnumerator EnableDamageIn()
    {
        yield return new WaitForSeconds(enableDamageIn);

        capsuleCollider.enabled = true;
        //meshRenderer.enabled = true;

        AudioManager.instance.PlaySfx("SolarBeam"); // Plays solar beam sound

        StartCoroutine(DestroyIn());
    }

    private IEnumerator DestroyIn()
    {
        yield return new WaitForSeconds(destroyAfterEnableIn);

        Destroy(this.gameObject);
    }
}
