using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiantSphereController : MonoBehaviour
{
    public StatusEffectData sunburnEffect;

    private void OnTriggerStay(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.CompareTag("Enemy"))
        {
            if (obj.TryGetComponent<IEffectable>(out IEffectable effectable))
            {
                effectable.AddStatusEffect(sunburnEffect); // If effectable in trigger, give sunburn
            }
        }
    }
}
