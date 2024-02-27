using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerController : MonoBehaviour
{
    public StatusEffectData hellfireEffect;
    private ParticleSystem particle;
    [NonSerialized] public PlayerWeaponController playerWeaponController;
    [NonSerialized] public float duration;

    private void Start()
    {
        particle = GetComponent<ParticleSystem>();
        StartCoroutine(FlamethrowerDuration());
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other == null) return;

        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<IEffectable>(out IEffectable effectable))
            {
                effectable.AddStatusEffect(hellfireEffect);
            }
        }
    }

    private IEnumerator FlamethrowerDuration()
    {
        yield return new WaitForSeconds(duration);
        particle.Stop();
        playerWeaponController.canShoot = true;
        yield return new WaitForSeconds(duration + 10f);
        Destroy(this.gameObject);
    }
}
