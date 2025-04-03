using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatanicSmiteController : MonoBehaviour
{
    public StatusEffectData hellFire;

    private void OnTriggerStay(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.CompareTag("Enemy"))
        {
            if (obj.TryGetComponent<IEffectable>(out IEffectable effectable))
            {
                effectable.AddStatusEffect(hellFire); // If effectable in trigger, give sunburn
            }
        }
    }
}
