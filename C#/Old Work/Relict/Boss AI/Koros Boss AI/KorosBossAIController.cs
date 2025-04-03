using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using static ITakeDamage;
using static GameManager;

public class KorosBossAIController : EnemyBossBaseController
{
    // Interface event implementation
    public ITakeDamage.AboutToTakeDamage AboutToTakeDamageEvent;
    public override ITakeDamage.AboutToTakeDamage AboutToBeDamaged { get => AboutToTakeDamageEvent; set => AboutToTakeDamageEvent = value; }

    public FinalDamage DamageToBeTaken;
    public override ITakeDamage.FinalDamage BroadcastDamageToBeTaken { get => DamageToBeTaken; set => DamageToBeTaken = value; }

    public GameObject HitVFX;

    AudioSource audioSource; // Audio source reference

    // Data
    [Header("Data")]
    public BossData korosData;
    [Header("")]


    // Health
    [Header("Health Settings")]
    public float maxHealth = 1000f;
    public float easyHealth = 1250f;
    public float normalHealth = 1800f;
    public float hardHealth = 2500;
    public float health;
    public TextMeshProUGUI healthText;
    [SerializeField] private FloatingHealthbar healthbar;
    [Header("")]


    // Interface status effect list implementation
    public List<StatusEffectBase> statusEffects;
    public override List<StatusEffectBase> statusEffectBases { get => statusEffects; set => statusEffects = value; }


    // Attack
    [Header("Attack Settings")]
    //public float shockwaveAttackChance = 100f;
    public Transform projectileSpawnPos;
    public GameObject crystalProjectile;
    public float startAttackEvery = 15f;
    public float moveToSpawnSpeed = 4.5f;

    [Header("Dash Attack Settings")]
    public float upAndDownSpeed = 8.5f;
    public float dashSetupRotateSpeed = 30f;
    public float dashAtGroundRotateSpeed = 45f;
    public float startDashAfter = 2f;
    public float dashAttackRotateSpeed = 45f;
    public float dashAttackMoveSpeed = 25.5f;
    public float dashAttackDamageOutput = 250f;
    public float pushForce = 65f;

    [Header("Shockwave Attack Settings")]
    public int shockwaveCount = 3;
    public float spawnShockwaveEvery = 1.5f;
    [Header("")]


    // Wander
    [Header("Wander Settings")]
    public float minWanderWaypointSpawn = 10f;
    public float maxWanderWaypointSpawn = 50f;
    public float wanderSpeed = 5f;
    public float wanderUpAndDownSpeed = 8.5f;
    public float wanderWaitAtWaypointFor = 0.01f;
    public float fireProjectileEvery = 5f;
    
    [Header("")]
    public List<GameObject> enemiesToSpawnInWander = new List<GameObject>();
    public List<Transform> enemySpawnTransforms = new List<Transform>();
    private List<Vector3> enemySpawnLocations = new List<Vector3>();
    public GameObject vfxToSpawnUnderEnemy;
    [NonSerialized] public int enteredWanderState = 0;
    [Header("")]


    // Boss died
    [Header("Boss Death Event")]
    public UnityEvent OnBossDeath;
    [NonSerialized] public bool isDead = false;
    [Header("")]

    [Header("Sounds")]
    public AudioClip spawnSound;
    public AudioClip wanderStartSound;
    public AudioClip wanderExitSound;
    public AudioClip prepSound;
    public AudioClip dashAttackSound;
    public AudioClip ambientSound1;
    public AudioClip ambientSound2;
    public AudioClip ambientSound3;
    public AudioClip deathSound;

    private string ObjectiveText = "Kill Justice!";

    public Vector3 startPos;

    private void OnEnable()
    {
        GameManager.OnDifficultyChanged += ChangeDifficulty;
    }

    private void OnDisable()
    {
        GameManager.OnDifficultyChanged -= ChangeDifficulty;
    }

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;

        GameManager.instance.UpdateObjective(ObjectiveText);

        currentState = this.gameObject.AddComponent<KorosBossWanderState>();
        currentState.EnterState(this);

        // Quick difficulty addition
        switch (GameManager.instance.GameDifficulty)
        {
            case GameManager.Difficulty.Easy:
                maxHealth = easyHealth;
                break;

            case GameManager.Difficulty.Normal:
                maxHealth = normalHealth;
                break;

            case GameManager.Difficulty.Hard:
                maxHealth = hardHealth;
                break;
        }

