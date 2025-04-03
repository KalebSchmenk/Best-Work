using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusEffectBase : MonoBehaviour
{
    public delegate void OnStatusEffectAdded();
    public OnStatusEffectAdded OnEffectAdded;

    public delegate void OnStatusEffectRemoved();
    public OnStatusEffectAdded OnEffectRemoved;

    public delegate void OnStatusEffectDamage(float damage); // When this status effect does its DOT to the effected parent
    public OnStatusEffectDamage OnEffectOutputDamage;

    [NonSerialized] public GameObject parent;

    protected ITakeDamage damageable;
    protected IEffectable effectable;

    public StatusEffectData data;

    public GameObject effectParticles;
    private GameObject spawnedparticles;

    private void Start()
    {
        AddEffect();
    }

    private void OnDestroy()
    {
        RemoveEffect();
    }

    // On status effect added
    public virtual void AddEffect() 
    {
        OnEffectAdded?.Invoke();

        parent = this.gameObject.transform.parent.gameObject;
        damageable = parent.GetComponent<ITakeDamage>();
        effectable = parent.GetComponent<IEffectable>();

        SpawnVFX();

    }

    public virtual void RemoveEffect() 
    {
        data = null;
        _currentEffectTime = 0f;
        _nextTickTime = 0f;

        if (effectParticles != null) Destroy(spawnedparticles);

        OnEffectRemoved?.Invoke();
    }

    protected void SpawnVFX()
    {
        if(effectParticles != null)
        {
            spawnedparticles = Instantiate(effectParticles.gameObject, parent.gameObject.transform);
        }
    }

    protected void RemoveVFX()
    {
        Destroy(spawnedparticles);
    }

    public void Update()
    {
        OnTick();
    }

    private float _currentEffectTime = 0f;
    private float _nextTickTime = 0f;
    public void OnTick()
    {
        _currentEffectTime += Time.deltaTime;

        if (_currentEffectTime >= data.Lifetime) RemoveEffect();

        if (data == null) return;

        if(data.DamageOverTime != 0 && _currentEffectTime > _nextTickTime)
        {
            _nextTickTime += data.TickSpeed;
            if (data.DamageOverTime > 0) 
            {
                OnEffectOutputDamage?.Invoke(data.DamageOverTime);
                damageable.TakeDamage(this.transform.position, data.damageNumberColor, data.DamageOverTime, true);
            }
        }
    }
}
