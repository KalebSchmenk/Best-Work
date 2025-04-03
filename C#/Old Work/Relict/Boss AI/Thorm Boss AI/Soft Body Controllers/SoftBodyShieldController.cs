using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static ITakeDamage;

public class SoftBodyShieldController : MonoBehaviour, ITakeDamage
{
    public float health = 50f;
    public Collider defendingCollider;
    private float startingHealth;

    #region Events
    public AboutToTakeDamage AboutToBeDamagedEvent;
    public AboutToTakeDamage AboutToBeDamaged { get => AboutToBeDamagedEvent; set => AboutToBeDamagedEvent = value; }

    public FinalDamage DamageToBeTaken;
    public FinalDamage BroadcastDamageToBeTaken { get => DamageToBeTaken; set => DamageToBeTaken = value; }
    #endregion

    private void Start()
    {
        startingHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0f)
        {
            print("Shield broken!");
            defendingCollider.enabled = true;
            this.gameObject.SetActive(false);
        }
    }

    public void Regenerate()
    {
        health = startingHealth;
        defendingCollider.enabled = false;
    }

    public void Hide()
    {
        this.gameObject.GetComponent<Collider>().enabled = false;
        defendingCollider.enabled = false;

        // For prototyping we turn off the renderers
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        defendingCollider.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void UnHide()
    {
        if (this.gameObject.activeSelf == false) this.gameObject.SetActive(true);
        this.gameObject.GetComponent<Collider>().enabled = true;
        defendingCollider.enabled = true;
        
        // For prototyping we turn back on the renderers
        this.gameObject.GetComponent<MeshRenderer>().enabled = true;
        defendingCollider.gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    #region Damage
    public void TakeDamage(Vector3 damageLocation, Color damageNumberColor, float damage, bool invokeTookDamageEvent)
    {
        float blankCritVal = -1;
        float blankDamageMul = -1; // Blank values so that we can still invoke event

        if (invokeTookDamageEvent) AboutToBeDamaged?.Invoke(ref damage, ref blankCritVal, ref blankDamageMul);

        print("Damage dealt to shield: " + damage);

        DamageNumberSpawner.SpawnDamageNumber(damageLocation, damage, damageNumberColor);
        health -= damage;
    }

    public void TakeDamage(Vector3 damageLocation, Color damageNumberColor, float damage, float baseCritChance, float critDamageMultiplier)
    {
        AboutToBeDamaged?.Invoke(ref damage, ref baseCritChance, ref critDamageMultiplier);

        var critValue = CritChanceController.instance.TryCritHit(damage, baseCritChance, critDamageMultiplier);

        if (critValue == -1)
        {
            DamageToBeTaken?.Invoke(ref damage);
            DamageNumberSpawner.SpawnDamageNumber(damageLocation, damage, damageNumberColor);
            health -= damage; // Normal Damage
        }
        else
        {
            print("Crit damage dealt to shield: " + critValue);
            DamageToBeTaken?.Invoke(ref critValue);
            DamageNumberSpawner.SpawnDamageNumber(damageLocation, critValue, damageNumberColor);
            health -= critValue; // Crit damage
        }
    }

    public void TakeDamage(Vector3 damageLocation, Color damageNumberColor, float damage, float minDamage)
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