        health = maxHealth;
        healthText.text = (health.ToString("#") + "/" + maxHealth.ToString("#"));

        audioSource = GetComponent<AudioSource>(); // Audio source code

        foreach (Transform trans in enemySpawnTransforms) // Saves og vectors as the children move with the boss which in turn would move the spawn locations for the enemies
        {
            enemySpawnLocations.Add(trans.position);
        }

        PlaySound(spawnSound); // Plays a sound when the boss spawns


        //AudioManager.instance.StopMusic("NormalMusic"); // Stops normal music as the boss fight begins
        //AudioManager.instance.PlayMusic("BossMusic"); // Starts boss music as the boss fight begins
    }

    public virtual void ChangeDifficulty(GameManager.Difficulty newDifficulty)
    {
        print("In justice change dif");
        switch (newDifficulty)
        {
            case Difficulty.Easy:
                maxHealth = easyHealth;
                health = maxHealth;
                break;

            case Difficulty.Normal:
                maxHealth = normalHealth;
                health = maxHealth;
                break;

            case Difficulty.Hard:
                maxHealth = hardHealth;
                health = maxHealth;
                break;
        }

        healthbar.UpdateHealthbar(health, maxHealth);
        healthText.text = (health.ToString("#") + " / " + maxHealth.ToString("#"));
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();

        CheckHealth();
    }

    // Switches state to passed in state
    public void SwitchState(BossStates state)
    {
        currentState.ExitState();

        EnemyBossBaseState newState;

        switch(state)
        {
            case BossStates.Idle:
                newState = this.AddComponent<KorosBossIdleState>();
                break;

            case BossStates.Attack:
                newState = this.AddComponent<KorosBossAttackState>();
                break;

            case BossStates.Wander:
                newState = this.AddComponent<KorosBossWanderState>();
                break;

            case BossStates.Dead:
                newState = this.AddComponent<KorosBossDeadState>();
                //AudioManager.instance.StopMusic("BossMusic"); // Stops boss music when boss dies
                break;

            default:
                newState = this.AddComponent<KorosBossIdleState>();
                break;
        }

        currentState = newState;

        currentState.EnterState(this);
    }

    // Checks boss health and invokes event if boss died
    private void CheckHealth()
    {
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

            SwitchState(BossStates.Dead);

            OnBossDeath?.Invoke();
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

        SpawnVFX(damageLocation);
        healthbar.UpdateHealthbar(health, maxHealth);
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

        SpawnVFX(damageLocation);
        healthbar.UpdateHealthbar(health, maxHealth);
        healthText.text = (health.ToString("#") + " / " + maxHealth.ToString("#"));
    }

    // Adds a status effect to boss
    public override void AddStatusEffect(StatusEffectData effectData)
    {
        bool foundCopy = false;
        foreach (var effect in statusEffectBases)
        {
            if (effect.data.GetType() == effectData.GetType())
            {
                print("Enemy already has " + effect.ToString() + "! Not adding another.");
                foundCopy = true;
                break;
            }
        }

        if (!foundCopy)
        {
            var statusEffectObj = Instantiate(effectData.StatusEffectPrefab, this.transform);
            statusEffectObj.transform.position = this.transform.position;

            var effectType = statusEffectObj.GetComponent<StatusEffectBase>();
            statusEffectBases.Add(effectType);
            EnemyEvents.OnEnemyAddStatusEffect?.Invoke(this, effectType, this);
        }
    }

    // Removes a status effect from boss
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

    // Spawns VFX
    public void SpawnVFX(Vector3 spawnLocal)
    {
        Instantiate(HitVFX.gameObject, spawnLocal, Quaternion.identity, transform);
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

        PlaySound(ambientSound2); // Plays a sound as the whale spawns minions
    }

    public void PlaySound(AudioClip clip) // Allows the enemy to play sounds
    {
        audioSource.PlayOneShot(clip);
    }

    public IEnumerator Shove(PlayerController controller, GameObject player)
    {
        Vector3 shoveDir = (player.transform.position - this.transform.position).normalized;
        controller.canMove = false;
        controller.playerVelocity = new Vector3(shoveDir.x * pushForce, 0f, shoveDir.z * pushForce);
        yield return new WaitForSeconds(1.5f);
        controller.playerVelocity = Vector3.zero;
        controller.canMove = true;
    }
}
