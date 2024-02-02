using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements.Experimental;

public class RadiantRushMajorCard : MajorCardBase
{
    [Header("Card References")]
    public bool isDashing = false;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashPower;
    [SerializeField] private float damageOutput = 2f;
    [SerializeField] private float sphereRadius;
    [SerializeField] LayerMask everythingLayerMask;
    [SerializeField] private StatusEffectData sunburnEffect;

    private Vector3 playerVelocity;
    PlayerController controller;
    CharacterController characterController;


    public override void OnAdd()
    {
        base.OnAdd();

        controller = player.GetComponent<PlayerController>();
        characterController = player.GetComponent<CharacterController>();
    }

    public override void AbilityKeyDown()
    {
        if (GetCooldown()) return; // Guard clause if we are cooling down - return

        print("Dash key down");
    }

    public override void AbilityKeyUp()
    {
        if (GetCooldown()) return; // Guard clause if we are cooling down - return

        Rush();

        PlayerEvents.OnAbilityUsed?.Invoke(this);

        StartCooldown();    // ability cooldown
    }

    public override void OnRemove()
    {
        base.OnRemove();
    }

    private void Rush()
    {
        Debug.Log("Rush function called");
        StartCoroutine(RadiantRush());
    }

    IEnumerator RadiantRush()
    {
        isDashing = true;

        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, sphereRadius);

        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                RaycastHit hit;

                Vector3 spawnRayFrom = player.transform.position;
                spawnRayFrom.y += 1.25f;
                Vector3 enemyPos = collider.gameObject.transform.position;
                enemyPos.y += 0.25f;

                // If ray hit enemy
                Debug.DrawLine(spawnRayFrom, enemyPos, Color.green, 10f);
                if (Physics.Raycast(spawnRayFrom, enemyPos - spawnRayFrom, out hit, Mathf.Infinity, everythingLayerMask, QueryTriggerInteraction.Ignore))
                {
                    print("Hit " + hit.collider.gameObject);
                    if (GameObject.ReferenceEquals(collider.gameObject, hit.collider.gameObject))
                    {
                        // Deal Damage
                        if (collider.gameObject.TryGetComponent<ITakeDamage>(out ITakeDamage damageable))
                        {
                            damageable.TakeDamage(enemyPos, sunburnEffect.damageNumberColor, damageOutput, true);
                        }

                        // Try for status effect
                        if (collider.gameObject.TryGetComponent<IEffectable>(out IEffectable effectable))
                        {
                            bool foundCopy = false;
                            foreach (var effect in effectable.statusEffectBases)
                            {
                                if (effect.GetType() == typeof(SunburnStatusEffect))
                                {
                                    print("Enemy already has hellfire! Not adding another.");
                                    foundCopy = true;
                                    break;
                                }
                            }

                            if (!foundCopy)
                            {
                                effectable.AddStatusEffect(sunburnEffect);
                            }
                        }
                    }
                }
            }
        }

        controller.playerVelocity = new Vector3(player.transform.forward.x * dashPower, 0f, player.transform.forward.z * dashPower);
        yield return new WaitForSeconds(dashTime);
        controller.playerVelocity = Vector3.zero;
        yield return new WaitForSeconds(coolDownTimer);
        isDashing = false;
    }
}
