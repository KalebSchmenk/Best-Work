using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellishInfernoController : MonoBehaviour
{
    [NonSerialized] public float damage = 10f;
    [NonSerialized] public float enableDamageIn = 1f;
    [NonSerialized] public float tornadoSpeed;
    [NonSerialized] public StatusEffectData HellfireStatusEffectData;
    [NonSerialized] public StatusEffectData DazedStatusEffectData;
    [NonSerialized] public bool CanStun;


    [SerializeField] CapsuleCollider capsuleCollider;
    [SerializeField] MeshRenderer meshRenderer;

    public AnimationCurve DistanceVersusSpeed;

    private float _initialDistanceToTarget;
    private float _distanceToTarget;
    private float tempSpeed;
    public Vector3 destination;
    public void Start()
    {
        Debug.Log(destination);
        Debug.Log(transform.position);

        StartCoroutine(EnableDamageIn());
        _initialDistanceToTarget = (destination - transform.position).magnitude;
        tempSpeed = 0;
    }

    public void Update()
    {
        // Calculate our distance from target
        Vector3 deltaPosition = destination - transform.position;
        _distanceToTarget = deltaPosition.magnitude;

        if (Mathf.Abs(_distanceToTarget) < .05f)
            Destroy(gameObject);

        // Update our speed based on our distance from the target
        tempSpeed = DistanceVersusSpeed.Evaluate((_initialDistanceToTarget - _distanceToTarget) / _initialDistanceToTarget) *tornadoSpeed;

        // If we need to move father than we can in this update, then limit how much we move
        if (_distanceToTarget > tempSpeed)
            deltaPosition = deltaPosition.normalized * tempSpeed;

        // Set our position
        transform.position += deltaPosition * tempSpeed * Time.deltaTime;
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
                IEffectable effectableInterface;
                obj.TryGetComponent<IEffectable>(out effectableInterface);

                if (effectableInterface != null)
                {
                    Debug.Log("Status effects applied from Inferno");
                    effectableInterface.AddStatusEffect(HellfireStatusEffectData);
                    
                    if(CanStun)
                    {
                        Debug.Log("Dazed applied from Inferno");

                        effectableInterface.AddStatusEffect(DazedStatusEffectData);
                    }
                }
            }
        }
    }

    private IEnumerator EnableDamageIn()
    {
        yield return new WaitForSeconds(enableDamageIn);

        capsuleCollider.enabled = true;
        //meshRenderer.enabled = true;

        AudioManager.instance.PlaySfx("SolarBeam"); // Plays solar beam sound
    }
}
