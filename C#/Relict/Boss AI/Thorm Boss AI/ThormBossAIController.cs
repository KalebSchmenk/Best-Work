using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using static ITakeDamage;

public class ThormBossAIController : EnemyBossBaseController
{
    // Health
    [Header("")]
    [Header("Health Settings")]
    public float maxHealth = 1000f;
    public float health;
    public TextMeshProUGUI healthText;
    [SerializeField] private FloatingHealthbar healthbar;
    [Header("")]

    // Shield Info
    [Header("Shield Refs")]
    public List<SoftBodyShieldController> softBodyShields = new List<SoftBodyShieldController>();
    [Header("")]

    // Data
    [Header("Data")]
    public BossData thormData;
    public LayerMask groundMask;
    [Header("")]

    // AI
    [Header("AI")]
    public NavMeshAgent navAgent;
    public float randomWanderRadius = 15f;
    [Header("")]

    // Shell settings
    [Header("Shell Settings")]
    public float waitInShellFor = 15f;
    [Header("")]

    // Lightning settings
    [Header("Lightning Settings")]
    public float spawnLightningEvery = 4f;
    [Header("")]

    // Enemies
    [Header("Enemy Spawning")]
    public List<GameObject> enemiesToSpawnInWander = new List<GameObject>();
    public List<Transform> enemySpawnTransforms = new List<Transform>();
    private List<Vector3> enemySpawnLocations = new List<Vector3>();
    public GameObject vfxToSpawnUnderEnemy;
    [Header("")]

    // Boss died
    [Header("Boss Death Event")]
    public UnityEvent OnBossDeath;
    [NonSerialized] public bool isDead = false;
    [Header("")]

    public GameObject lightningAttack;
    public GameObject groundSlam;

    #region Interface Implementations
    // Interface event implementation
    public ITakeDamage.AboutToTakeDamage AboutToTakeDamageEvent;
    public override ITakeDamage.AboutToTakeDamage AboutToBeDamaged { get => AboutToTakeDamageEvent; set => AboutToTakeDamageEvent = value; }

    public FinalDamage DamageToBeTaken;
    public override ITakeDamage.FinalDamage BroadcastDamageToBeTaken { get => DamageToBeTaken; set => DamageToBeTaken = value; }

    // Interface status effect list implementation
    public List<StatusEffectBase> statusEffects;
    public override List<StatusEffectBase> statusEffectBases { get => statusEffects; set => statusEffects = value; }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthText.text = (health.ToString("#; #; 0") + "/" + maxHealth.ToString("#; #; 0"));

        if (navAgent == null) navAgent = this.GetComponent<NavMeshAgent>();

        // Saves og vectors as the children move with the boss which in turn would move the spawn locations for the enemies
        enemySpawnTransforms.ForEach(s => enemySpawnLocations.Add(s.position));
        enemySpawnTransforms.Clear();

        //SwitchState(typeof(ThormBossWanderState));
        SwitchState(typeof(ThormAttackState));
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();
        
        CheckHealth();
    }

    // Checks boss health and invokes event if boss died
    private void CheckHealth()
    {
        healthbar.UpdateHealthbar(health, maxHealth);

        if (isDead)
        {
            health = 0;
            healthText.text = (health.ToString("#; #; 0") + "/" + maxHealth.ToString("#; #; 0"));
            return;
        }

        if (health <= 0)
        {
            health = 0;
            healthText.text = (health.ToString("#; #; 0") + "/" + maxHealth.ToString("#; #; 0"));

            isDead = true;

            //SwitchState(BossStates.Dead);

            OnBossDeath?.Invoke();
            return;
        }

        healthText.text = (health.ToString("#; #; 0") + "/" + maxHealth.ToString("#; #; 0"));
    }

    // Switches state to passed in state
    public void SwitchState(System.Type state)
    {
        if (currentState != null) currentState.ExitState();

        currentState = gameObject.AddComponent(state) as EnemyBossBaseState;

        currentState.EnterState(this);
    }

    public void RegenerateShields()
    {
        foreach(var shield in softBodyShields)
        {
            shield.gameObject.SetActive(true);
            shield.Regenerate();
        }
    }

    // Spawns enemies
    public void SpawnEnemies()
    {
        GameObject enemy;
        GameObject vfx;
        for (int i = 0; i < enemiesToSpawnInWander.Count; i++)
        {
            try
            {
                enemy = Instantiate(enemiesToSpawnInWander[i], enemySpawnLocations[i], Quaternion.identity);
                vfx = Instantiate(vfxToSpawnUnderEnemy, enemy.transform.position, Quaternion.identity);
            }
            catch (NullReferenceException e)
            {
                Debug.LogError("Failed to spawn an enemy due to null reference. Could it be missing enemy to spawn or missing spawn location?");
                Console.WriteLine(e.Message); // Adding this line to avoid the warning in console - Noah
            }
        }
    }

    #region Damage and Effects
    public override void AddStatusEffect(StatusEffectData statusEffectData)
    {
        bool foundCopy = false;
        foreach (var effect in statusEffectBases)
        {
            if (effect.data.GetType() == statusEffectData.GetType())
            {
                foundCopy = true;
                break;
            }
        }

        if (!foundCopy)
        {
            var statusEffectObj = Instantiate(statusEffectData.StatusEffectPrefab, this.transform);
            statusEffectObj.transform.position = this.transform.position;

            var effectType = statusEffectObj.GetComponent<StatusEffectBase>();
            statusEffectBases.Add(effectType);
            EnemyEvents.OnEnemyAddStatusEffect?.Invoke(this, effectType, this);
        }
    }

    public override void RemoveStatusEffect(StatusEffectBase statusEffect)
    {
        foreach (var effect in statusEffects)
        {
            if (effect.GetType() == statusEffect.GetType())
            {
                statusEffects.Remove(effect);
                EnemyEvents.OnEnemyRemovedStatusEffect?.Invoke(this, effect, this);
                break;
            }
        }
    }

    // Damages enemy
    public override void TakeDamage(Vector3 damageLocation, Color damageNumberColor, float damage, bool invokeEvent)
    {
        float blankCritVal = -1;
        float blankDamageMul = -1; // Blank values so that we can still invoke event

        if (invokeEvent) AboutToBeDamaged?.Invoke(ref damage, ref blankCritVal, ref blankDamageMul);

        print("Damage taken: " + damage);

        DamageNumberSpawner.SpawnDamageNumber(damageLocation, damage, damageNumberColor);
        health -= damage;
    }

    // Damages enemy
    public override void TakeDamage(Vector3 damageLocation, Color damageNumberColor, float damage, float minDamage)
    {
        // boss shouldn't fall off the map
    }

    // Damages enemy
    public override void TakeDamage(Vector3 damageLocation, Color damageNumberColor, float damage, float baseCritChance, float critDamageMultiplier)
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
            print("Crit damage dealt to enemy: " + critValue);
            DamageToBeTaken?.Invoke(ref critValue);
            DamageNumberSpawner.SpawnDamageNumber(damageLocation, critValue, damageNumberColor);
            health -= critValue; // Crit damage
        }
    }
    #endregion
}
